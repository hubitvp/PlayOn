using Microsoft.Data.SqlClient;
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
using System.Data;
using static PlayOn.Common.ApplicationConstants.ParticipationRules;

namespace PlayOn.Services
{
    public class ParticipationService : IParticipationService
    {
        private const int MaxConcurrencyRetries = 5;

        private const string MatchClosedMessage =
            "This match is cancelled, finished or already started.";

        private const string ConcurrencyMessage =
            "The match changed while you were doing this. Please try again.";

        private const string DuplicateJoinMessage =
            "You are already part of this match.";

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

            for (var attempt = 0; attempt <= MaxConcurrencyRetries; attempt++)
            {
                try
                {
                    return await matchRepository.ExecuteInTransactionAsync(
                        IsolationLevel.Serializable,
                        async cancellationToken =>
                        {
                            var info = await GetInfoAsync(request.MatchId, cancellationToken);
                            if (info == null || info.IsDeleted)
                            {
                                return ServiceResponse<JoinMatchResponseModel>.NotFound();
                            }

                            var nowLocal = timeProvider.GetLocalNow().LocalDateTime;

                            if (!IsOpenForParticipation(info, nowLocal))
                            {
                                return ServiceResponse<JoinMatchResponseModel>
                                    .Conflict(MatchClosedMessage);
                            }

                            var participant = await participantRepository
                                .Query(asNoTracking: false)
                                .SingleOrDefaultAsync(
                                    x => x.MatchId == info.Id && x.UserId == request.UserId,
                                    cancellationToken);

                            if (participant != null
                                && participant.Status != ParticipantStatus.Cancelled)
                            {
                                var message = participant.Status == ParticipantStatus.Joined
                                    || participant.Status == ParticipantStatus.Waitlisted
                                        ? DuplicateJoinMessage
                                        : "You cannot join this match again.";

                                return ServiceResponse<JoinMatchResponseModel>
                                    .Conflict(message);
                            }

                            var ruleError = await CheckJoinRulesAsync(
                                request.UserId,
                                info,
                                nowLocal,
                                cancellationToken);

                            if (ruleError != null)
                            {
                                return ServiceResponse<JoinMatchResponseModel>
                                    .Failure(new[] { ruleError });
                            }

                            var joinedCount = await CountByStatusAsync(
                                info.Id,
                                ParticipantStatus.Joined,
                                cancellationToken);

                            var status = joinedCount < info.MaxPlayers
                                ? ParticipantStatus.Joined
                                : ParticipantStatus.Waitlisted;

                            var waitlistPosition = status == ParticipantStatus.Waitlisted
                                ? await CountByStatusAsync(
                                    info.Id,
                                    ParticipantStatus.Waitlisted,
                                    cancellationToken) + 1
                                : 0;

                            var match = await matchRepository
                                .Query(asNoTracking: false)
                                .SingleAsync(x => x.Id == info.Id, cancellationToken);

                            MarkMatchForConcurrencyCheck(match);

                            if (participant == null)
                            {
                                await participantRepository.AddAsync(
                                    ParticipationDataBuilder.ParticipantToEntity(
                                        info.Id,
                                        request.UserId,
                                        status,
                                        nowLocal),
                                    cancellationToken);
                            }
                            else
                            {
                                participant.Status = status;
                                participant.JoinedOn = nowLocal;
                                participant.StatusChangedOn = nowLocal;
                                participant.IsLateCancel = false;
                            }

                            var newJoinedCount = status == ParticipantStatus.Joined
                                ? joinedCount + 1
                                : joinedCount;

                            var isConfirmed = status == ParticipantStatus.Joined
                                && match.Status == MatchStatus.Open
                                && newJoinedCount >= info.MinPlayers;

                            if (isConfirmed)
                            {
                                match.Status = MatchStatus.Confirmed;

                                await AddMatchConfirmedNotificationsAsync(
                                    info,
                                    request.UserId,
                                    nowLocal,
                                    cancellationToken);
                            }

                            await matchRepository.SaveChangesAsync(cancellationToken);

                            var responseData = ParticipationDataBuilder.JoinToResponseModel(
                                status,
                                waitlistPosition,
                                isConfirmed);

                            return ServiceResponse<JoinMatchResponseModel>
                                .Success(responseData);
                        });
                }
                catch (DbUpdateConcurrencyException) when (attempt < MaxConcurrencyRetries)
                {
                    matchRepository.ClearChangeTracker();
                    await DelayRetryAsync(attempt);
                }
                catch (DbUpdateConcurrencyException)
                {
                    matchRepository.ClearChangeTracker();
                    return ServiceResponse<JoinMatchResponseModel>
                        .Conflict(ConcurrencyMessage);
                }
                catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
                {
                    matchRepository.ClearChangeTracker();
                    return ServiceResponse<JoinMatchResponseModel>
                        .Conflict(DuplicateJoinMessage);
                }
                catch (DbUpdateException ex) when (IsTransientTransactionConflict(ex) && attempt < MaxConcurrencyRetries)
                {
                    matchRepository.ClearChangeTracker();
                    await DelayRetryAsync(attempt);
                }
                catch (DbUpdateException)
                {
                    matchRepository.ClearChangeTracker();
                    throw;
                }
            }

            return ServiceResponse<JoinMatchResponseModel>
                .Conflict(ConcurrencyMessage);
        }

        public async Task<ServiceResponse<LeaveMatchResponseModel>>
            LeaveAsync(ParticipationRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<LeaveMatchResponseModel>.Failure(errors);
            }

            for (var attempt = 0; attempt <= MaxConcurrencyRetries; attempt++)
            {
                try
                {
                    return await matchRepository.ExecuteInTransactionAsync(
                        IsolationLevel.Serializable,
                        async ct =>
                        {
                            var info = await GetInfoAsync(request.MatchId, ct);
                            if (info == null || info.IsDeleted)
                            {
                                return ServiceResponse<LeaveMatchResponseModel>.NotFound();
                            }

                            if (info.OrganiserId == request.UserId)
                            {
                                return ServiceResponse<LeaveMatchResponseModel>
                                    .Conflict("The organiser cannot leave the match. Cancel it instead.");
                            }

                            var nowLocal = timeProvider.GetLocalNow().LocalDateTime;

                            if (!IsOpenForParticipation(info, nowLocal))
                            {
                                return ServiceResponse<LeaveMatchResponseModel>
                                    .Conflict(MatchClosedMessage);
                            }

                            var participant = await FindActiveParticipantAsync(
                                info.Id,
                                request.UserId,
                                asNoTracking: false,
                                ct);

                            if (participant == null)
                            {
                                return ServiceResponse<LeaveMatchResponseModel>
                                    .Conflict("You are not part of this match.");
                            }

                            var wasJoined = participant.Status == ParticipantStatus.Joined;

                            var isLateCancel = wasJoined
                                && info.Date.ToDateTime(info.StartsAt) - nowLocal
                                    < TimeSpan.FromHours(LateCancelHours);

                            participant.Status = ParticipantStatus.Cancelled;
                            participant.StatusChangedOn = nowLocal;
                            participant.IsLateCancel = isLateCancel;

                            var match = await matchRepository
                                .Query(asNoTracking: false)
                                .SingleAsync(x => x.Id == info.Id, ct);

                            MarkMatchForConcurrencyCheck(match);

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
                                var joinedAfterLeave = Math.Max(
                                    0,
                                    await CountByStatusAsync(info.Id, ParticipantStatus.Joined, ct) - 1);

                                if (joinedAfterLeave < info.MaxPlayers)
                                {
                                    var promotion = await PromoteFromWaitlistAsync(info, nowLocal, ct);

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
                                else if (match.Status == MatchStatus.Open
                                    && joinedAfterLeave >= info.MinPlayers)
                                {
                                    match.Status = MatchStatus.Confirmed;
                                }
                            }

                            if (notifications.Count > 0)
                            {
                                await notificationRepository.AddRangeAsync(notifications, ct);
                            }

                            await matchRepository.SaveChangesAsync(ct);

                            var responseData = ParticipationDataBuilder
                                .LeaveToResponseModel(isLateCancel, isPromoted);

                            return ServiceResponse<LeaveMatchResponseModel>.Success(responseData);
                        });
                }
                catch (DbUpdateConcurrencyException) when (attempt < MaxConcurrencyRetries)
                {
                    matchRepository.ClearChangeTracker();
                    await DelayRetryAsync(attempt);
                }
                catch (DbUpdateConcurrencyException)
                {
                    matchRepository.ClearChangeTracker();
                    return ServiceResponse<LeaveMatchResponseModel>
                        .Conflict(ConcurrencyMessage);
                }
                catch (DbUpdateException ex) when (IsTransientTransactionConflict(ex) && attempt < MaxConcurrencyRetries)
                {
                    matchRepository.ClearChangeTracker();
                    await DelayRetryAsync(attempt);
                }
                catch (DbUpdateException)
                {
                    matchRepository.ClearChangeTracker();
                    throw;
                }
            }

            return ServiceResponse<LeaveMatchResponseModel>
                .Conflict(ConcurrencyMessage);
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

            var page = await GetPageAsync(
                participationQuery,
                requestData.Tab,
                pageRequestData);

            return ServiceResponse<PageResult<MyMatchResponseModel>>.Success(page);
        }

        private IQueryable<MatchParticipant> BuildMyMatchesQuery(
            MyMatchesRequestData requestData)
        {
            var nowLocal = timeProvider.GetLocalNow().LocalDateTime;
            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            var participationQuery = participantRepository
                .Query()
                .Where(x => x.UserId == requestData.UserId
                    && !x.Match.IsDeleted
                    && x.Match.Event != null);

            var badrgf = participationQuery.ToList();

            return requestData.Tab switch
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

        private IQueryable<MatchParticipant> ApplyUpcomingTab(
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
                            && x.Match.Event.Venue.StartsAt >= nowTime)));
        }

        private IQueryable<MatchParticipant> ApplyPastTab(
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
                                    && x.Match.Event.Venue.StartsAt < nowTime)))));
        }

        private IQueryable<MatchParticipant> ApplyCancelledTab(
            IQueryable<MatchParticipant> query)
        {
            return query
                .Where(x => x.Status == ParticipantStatus.Cancelled
                    || x.Match.Status == MatchStatus.Cancelled);
        }

        private async Task<PageResult<MyMatchResponseModel>>
            GetPageAsync(
                IQueryable<MatchParticipant> query,
                MyMatchesTab tab,
                PageRequestData pageRequestData)
        {
            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(pageRequestData.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(pageRequestData.Page, 1, totalPages);

            var orderedQuery = tab is MyMatchesTab.Upcoming or MyMatchesTab.Waitlist
                ? query
                    .OrderBy(x => x.Match.Event!.Venue.Date)
                    .ThenBy(x => x.Match.Event!.Venue.StartsAt)
                : query
                    .OrderByDescending(x => x.Match.Event!.Venue.Date)
                    .ThenByDescending(x => x.Match.Event!.Venue.StartsAt);

            var items = await orderedQuery
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

        private void MarkMatchForConcurrencyCheck(Match match)
        {
            matchRepository.Update(match);
        }

        private static async Task DelayRetryAsync(int attempt)
        {
            var jitterMs = Random.Shared.Next(10, 35) * (attempt + 1);
            await Task.Delay(jitterMs);
        }

        private static bool HasSqlErrorNumber(Exception exception, params int[] errorNumbers)
        {
            Exception? current = exception;
            while (current != null)
            {
                if (current is SqlException sqlException
                    && sqlException.Errors.Cast<SqlError>().Any(e => errorNumbers.Contains(e.Number)))
                {
                    return true;
                }

                current = current.InnerException;
            }

            return false;
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException exception)
        {
            return HasSqlErrorNumber(exception, 2601, 2627);
        }

        private static bool IsTransientTransactionConflict(Exception exception)
        {
            return HasSqlErrorNumber(exception, 1205, 1222);
        }

        private async Task<JoinInfoRow?> GetInfoAsync(
            Guid matchId,
            CancellationToken cancellationToken = default)
        {
            return await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null)
                .Select(ParticipationDataBuilder.JoinInfoProjection())
                .SingleOrDefaultAsync(cancellationToken);
        }

        private static bool IsOpenForParticipation(JoinInfoRow info, DateTime nowLocal)
        {
            return (info.Status == MatchStatus.Open
                    || info.Status == MatchStatus.Confirmed)
                && info.Date.ToDateTime(info.StartsAt) > nowLocal;
        }

        private async Task<MatchParticipant?> FindActiveParticipantAsync(
            Guid matchId,
            Guid userId,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            return await participantRepository
                .Query(asNoTracking)
                .SingleOrDefaultAsync(x => x.MatchId == matchId
                    && x.UserId == userId
                    && (x.Status == ParticipantStatus.Joined
                        || x.Status == ParticipantStatus.Waitlisted),
                    cancellationToken);
        }

        private async Task<int> CountByStatusAsync(
            Guid matchId,
            ParticipantStatus status,
            CancellationToken cancellationToken = default)
        {
            return await participantRepository
                .Query()
                .CountAsync(
                    x => x.MatchId == matchId && x.Status == status,
                    cancellationToken);
        }

        private async Task AddMatchConfirmedNotificationsAsync(
            JoinInfoRow info,
            Guid joiningUserId,
            DateTime nowLocal,
            CancellationToken cancellationToken = default)
        {
            var userIds = await participantRepository
                .Query()
                .Where(x => x.MatchId == info.Id
                    && x.Status == ParticipantStatus.Joined)
                .Select(x => x.UserId)
                .ToListAsync(cancellationToken);

            userIds.Add(joiningUserId);

            var notifications = userIds
                .Distinct()
                .Select(userId => NotificationDataBuilder.NotificationToEntity(
                    userId,
                    NotificationType.MatchConfirmed,
                    $"\"{info.Title}\" is confirmed: enough players have joined.",
                    info.Id,
                    nowLocal))
                .ToList();

            await notificationRepository.AddRangeAsync(notifications, cancellationToken);
        }

        private async Task<Notification?> PromoteFromWaitlistAsync(
            JoinInfoRow info,
            DateTime nowLocal,
            CancellationToken cancellationToken = default)
        {
            var waitingPlayers = await participantRepository
                .Query(asNoTracking: false)
                .Where(x => x.MatchId == info.Id
                    && x.Status == ParticipantStatus.Waitlisted)
                .OrderBy(x => x.JoinedOn)
                .ThenBy(x => x.MatchId)
                .ThenBy(x => x.UserId)
                .ToListAsync(cancellationToken);

            foreach (var candidate in waitingPlayers)
            {
                if (await CheckJoinRulesAsync(candidate.UserId, info, nowLocal, cancellationToken) != null)
                {
                    continue;
                }

                candidate.Status = ParticipantStatus.Joined;
                candidate.StatusChangedOn = nowLocal;

                return NotificationDataBuilder.NotificationToEntity(
                    candidate.UserId,
                    NotificationType.Promoted,
                    $"A spot opened up in \"{info.Title}\" and you are now playing.",
                    info.Id,
                    nowLocal);
            }

            return null;
        }

        private async Task<ValidationResult?> CheckJoinRulesAsync(
            Guid userId,
            JoinInfoRow info,
            DateTime nowLocal,
            CancellationToken cancellationToken = default)
        {
            if (await IsBlockedByStrikesAsync(userId, nowLocal, cancellationToken))
            {
                return new ValidationResult(
                    "You have too many late cancellations or no-shows, so you cannot join matches for now.");
            }

            if (!await IsSkillInRangeAsync(userId, info, cancellationToken))
            {
                return new ValidationResult(
                    "Your skill level for this sport is outside the range of this match. You can change it in your profile.");
            }

            if (await HasOverlappingBookingAsync(userId, info, cancellationToken))
            {
                return new ValidationResult(
                    "You are already in another match at the same time.");
            }

            return null;
        }

        private async Task<bool> IsBlockedByStrikesAsync(
            Guid userId,
            DateTime nowLocal,
            CancellationToken cancellationToken = default)
        {
            var strikeSince = nowLocal.AddDays(-StrikeWindowDays);

            var strikeDates = await participantRepository
                .Query()
                .Where(x => x.UserId == userId
                    && (x.IsLateCancel || x.Status == ParticipantStatus.NoShow)
                    && x.StatusChangedOn >= strikeSince)
                .Select(x => x.StatusChangedOn)
                .ToListAsync(cancellationToken);

            return strikeDates.Count >= StrikesToBlock
                && nowLocal < strikeDates.Max().AddDays(BlockDays);
        }

        private async Task<bool> IsSkillInRangeAsync(
            Guid userId,
            JoinInfoRow info,
            CancellationToken cancellationToken = default)
        {
            var preference = await preferenceRepository
                .Query()
                .Where(x => x.UserId == userId && x.SportId == info.SportId)
                .Select(x => (SkillLevel?)x.SkillLevel)
                .SingleOrDefaultAsync(cancellationToken);

            var skill = preference ?? SkillLevel.Beginner;

            return skill >= info.MinSkill && skill <= info.MaxSkill;
        }

        private async Task<bool> HasOverlappingBookingAsync(
            Guid userId,
            JoinInfoRow info,
            CancellationToken cancellationToken = default)
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
                .ToListAsync(cancellationToken);

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