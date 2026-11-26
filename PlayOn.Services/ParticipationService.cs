using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Participation;
using PlayOn.Services.Data.Participation.RequestData;
using PlayOn.Services.Data.Participation.ResponseModels;
using PlayOn.Services.DataBuilders;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.ParticipationRules;

namespace PlayOn.Services
{
    public class ParticipationService : IParticipationService
    {
        private const string MatchClosedMessage =
            "This match is cancelled, finished or already started.";

        private const string ConcurrencyMessage =
            "The match changed while you were doing this. Please try again.";

        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<SportPreference> preferenceRepository;
        private readonly IRepository<Notification> notificationRepository;
        private readonly TimeProvider timeProvider;

        public ParticipationService(
            IRepository<Match> matchRepository,
            IRepository<MatchParticipant> participantRepository,
            IRepository<SportPreference> preferenceRepository,
            IRepository<Notification> notificationRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.participantRepository = participantRepository;
            this.preferenceRepository = preferenceRepository;
            this.notificationRepository = notificationRepository;
            this.timeProvider = timeProvider;
        }

        public async Task<ServiceResponse<JoinMatchResponseModel>>
            JoinAsync(
                ParticipationRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<JoinMatchResponseModel>.Failure(errors);
            }

            var info = await GetInfoAsync(request.MatchId);
            if (info == null || info.IsDeleted)
            {
                return ServiceResponse<JoinMatchResponseModel>.NotFound();
            }

            if (!IsOpenForParticipation(info))
            {
                return ServiceResponse<JoinMatchResponseModel>
                    .Conflict(MatchClosedMessage);
            }

            var participant = await participantRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x =>
                    x.MatchId == info.Id &&
                    x.UserId == request.UserId);

            if (participant != null
                && participant.Status != ParticipantStatus.Cancelled)
            {
                var message = participant.Status == ParticipantStatus.Joined
                    || participant.Status == ParticipantStatus.Waitlisted
                        ? "You are already part of this match."
                        : "You cannot join this match again.";

                return ServiceResponse<JoinMatchResponseModel>
                    .Conflict(message);
            }

            var localDateTime = timeProvider.GetLocalNow().LocalDateTime;

            var ruleError = await CheckJoinRulesAsync(
                request.UserId,
                info,
                localDateTime);

            if (ruleError != null)
            {
                return ServiceResponse<JoinMatchResponseModel>
                    .Failure(new[] { ruleError });
            }

            var joinedCount = await CountByStatusAsync(
                info.Id,
                ParticipantStatus.Joined);

            var status = joinedCount < info.MaxPlayers
                ? ParticipantStatus.Joined
                : ParticipantStatus.Waitlisted;

            var waitlistPosition = status == ParticipantStatus.Waitlisted
                ? await CountByStatusAsync(
                    info.Id,
                    ParticipantStatus.Waitlisted) + 1
                : 0;

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == info.Id);

            if (participant == null)
            {
                await participantRepository.AddAsync(
                    ParticipationDataBuilder.ParticipantToEntity(
                        info.Id,
                        request.UserId,
                        status,
                        localDateTime));
            }
            else
            {
                // Reactivate the previously cancelled participation.
                participant.Status = status;
                participant.JoinedOn = localDateTime;
                participant.StatusChangedOn = localDateTime;
                participant.IsLateCancel = false;
            }

            var isConfirmed = status == ParticipantStatus.Joined
                && match.Status == MatchStatus.Open
                && joinedCount + 1 >= info.MinPlayers;

            if (isConfirmed)
            {
                match.Status = MatchStatus.Confirmed;

                await AddMatchConfirmedNotificationsAsync(
                    info,
                    request.UserId,
                    localDateTime);
            }

            if (!await TrySaveChangesAsync())
            {
                return ServiceResponse<JoinMatchResponseModel>
                    .Conflict(ConcurrencyMessage);
            }

            var responseData = ParticipationDataBuilder.JoinToResponseModel(
                status,
                waitlistPosition,
                isConfirmed);

            return ServiceResponse<JoinMatchResponseModel>
                .Success(responseData);
        }

        public async Task<ServiceResponse<LeaveMatchResponseModel>>
            LeaveAsync(ParticipationRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<LeaveMatchResponseModel>.Failure(errors);
            }

            var info = await GetInfoAsync(request.MatchId);
            if (info == null || info.IsDeleted)
            {
                return ServiceResponse<LeaveMatchResponseModel>.NotFound();
            }

            if (info.OrganiserId == request.UserId)
            {
                return ServiceResponse<LeaveMatchResponseModel>
                    .Conflict("The organiser cannot leave the match. Cancel it instead.");
            }

            if (!IsOpenForParticipation(info))
            {
                return ServiceResponse<LeaveMatchResponseModel>
                    .Conflict(MatchClosedMessage);
            }

            var participant = await FindActiveParticipantAsync(
                info.Id,
                request.UserId,
                asNoTracking: false);

            if (participant == null)
            {
                return ServiceResponse<LeaveMatchResponseModel>
                    .Conflict("You are not part of this match.");
            }

            var nowLocal = timeProvider.GetLocalNow().DateTime;

            var wasJoined = participant.Status == ParticipantStatus.Joined;

            var isLateCancel = wasJoined
                && info.Date.ToDateTime(info.StartsAt) - nowLocal
                    < TimeSpan.FromHours(LateCancelHours);

            participant.Status = ParticipantStatus.Cancelled;
            participant.StatusChangedOn = nowLocal;
            participant.IsLateCancel = isLateCancel;

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == info.Id);

            var notifications = new List<Notification>();

            if (isLateCancel)
            {
                notifications.Add(NotificationDataBuilder.NotificationToEntity(
                    request.UserId,
                    NotificationType.StrikeAdded,
                    $"You left \"{info.Title}\" less than {LateCancelHours} hours before it started. "
                        + $"This counts as a strike for {StrikeWindowDays} days.",
                    info.Id,
                    nowLocal));
            }

            var isPromoted = false;

            if (wasJoined)
            {
                var joinedAfterLeave = await CountByStatusAsync(
                    info.Id,
                    ParticipantStatus.Joined) - 1;

                if (joinedAfterLeave < info.MaxPlayers)
                {
                    var promotion = await PromoteFromWaitlistAsync(info, nowLocal);

                    if (promotion != null)
                    {
                        notifications.Add(promotion);
                        isPromoted = true;
                        joinedAfterLeave++;
                    }
                }

                if (match.Status == MatchStatus.Confirmed
                    && joinedAfterLeave < info.MinPlayers)
                {
                    match.Status = MatchStatus.Open;
                }
            }

            if (notifications.Count > 0)
            {
                await notificationRepository.AddRangeAsync(notifications);
            }

            if (!await TrySaveChangesAsync())
            {
                return ServiceResponse<LeaveMatchResponseModel>
                    .Conflict(ConcurrencyMessage);
            }

            var responseData = ParticipationDataBuilder
                .LeaveToResponseModel(isLateCancel, isPromoted);

            return ServiceResponse<LeaveMatchResponseModel>.Success(responseData);
        }

        public async Task<ServiceResponse<PageResult<MyMatchResponseModel>>>
            GetSinglePageAsync(
                MyMatchesRequestData requestData,
                PageRequestData pageRequestData)
        {
            ArgumentNullException.ThrowIfNull(requestData);
            ArgumentNullException.ThrowIfNull(pageRequestData);

            var validationErrors = ServiceRequestDataValidator.Validate(requestData);
            if (validationErrors.Any())
            {
                return ServiceResponse<PageResult<MyMatchResponseModel>>
                    .Failure(validationErrors);
            }

            var participationQuery = BuildMyMatchesQuery(requestData);

            var page = await GetSinglePageAsync(participationQuery, pageRequestData);

            return ServiceResponse<PageResult<MyMatchResponseModel>>.Success(page);
        }

        private IOrderedQueryable<MatchParticipant> BuildMyMatchesQuery(
            MyMatchesRequestData query)
        {
            var nowLocal = timeProvider.GetLocalNow().DateTime;
            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            var participationQuery = participantRepository
                .Query()
                .Where(x => x.UserId == query.UserId
                    && !x.Match.IsDeleted
                    && x.Match.Event != null);

            return query.Tab switch
            {
                MyMatchesTab.Upcoming => ApplyUpcomingTab(
                    participationQuery,
                    ParticipantStatus.Joined,
                    today,
                    nowTime),

                MyMatchesTab.Waitlist => ApplyUpcomingTab(
                    participationQuery,
                    ParticipantStatus.Waitlisted,
                    today,
                    nowTime),

                MyMatchesTab.Past => ApplyPastTab(
                    participationQuery,
                    today,
                    nowTime),

                _ => ApplyCancelledTab(participationQuery)
            };
        }

        private IOrderedQueryable<MatchParticipant> ApplyUpcomingTab(
            IQueryable<MatchParticipant> query,
            ParticipantStatus status,
            DateOnly today,
            TimeOnly nowTime)
        {
            return query
                .Where(x => x.Status == status
                    && x.Match.Status != MatchStatus.Cancelled
                    && (x.Match.Event!.Venue.Date > today
                        || (x.Match.Event.Venue.Date == today
                            && x.Match.Event.Venue.StartsAt >= nowTime)))
                .OrderBy(x => x.Match.Event!.Venue.Date)
                .ThenBy(x => x.Match.Event!.Venue.StartsAt);
        }

        private IOrderedQueryable<MatchParticipant> ApplyPastTab(
            IQueryable<MatchParticipant> query,
            DateOnly today,
            TimeOnly nowTime)
        {
            return query
                .Where(x => x.Match.Status != MatchStatus.Cancelled
                    && (x.Status == ParticipantStatus.Attended
                        || x.Status == ParticipantStatus.NoShow
                        || (x.Status == ParticipantStatus.Joined
                            && (x.Match.Event!.Venue.Date < today
                                || (x.Match.Event.Venue.Date == today
                                    && x.Match.Event.Venue.StartsAt < nowTime)))))
                .OrderByDescending(x => x.Match.Event!.Venue.Date)
                .ThenByDescending(x => x.Match.Event!.Venue.StartsAt);
        }

        private IOrderedQueryable<MatchParticipant> ApplyCancelledTab(
            IQueryable<MatchParticipant> query)
        {
            return query
                .Where(x => x.Status == ParticipantStatus.Cancelled
                    || x.Match.Status == MatchStatus.Cancelled)
                .OrderByDescending(x => x.Match.Event!.Venue.Date)
                .ThenByDescending(x => x.Match.Event!.Venue.StartsAt);
        }

        private async Task<PageResult<MyMatchResponseModel>>
            GetSinglePageAsync(
                IOrderedQueryable<MatchParticipant> query,
                PageRequestData pageRequestData)
        {
            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(pageRequestData.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(pageRequestData.Page, 1, totalPages);

            var items = await query
                .ThenBy(x => x.MatchId)
                .ThenBy(x => x.UserId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ParticipationDataBuilder.MyMatchProjection())
                .ToListAsync();

            return new PageResult<MyMatchResponseModel>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }

        private async Task<JoinInfoRow?> GetInfoAsync(Guid matchId)
        {
            return await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null)
                .Select(ParticipationDataBuilder.JoinInfoProjection())
                .SingleOrDefaultAsync();
        }

        private bool IsOpenForParticipation(JoinInfoRow info)
        {
            return (info.Status == MatchStatus.Open
                    || info.Status == MatchStatus.Confirmed)
                && info.Date.ToDateTime(info.StartsAt)
                    > timeProvider.GetLocalNow().DateTime;
        }

        private async Task<MatchParticipant?> FindActiveParticipantAsync(
            Guid matchId,
            Guid userId,
            bool asNoTracking = true)
        {
            return await participantRepository
                .Query(asNoTracking)
                .FirstOrDefaultAsync(x => x.MatchId == matchId
                    && x.UserId == userId
                    && (x.Status == ParticipantStatus.Joined
                        || x.Status == ParticipantStatus.Waitlisted));
        }

        private async Task<int> CountByStatusAsync(
            Guid matchId,
            ParticipantStatus status)
        {
            return await participantRepository
                .Query()
                .CountAsync(x => x.MatchId == matchId && x.Status == status);
        }

        private async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await matchRepository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        private async Task AddMatchConfirmedNotificationsAsync(
            JoinInfoRow info,
            Guid joiningUserId,
            DateTime nowUtc)
        {
            var userIds = await participantRepository
                .Query()
                .Where(x => x.MatchId == info.Id
                    && x.Status == ParticipantStatus.Joined)
                .Select(x => x.UserId)
                .ToListAsync();

            userIds.Add(joiningUserId);

            var notifications = userIds
                .Distinct()
                .Select(userId => NotificationDataBuilder.NotificationToEntity(
                    userId,
                    NotificationType.MatchConfirmed,
                    $"\"{info.Title}\" is confirmed: enough players have joined.",
                    info.Id,
                    nowUtc))
                .ToList();

            await notificationRepository.AddRangeAsync(notifications);
        }

        private async Task<Notification?> PromoteFromWaitlistAsync(
            JoinInfoRow info,
            DateTime nowUtc)
        {
            var waitingPlayers = await participantRepository
                .Query(asNoTracking: false)
                .Where(x => x.MatchId == info.Id
                    && x.Status == ParticipantStatus.Waitlisted)
                .OrderBy(x => x.JoinedOn)
                .ThenBy(x => x.MatchId)
                .ThenBy(x => x.UserId)
                .ToListAsync();

            foreach (var candidate in waitingPlayers)
            {
                if (await CheckJoinRulesAsync(candidate.UserId, info, nowUtc) != null)
                {
                    continue;
                }

                candidate.Status = ParticipantStatus.Joined;
                candidate.StatusChangedOn = nowUtc;

                return NotificationDataBuilder.NotificationToEntity(
                    candidate.UserId,
                    NotificationType.Promoted,
                    $"A spot opened up in \"{info.Title}\" and you are now playing.",
                    info.Id,
                    nowUtc);
            }

            return null;
        }

        private async Task<ValidationResult?> CheckJoinRulesAsync(
            Guid userId,
            JoinInfoRow info,
            DateTime nowUtc)
        {
            if (await IsBlockedByStrikesAsync(userId, nowUtc))
            {
                return new ValidationResult(
                    "You have too many late cancellations or no-shows, so you cannot join matches for now.");
            }

            if (!await IsSkillInRangeAsync(userId, info))
            {
                return new ValidationResult(
                    "Your skill level for this sport is outside the range of this match. You can change it in your profile.");
            }

            if (await HasOverlappingBookingAsync(userId, info))
            {
                return new ValidationResult(
                    "You are already in another match at the same time.");
            }

            return null;
        }

        private async Task<bool> IsBlockedByStrikesAsync(
            Guid userId,
            DateTime nowUtc)
        {
            var strikeSince = nowUtc.AddDays(-StrikeWindowDays);

            var strikeDates = await participantRepository
                .Query()
                .Where(x => x.UserId == userId
                    && (x.IsLateCancel || x.Status == ParticipantStatus.NoShow)
                    && x.StatusChangedOn >= strikeSince)
                .Select(x => x.StatusChangedOn)
                .ToListAsync();

            return strikeDates.Count >= StrikesToBlock
                && nowUtc < strikeDates.Max().AddDays(BlockDays);
        }

        private async Task<bool> IsSkillInRangeAsync(
            Guid userId,
            JoinInfoRow info)
        {
            var preference = await preferenceRepository
                .Query()
                .Where(x => x.UserId == userId && x.SportId == info.SportId)
                .Select(x => (SkillLevel?)x.SkillLevel)
                .SingleOrDefaultAsync();

            var skill = preference ?? SkillLevel.Beginner;

            return skill >= info.MinSkill && skill <= info.MaxSkill;
        }

        private async Task<bool> HasOverlappingBookingAsync(
            Guid userId,
            JoinInfoRow info)
        {
            var bookings = await participantRepository
                .Query()
                .Where(x => x.UserId == userId
                    && x.MatchId != info.Id
                    && (x.Status == ParticipantStatus.Joined
                        || x.Status == ParticipantStatus.Waitlisted)
                    && !x.Match.IsDeleted
                    && x.Match.Status != MatchStatus.Cancelled
                    && x.Match.Event != null
                    && x.Match.Event.Venue.Date == info.Date)
                .Select(x => new BookingRow
                {
                    StartsAt = x.Match.Event!.Venue.StartsAt,
                    DurationMinutes = x.Match.DurationMinutes
                })
                .ToListAsync();

            var start = info.StartsAt.Hour * 60 + info.StartsAt.Minute;
            var end = start + info.DurationMinutes;

            return bookings.Any(booking =>
            {
                var otherStart = booking.StartsAt.Hour * 60 + booking.StartsAt.Minute;
                var otherEnd = otherStart + booking.DurationMinutes;

                return start < otherEnd && otherStart < end;
            });
        }
    }
}