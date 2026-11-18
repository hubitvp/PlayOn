using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Match;
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
        private const string ConflictMessage =
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
            JoinAsync(ParticipationRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<JoinMatchResponseModel>.Failure(errors);
            }

            var info = await GetInfoAsync(request.MatchId);

            if (info == null || info.IsDeleted || info.IsHidden)
            {
                return ServiceResponse<JoinMatchResponseModel>.NotFound();
            }

            if (!IsOpenForParticipation(info))
            {
                return ServiceResponse<JoinMatchResponseModel>
                    .Conflict("This match is cancelled, finished or already started.");
            }

            var alreadyIn = await participantRepository
                .Query()
                .AnyAsync(x => x.MatchId == info.Id
                    && x.UserId == request.UserId
                    && (x.Status == ParticipantStatus.Joined
                        || x.Status == ParticipantStatus.Waitlisted));

            if (alreadyIn)
            {
                return ServiceResponse<JoinMatchResponseModel>
                    .Conflict("You are already part of this match.");
            }

            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            var ruleError = await CheckJoinRulesAsync(request.UserId, info, nowUtc);

            if (ruleError != null)
            {
                return ServiceResponse<JoinMatchResponseModel>
                    .Failure(new[] { ruleError });
            }

            var joinedCount = await CountByStatusAsync(info.Id, ParticipantStatus.Joined);

            var status = joinedCount < info.MaxPlayers
                ? ParticipantStatus.Joined
                : ParticipantStatus.Waitlisted;

            var waitlistPosition = 0;

            if (status == ParticipantStatus.Waitlisted)
            {
                waitlistPosition = await CountByStatusAsync(info.Id, ParticipantStatus.Waitlisted) + 1;
            }

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == info.Id);

            await participantRepository.AddAsync(
                ParticipationDataBuilder.ParticipantToEntity(
                    info.Id,
                    request.UserId,
                    status,
                    nowUtc));

            var confirmed = false;

            if (status == ParticipantStatus.Joined
                && match.Status == MatchStatus.Open
                && joinedCount + 1 >= info.MinPlayers)
            {
                match.Status = MatchStatus.Confirmed;
                confirmed = true;

                var userIds = await participantRepository
                    .Query()
                    .Where(x => x.MatchId == info.Id && x.Status == ParticipantStatus.Joined)
                    .Select(x => x.UserId)
                    .ToListAsync();

                userIds.Add(request.UserId);

                //var notifications = userIds
                //    .Select(userId => NotificationDataBuilder.NotificationToEntity(
                //        userId,
                //        NotificationType.MatchConfirmed,
                //        $"\"{info.Title}\" is confirmed: enough players have joined.",
                //        info.Id,
                //        nowUtc))
                //    .ToList();

                IEnumerable<Notification> notifications = [];

                await notificationRepository.AddRangeAsync(notifications);
            }



            matchRepository.Update(match);

            try
            {
                await matchRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return ServiceResponse<JoinMatchResponseModel>.Conflict(ConflictMessage);
            }

            return ServiceResponse<JoinMatchResponseModel>
                .Success(ParticipationDataBuilder.JoinToResponseModel(status, waitlistPosition, confirmed));
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
                    .Conflict("This match is cancelled, finished or already started.");
            }

            var participant = await participantRepository
                .Query(asNoTracking: false)
                .FirstOrDefaultAsync(x => x.MatchId == info.Id
                    && x.UserId == request.UserId
                    && (x.Status == ParticipantStatus.Joined
                        || x.Status == ParticipantStatus.Waitlisted));

            if (participant == null)
            {
                return ServiceResponse<LeaveMatchResponseModel>
                    .Conflict("You are not part of this match.");
            }

            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
            var nowLocal = timeProvider.GetLocalNow().DateTime;

            var wasJoined = participant.Status == ParticipantStatus.Joined;
            var start = info.Date.ToDateTime(info.StartsAt);
            var isLate = wasJoined && start - nowLocal < TimeSpan.FromHours(LateCancelHours);

            participant.Status = ParticipantStatus.Cancelled;
            participant.StatusChangedOn = nowUtc;
            participant.IsLateCancel = isLate;

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == info.Id);

            var notifications = new List<Notification>();

            if (isLate)
            {
                //notifications.Add(NotificationDataBuilder.NotificationToEntity(
                //    request.UserId,
                //    NotificationType.StrikeAdded,
                //    $"You left \"{info.Title}\" less than {LateCancelHours} hours before it started. "
                //        + $"This counts as a strike for {StrikeWindowDays} days.",
                //    info.Id,
                //    nowUtc));
            }

            var promoted = false;

            if (wasJoined)
            {

                var joinedAfter = await CountByStatusAsync(info.Id, ParticipantStatus.Joined) - 1;

                if (joinedAfter < info.MaxPlayers)
                {
                    var waiting = await participantRepository
                        .Query(asNoTracking: false)
                        .Where(x => x.MatchId == info.Id && x.Status == ParticipantStatus.Waitlisted)
                        .OrderBy(x => x.JoinedOn)
                        .ThenBy(x => x.Id)
                        .ToListAsync();

                    foreach (var candidate in waiting)
                    {
                        if (await CheckJoinRulesAsync(candidate.UserId, info, nowUtc) != null)
                        {
                            continue;
                        }

                        candidate.Status = ParticipantStatus.Joined;
                        candidate.StatusChangedOn = nowUtc;

                        //notifications.Add(NotificationDataBuilder.NotificationToEntity(
                        //    candidate.UserId,
                        //    NotificationType.Promoted,
                        //    $"A spot opened up in \"{info.Title}\" and you are now playing.",
                        //    info.Id,
                        //    nowUtc));

                        promoted = true;
                        joinedAfter++;
                        break;
                    }
                }

                if (match.Status == MatchStatus.Confirmed && joinedAfter < info.MinPlayers)
                {
                    match.Status = MatchStatus.Open;
                }
            }

            matchRepository.Update(match);
            await notificationRepository.AddRangeAsync(notifications);

            try
            {
                await matchRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return ServiceResponse<LeaveMatchResponseModel>.Conflict(ConflictMessage);
            }

            return ServiceResponse<LeaveMatchResponseModel>
                .Success(ParticipationDataBuilder.LeaveToResponseModel(isLate, promoted));
        }


        public async Task<ServiceResponse<PageResult<MyMatchResponseModel>>>
            GetMineAsync(MyMatchesRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<PageResult<MyMatchResponseModel>>.Failure(errors);
            }

            var nowLocal = timeProvider.GetLocalNow().DateTime;
            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            var baseQuery = participantRepository
                .Query()
                .Where(x => x.UserId == request.UserId
                    && !x.Match.IsDeleted
                    && x.Match.Event != null);

            IQueryable<MatchParticipant> query;

            switch (request.Tab)
            {
                case MyMatchesTab.Upcoming:
                    query = baseQuery
                        .Where(x => x.Status == ParticipantStatus.Joined
                            && x.Match.Status != MatchStatus.Cancelled
                            && (x.Match.Event!.Venue.Date > today
                                || (x.Match.Event.Venue.Date == today
                                    && x.Match.Event.Venue.StartsAt >= nowTime)))
                        .OrderBy(x => x.Match.Event!.Venue.Date)
                        .ThenBy(x => x.Match.Event!.Venue.StartsAt)
                        .ThenBy(x => x.Id);
                    break;

                case MyMatchesTab.Waitlist:
                    query = baseQuery
                        .Where(x => x.Status == ParticipantStatus.Waitlisted
                            && x.Match.Status != MatchStatus.Cancelled
                            && (x.Match.Event!.Venue.Date > today
                                || (x.Match.Event.Venue.Date == today
                                    && x.Match.Event.Venue.StartsAt >= nowTime)))
                        .OrderBy(x => x.Match.Event!.Venue.Date)
                        .ThenBy(x => x.Match.Event!.Venue.StartsAt)
                        .ThenBy(x => x.Id);
                    break;

                case MyMatchesTab.Past:
                    query = baseQuery
                        .Where(x => x.Match.Status != MatchStatus.Cancelled
                            && (x.Status == ParticipantStatus.Attended
                                || x.Status == ParticipantStatus.NoShow
                                || (x.Status == ParticipantStatus.Joined
                                    && (x.Match.Event!.Venue.Date < today
                                        || (x.Match.Event.Venue.Date == today
                                            && x.Match.Event.Venue.StartsAt < nowTime)))))
                        .OrderByDescending(x => x.Match.Event!.Venue.Date)
                        .ThenByDescending(x => x.Match.Event!.Venue.StartsAt)
                        .ThenByDescending(x => x.Id);
                    break;

                default:
                    query = baseQuery
                        .Where(x => x.Status == ParticipantStatus.Cancelled
                            || x.Match.Status == MatchStatus.Cancelled)
                        .OrderByDescending(x => x.Match.Event!.Venue.Date)
                        .ThenByDescending(x => x.Match.Event!.Venue.StartsAt)
                        .ThenByDescending(x => x.Id);
                    break;
            }

            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(request.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(request.Page, 1, totalPages);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ParticipationDataBuilder.MyMatchProjection())
                .ToListAsync();

            var page = new PageResult<MyMatchResponseModel>(
                items,
                pageNumber,
                pageSize,
                totalCount);

            return ServiceResponse<PageResult<MyMatchResponseModel>>.Success(page);
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
            return (info.Status == MatchStatus.Open || info.Status == MatchStatus.Confirmed)
                && info.Date.ToDateTime(info.StartsAt) > timeProvider.GetLocalNow().DateTime;
        }

        private async Task<int> CountByStatusAsync(Guid matchId, ParticipantStatus status)
        {
            return await participantRepository
                .Query()
                .CountAsync(x => x.MatchId == matchId && x.Status == status);
        }


        private async Task<ValidationResult?>
            CheckJoinRulesAsync(
                Guid userId,
                JoinInfoRow info,
                DateTime nowUtc)
        {
            var strikeSince = nowUtc.AddDays(-StrikeWindowDays);

            var strikes = await participantRepository
                .Query()
                .Where(x => x.UserId == userId
                    && (x.IsLateCancel || x.Status == ParticipantStatus.NoShow)
                    && x.StatusChangedOn >= strikeSince)
                .Select(x => x.StatusChangedOn)
                .ToListAsync();

            if (strikes.Count >= StrikesToBlock
                && nowUtc < strikes.Max().AddDays(BlockDays))
            {
                return new ValidationResult(
                    "You have too many late cancellations or no-shows, so you cannot join matches for now.");
            }

            var preference = await preferenceRepository
                .Query()
                .Where(x => x.UserId == userId && x.SportId == info.SportId)
                .Select(x => (SkillLevel?)x.SkillLevel)
                .SingleOrDefaultAsync();

            var skill = preference ?? SkillLevel.Beginner;

            if (skill < info.MinSkill || skill > info.MaxSkill)
            {
                return new ValidationResult(
                    "Your skill level for this sport is outside the range of this match. You can change it in your profile.");
            }

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

            foreach (var booking in bookings)
            {
                var otherStart = booking.StartsAt.Hour * 60 + booking.StartsAt.Minute;
                var otherEnd = otherStart + booking.DurationMinutes;

                if (start < otherEnd && otherStart < end)
                {
                    return new ValidationResult(
                        "You are already in another match at the same time.");
                }
            }

            return null;
        }
    }
}
