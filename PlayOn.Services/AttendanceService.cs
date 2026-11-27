using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Attendance;
using PlayOn.Services.Data.Attendance.RequestData;
using PlayOn.Services.Data.Attendance.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Records attendance for finished matches, marking players as attended or no-show and updating their reliability statistics.
    /// </summary>
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Notification> notificationRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttendanceService"/> class.
        /// </summary>
        /// <param name="matchRepository">Repository for matches.</param>
        /// <param name="participantRepository">Repository for match participants.</param>
        /// <param name="userRepository">Repository for users.</param>
        /// <param name="notificationRepository">Repository for notifications.</param>
        /// <param name="timeProvider">Provider of the current time.</param>
        public AttendanceService(
            IRepository<Match> matchRepository,
            IRepository<MatchParticipant> participantRepository,
            IRepository<ApplicationUser> userRepository,
            IRepository<Notification> notificationRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.participantRepository = participantRepository;
            this.userRepository = userRepository;
            this.notificationRepository = notificationRepository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Gets the attendance sheet for a match: its details and the players currently joined, in join order.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="actorId">The identifier of the user making the request.</param>
        /// <param name="actorIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the attendance sheet; not-found for an empty, unknown or deleted match, forbidden if the actor is neither organiser nor admin, or conflict if the match is cancelled, already completed or not yet ended.</returns>
        public async Task<ServiceResponse<AttendanceResponseModel>>
            GetAsync(
                Guid matchId,
                Guid actorId,
                bool actorIsAdmin)
        {
            if (matchId == Guid.Empty)
            {
                return ServiceResponse<AttendanceResponseModel>.NotFound();
            }

            var info = await GetInfoAsync(matchId);

            var failure = CheckAccess<AttendanceResponseModel>(info, actorId, actorIsAdmin);
            if (failure != null)
            {
                return failure;
            }

            var players = await participantRepository
                .Query()
                .Where(x => x.MatchId == matchId && x.Status == ParticipantStatus.Joined)
                .OrderBy(x => x.JoinedOn)
                .ThenBy(x => x.MatchId)
                .ThenBy(x => x.UserId)
                .Select(AttendanceDataBuilder.AttendancePlayerProjection())
                .ToListAsync();

            return ServiceResponse<AttendanceResponseModel>
                .Success(AttendanceDataBuilder.AttendanceToResponseModel(info!, players));
        }

        /// <summary>
        /// Records attendance for all joined players of a match. Every joined player must have an entry; players are marked attended or no-show, no-shows are notified, the match is completed and player statistics are updated.
        /// </summary>
        /// <param name="request">The attendance submission data with one entry per player.</param>
        /// <returns>A response with the match identifier on success; a validation failure (including missing entries), not-found, forbidden or conflict result otherwise, with conflict also returned on a concurrent modification.</returns>
        public async Task<ServiceResponse<Guid>>
            SubmitAsync(AttendanceRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request);
            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            if (request.MatchId == Guid.Empty)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var info = await GetInfoAsync(request.MatchId);

            var failure = CheckAccess<Guid>(info, request.ActorId, request.ActorIsAdmin);
            if (failure != null)
            {
                return failure;
            }

            var participants = await participantRepository
                .Query(asNoTracking: false)
                .Where(x => x.MatchId == request.MatchId && x.Status == ParticipantStatus.Joined)
                .ToListAsync();

            var requestEntries = request.Entries;

            var entries = requestEntries
                .GroupBy(x => x.UserId)
                .ToDictionary(x => x.Key, x => x.Last().Attended);

            if (participants.Any(x => !entries.ContainsKey(x.UserId)))
            {
                return ServiceResponse<Guid>
                    .Failure("Mark attendance for every player.", "Players");
            }

            var localDateTime = timeProvider.GetLocalNow().LocalDateTime;

            var notifications = ApplyAttendance(
                participants,
                entries,
                info!,
                localDateTime);

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == request.MatchId);

            if (match == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            match.Status = MatchStatus.Completed;

            await UpdateUserStatsAsync(request.MatchId, participants);

            await notificationRepository.AddRangeAsync(notifications);

            if (!await TrySaveChangesAsync())
            {
                return ServiceResponse<Guid>.Conflict(
                    "The match changed while you were saving. Please open the page again.");
            }

            return ServiceResponse<Guid>.Success(request.MatchId);
        }

        /// <summary>
        /// Sets each participant's status to attended or no-show and builds a strike notification for every no-show.
        /// </summary>
        /// <param name="participants">The joined participants to update.</param>
        /// <param name="entries">The attended flag for each user, keyed by user identifier.</param>
        /// <param name="info">The match information used in notification text.</param>
        /// <param name="nowUtc">The timestamp to record as the status change time.</param>
        /// <returns>The notifications to be saved for participants marked as no-show.</returns>
        private static List<Notification> ApplyAttendance(
            IEnumerable<MatchParticipant> participants,
            IReadOnlyDictionary<Guid, bool> entries,
            AttendanceInfoRow info,
            DateTime nowUtc)
        {
            var notifications = new List<Notification>();

            foreach (var participant in participants)
            {
                var attended = entries[participant.UserId];

                participant.Status = attended
                    ? ParticipantStatus.Attended
                    : ParticipantStatus.NoShow;
                participant.StatusChangedOn = nowUtc;

                if (!attended)
                {
                    notifications.Add(NotificationDataBuilder.NotificationToEntity(
                        participant.UserId,
                        NotificationType.StrikeAdded,
                        $"You were marked as a no-show for \"{info.Title}\". This counts as a strike.",
                        info.Id,
                        nowUtc));
                }
            }

            return notifications;
        }

        /// <summary>
        /// Saves pending changes, treating a concurrency conflict as a normal outcome.
        /// </summary>
        /// <returns>True if the changes were saved; false if a concurrency conflict occurred.</returns>
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

        /// <summary>
        /// Recalculates matches played and reliability score for each participant from their past records plus the current match.
        /// </summary>
        /// <param name="matchId">The identifier of the current match, excluded from the stored history.</param>
        /// <param name="participants">The participants whose new statuses have been applied.</param>
        /// <returns>A task that completes when the user entities have been updated.</returns>
        private async Task UpdateUserStatsAsync(
            Guid matchId,
            IReadOnlyCollection<MatchParticipant> participants)
        {
            var userIds = participants
                .Select(x => x.UserId)
                .ToList();

            var history = await participantRepository
                .Query()
                .Where(x => userIds.Contains(x.UserId) && x.MatchId != matchId)
                .GroupBy(x => x.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    Attended = g.Count(x => x.Status == ParticipantStatus.Attended),
                    NoShow = g.Count(x => x.Status == ParticipantStatus.NoShow),
                    LateCancel = g.Count(x => x.Status == ParticipantStatus.Cancelled && x.IsLateCancel)
                })
                .ToListAsync();

            var users = await userRepository
                .Query(asNoTracking: false)
                .Where(x => userIds.Contains(x.Id))
                .ToListAsync();

            foreach (var user in users)
            {
                var past = history.SingleOrDefault(x => x.UserId == user.Id);
                var current = participants.First(x => x.UserId == user.Id);

                var attended = (past?.Attended ?? 0)
                    + (current.Status == ParticipantStatus.Attended ? 1 : 0);
                var noShow = (past?.NoShow ?? 0)
                    + (current.Status == ParticipantStatus.NoShow ? 1 : 0);
                var lateCancel = past?.LateCancel ?? 0;

                user.MatchesPlayed = attended;
                user.ReliabilityScore = AttendanceDataBuilder
                    .ReliabilityScore(attended, noShow, lateCancel);
            }
        }

        /// <summary>
        /// Loads the attendance information of a match that has an event.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <returns>The match information row, or null if no such match with an event exists.</returns>
        private async Task<AttendanceInfoRow?> GetInfoAsync(Guid matchId)
        {
            return await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null)
                .Select(AttendanceDataBuilder.AttendanceInfoProjection())
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Checks whether the actor may record or view attendance for the match and whether the match is in a valid state and has ended.
        /// </summary>
        /// <typeparam name="T">The payload type of the response to return.</typeparam>
        /// <param name="info">The match information, or null if not found.</param>
        /// <param name="actorId">The identifier of the acting user.</param>
        /// <param name="actorIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A failure response (not-found, forbidden or conflict) if access is not allowed; otherwise null.</returns>
        private ServiceResponse<T>? CheckAccess<T>(
            AttendanceInfoRow? info,
            Guid actorId,
            bool actorIsAdmin)
        {
            if (info == null || info.IsDeleted)
            {
                return ServiceResponse<T>.NotFound();
            }

            if (info.OrganiserId != actorId && !actorIsAdmin)
            {
                return ServiceResponse<T>.Forbidden();
            }

            if (info.Status == MatchStatus.Cancelled)
            {
                return ServiceResponse<T>.Conflict("A cancelled match has no attendance.");
            }

            if (info.Status == MatchStatus.Completed)
            {
                return ServiceResponse<T>.Conflict("Attendance for this match was already recorded.");
            }

            var end = info.Date
                .ToDateTime(info.StartsAt)
                .AddMinutes(info.DurationMinutes);

            if (end > timeProvider.GetLocalNow().DateTime)
            {
                return ServiceResponse<T>.Conflict("Attendance can be marked after the match has ended.");
            }

            return null;
        }
    }
}