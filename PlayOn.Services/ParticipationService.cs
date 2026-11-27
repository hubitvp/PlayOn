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
    /// <summary>
    /// Handles joining and leaving matches, waitlist promotion and the "my matches" listing,
    /// enforcing participation rules such as strikes, skill range and overlapping bookings.
    /// </summary>
    public class ParticipationService : IParticipationService
    {
        /// <summary>
        /// The maximum number of retries after a concurrency or transient transaction conflict.
        /// </summary>
        private const int MaxConcurrencyRetries = 5;

        /// <summary>
        /// Error message returned when the match is cancelled, finished or already started.
        /// </summary>
        private const string MatchClosedMessage =
            "This match is cancelled, finished or already started.";

        /// <summary>
        /// Error message returned when retries are exhausted because the match keeps changing.
        /// </summary>
        private const string ConcurrencyMessage =
            "The match changed while you were doing this. Please try again.";

        /// <summary>
        /// Error message returned when the user already has an active participation in the match.
        /// </summary>
        private const string DuplicateJoinMessage =
            "You are already part of this match.";

        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<SportPreference> preferenceRepository;
        private readonly IRepository<Notification> notificationRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipationService"/> class.
        /// </summary>
        /// <param name="matchRepository">Repository for matches.</param>
        /// <param name="participantRepository">Repository for match participants.</param>
        /// <param name="preferenceRepository">Repository for sport skill preferences.</param>
        /// <param name="notificationRepository">Repository for notifications.</param>
        /// <param name="timeProvider">Provider of the current local time.</param>
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

        /// <summary>
        /// Joins the user to a match inside a serializable transaction, retrying on concurrency conflicts.
        /// The user is added as joined while spots remain, otherwise waitlisted; the match becomes confirmed (and participants are notified) once the minimum player count is reached.
        /// Returns not found for missing or deleted matches and a conflict if the match is closed, the user is already part of it, or retries are exhausted; join-rule violations (strike block, skill range, overlapping booking) are returned as validation failures.
        /// </summary>
        /// <param name="request">The participation request containing the match and user identifiers.</param>
        /// <returns>A service response with the resulting participant status, waitlist position and whether the match was confirmed.</returns>
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

        /// <summary>
        /// Removes the user from a match inside a serializable transaction, retrying on concurrency conflicts.
        /// Leaving a joined spot less than the late-cancel threshold before the start counts as a late cancel (a strike) and notifies the user. A freed spot promotes the first eligible waitlisted player, and the match status is moved between open and confirmed according to the minimum player count.
        /// Returns not found for missing or deleted matches and a conflict if the user is the organiser, the match is closed, or the user is not part of it.
        /// </summary>
        /// <param name="request">The participation request containing the match and user identifiers.</param>
        /// <returns>A service response indicating whether the leave was a late cancel and whether another player was promoted.</returns>
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

        /// <summary>
        /// Returns one page of the user's matches for the selected tab (upcoming, waitlist, past or cancelled).
        /// The page size is clamped to 1-50 and the page number to the valid range.
        /// </summary>
        /// <param name="requestData">The request containing the user and the selected tab.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A service response with the page of matches, or validation errors if the request is invalid.</returns>
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

        /// <summary>
        /// Builds the base query of the user's participations in non-deleted matches that have an event, filtered for the requested tab.
        /// </summary>
        /// <param name="requestData">The request containing the user and the selected tab.</param>
        /// <returns>A query of participations matching the tab.</returns>
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

        /// <summary>
        /// Filters participations with the given status in non-cancelled matches that start now or later.
        /// </summary>
        /// <param name="query">The base participation query.</param>
        /// <param name="status">The participant status to match.</param>
        /// <param name="today">The current local date.</param>
        /// <param name="nowTime">The current local time.</param>
        /// <returns>The filtered query.</returns>
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

        /// <summary>
        /// Filters participations in non-cancelled matches that were attended, were no-shows, or were joined in matches that have already started.
        /// </summary>
        /// <param name="query">The base participation query.</param>
        /// <param name="today">The current local date.</param>
        /// <param name="nowTime">The current local time.</param>
        /// <returns>The filtered query.</returns>
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

        /// <summary>
        /// Filters participations that were cancelled by the user or belong to cancelled matches.
        /// </summary>
        /// <param name="query">The base participation query.</param>
        /// <returns>The filtered query.</returns>
        private IQueryable<MatchParticipant> ApplyCancelledTab(
            IQueryable<MatchParticipant> query)
        {
            return query
                .Where(x => x.Status == ParticipantStatus.Cancelled
                    || x.Match.Status == MatchStatus.Cancelled);
        }

        /// <summary>
        /// Counts, orders and pages the participations and projects them to response models.
        /// Upcoming and waitlist tabs are ordered by start ascending; other tabs by start descending.
        /// </summary>
        /// <param name="query">The filtered participation query.</param>
        /// <param name="tab">The selected tab, which determines the ordering.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>The page of matches with paging information.</returns>
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

        /// <summary>
        /// Marks the match as updated so that its concurrency token is checked when changes are saved.
        /// </summary>
        /// <param name="match">The match entity to mark.</param>
        private void MarkMatchForConcurrencyCheck(Match match)
        {
            matchRepository.Update(match);
        }

        /// <summary>
        /// Waits for a short randomized delay that grows with the attempt number before a retry.
        /// </summary>
        /// <param name="attempt">The zero-based retry attempt number.</param>
        private static async Task DelayRetryAsync(int attempt)
        {
            var jitterMs = Random.Shared.Next(10, 35) * (attempt + 1);
            await Task.Delay(jitterMs);
        }

        /// <summary>
        /// Determines whether the exception or any inner exception is a SQL Server exception with one of the given error numbers.
        /// </summary>
        /// <param name="exception">The exception to inspect.</param>
        /// <param name="errorNumbers">The SQL error numbers to look for.</param>
        /// <returns><c>true</c> if a matching SQL error is found; otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the exception was caused by a unique constraint or duplicate key violation.
        /// </summary>
        /// <param name="exception">The database update exception.</param>
        /// <returns><c>true</c> for a unique constraint violation; otherwise <c>false</c>.</returns>
        private static bool IsUniqueConstraintViolation(DbUpdateException exception)
        {
            return HasSqlErrorNumber(exception, 2601, 2627);
        }

        /// <summary>
        /// Determines whether the exception was caused by a deadlock or lock timeout that may succeed on retry.
        /// </summary>
        /// <param name="exception">The exception to inspect.</param>
        /// <returns><c>true</c> for a transient transaction conflict; otherwise <c>false</c>.</returns>
        private static bool IsTransientTransactionConflict(Exception exception)
        {
            return HasSqlErrorNumber(exception, 1205, 1222);
        }

        /// <summary>
        /// Loads the join-related information of a match that has an event.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The match information, or <c>null</c> if the match or its event does not exist.</returns>
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

        /// <summary>
        /// Determines whether the match is open or confirmed and has not started yet.
        /// </summary>
        /// <param name="info">The match information.</param>
        /// <param name="nowLocal">The current local time.</param>
        /// <returns><c>true</c> if users can still join or leave; otherwise <c>false</c>.</returns>
        private static bool IsOpenForParticipation(JoinInfoRow info, DateTime nowLocal)
        {
            return (info.Status == MatchStatus.Open
                    || info.Status == MatchStatus.Confirmed)
                && info.Date.ToDateTime(info.StartsAt) > nowLocal;
        }

        /// <summary>
        /// Finds the user's joined or waitlisted participation in a match.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="asNoTracking">Whether the entity should be read without change tracking.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The active participation, or <c>null</c> if there is none.</returns>
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

        /// <summary>
        /// Counts the participants of a match that have the given status.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="status">The participant status to count.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The number of matching participants.</returns>
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

        /// <summary>
        /// Adds a match-confirmed notification for every joined participant and the user who just joined.
        /// </summary>
        /// <param name="info">The match information.</param>
        /// <param name="joiningUserId">The identifier of the user who is joining.</param>
        /// <param name="nowLocal">The current local time.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
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

        /// <summary>
        /// Promotes the earliest waitlisted player who passes the join rules to joined status.
        /// </summary>
        /// <param name="info">The match information.</param>
        /// <param name="nowLocal">The current local time.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A promotion notification for the promoted player, or <c>null</c> if nobody could be promoted.</returns>
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

        /// <summary>
        /// Checks the rules a user must satisfy to join a match: not blocked by strikes, skill level within the match range, and no overlapping booking.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="info">The match information.</param>
        /// <param name="nowLocal">The current local time.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A validation error for the first violated rule, or <c>null</c> if the user may join.</returns>
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

        /// <summary>
        /// Determines whether the user has reached the strike limit within the strike window and is still inside the block period.
        /// Strikes are late cancellations and no-shows.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="nowLocal">The current local time.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns><c>true</c> if the user is currently blocked from joining; otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the user's skill level for the match sport is within the match skill range.
        /// A user without a preference for the sport is treated as a beginner.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="info">The match information.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns><c>true</c> if the skill level is in range; otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the user has another joined or waitlisted, non-cancelled match on the same date whose time span overlaps this match.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="info">The match information.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns><c>true</c> if an overlapping booking exists; otherwise <c>false</c>.</returns>
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