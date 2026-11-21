using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Attendance;
using PlayOn.Services.Data.Attendance.RequestData;
using PlayOn.Services.Data.Attendance.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Notification> notificationRepository;

        private readonly TimeProvider timeProvider;

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


        public async Task<ServiceResponse<AttendanceResponseModel>>
            GetAsync(
                Guid matchId,
                Guid actorId,
                bool actorIsAdmin)
        {
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
                .ThenBy(x => x.Id)
                .Select(AttendanceDataBuilder.AttendancePlayerProjection())
                .ToListAsync();

            return ServiceResponse<AttendanceResponseModel>
                .Success(AttendanceDataBuilder.AttendanceToResponseModel(info!, players));
        }


        public async Task<ServiceResponse<Guid>>
            SubmitAsync(AttendanceRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
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

            var entries = request.Entries
                .GroupBy(x => x.UserId)
                .ToDictionary(x => x.Key, x => x.Last().Attended);

            if (participants.Any(x => !entries.ContainsKey(x.UserId)))
            {
                return ServiceResponse<Guid>
                    .Failure("Mark attendance for every player.", "Players");
            }

            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            var notifications = ApplyAttendance(
                participants,
                entries,
                info!,
                nowUtc);

            var match = await matchRepository
                .Query(asNoTracking: false)
                .SingleAsync(x => x.Id == request.MatchId);

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
                var past = history.FirstOrDefault(x => x.UserId == user.Id);
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

        private async Task<AttendanceInfoRow?> GetInfoAsync(Guid matchId)
        {
            return await matchRepository
                .Query()
                .Where(x => x.Id == matchId && x.Event != null)
                .Select(AttendanceDataBuilder.AttendanceInfoProjection())
                .SingleOrDefaultAsync();
        }

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
