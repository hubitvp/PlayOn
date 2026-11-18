using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Ratings.RequestData;
using PlayOn.Services.Data.Ratings.ResponseModels;
using PlayOn.Services.DataBuilders;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.ParticipationRules;
using static PlayOn.Common.EntityConstants.PlayerRatingConstants;

namespace PlayOn.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<PlayerRating> ratingRepository;
        private readonly IRepository<Notification> notificationRepository;

        private readonly TimeProvider timeProvider;

        public RatingService(
            IRepository<Match> matchRepository,
            IRepository<MatchParticipant> participantRepository,
            IRepository<PlayerRating> ratingRepository,
            IRepository<Notification> notificationRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.participantRepository = participantRepository;
            this.ratingRepository = ratingRepository;
            this.notificationRepository = notificationRepository;
            this.timeProvider = timeProvider;
        }


        public async Task<ServiceResponse<RatingFormResponseModel>>
            GetFormAsync(
                Guid matchId,
                Guid raterId)
        {
            var access = await CheckAccessAsync(matchId, raterId);

            if (access.Status != ServiceStatus.Success)
            {
                return ToFailure<RatingFormResponseModel>(access.Status, access.Message);
            }

            var players = await GetRatablePlayersAsync(matchId, raterId);

            return ServiceResponse<RatingFormResponseModel>
                .Success(RatingDataBuilder.RatingFormToResponseModel(matchId, access.Title!, players));
        }


        public async Task<ServiceResponse<int>>
            SubmitAsync(RatingRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<int>.Failure(errors);
            }

            if (request.Entries.Count == 0)
            {
                return ServiceResponse<int>.Failure("Rate at least one player.", "Players");
            }

            var access = await CheckAccessAsync(request.MatchId, request.RaterId);

            if (access.Status != ServiceStatus.Success)
            {
                return ToFailure<int>(access.Status, access.Message);
            }

            var allowedIds = (await GetRatablePlayersAsync(request.MatchId, request.RaterId))
                .Select(x => x.UserId)
                .ToHashSet();

            var entryErrors = new List<ValidationResult>();
            var seen = new HashSet<Guid>();

            for (var i = 0; i < request.Entries.Count; i++)
            {
                var entry = request.Entries[i];
                var scoreMember = $"Players[{i}].Score";

                if (entry.Score < MinRatingScore || entry.Score > MaxRatingScore)
                {
                    entryErrors.Add(new ValidationResult(
                        $"The score must be between {MinRatingScore} and {MaxRatingScore}.",
                        new[] { scoreMember }));
                }

                if (entry.Comment is { } comment && comment.Trim().Length > CommentMaxLength)
                {
                    entryErrors.Add(new ValidationResult(
                        $"The comment can have at most {CommentMaxLength} characters.",
                        new[] { $"Players[{i}].Comment" }));
                }

                if (entry.RatedUserId == Guid.Empty
                    || !allowedIds.Contains(entry.RatedUserId)
                    || !seen.Add(entry.RatedUserId))
                {
                    entryErrors.Add(new ValidationResult(
                        "This player cannot be rated for this match.",
                        new[] { scoreMember }));
                }
            }

            if (entryErrors.Count > 0)
            {
                return ServiceResponse<int>.Failure(entryErrors);
            }

            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            var ratings = request.Entries
                .Select(x => RatingDataBuilder.RatingToEntity(request.MatchId, request.RaterId, x))
                .ToList();

            //var notifications = request.Entries
            //    .Select(x => NotificationDataBuilder.NotificationToEntity(
            //        x.RatedUserId,
            //        NotificationType.RatingReceived,
            //        $"You received a new rating after \"{access.Title}\".",
            //        request.MatchId,
            //        nowUtc))
            //    .ToList();

            IEnumerable<Notification> notifications = [];

            await ratingRepository.AddRangeAsync(ratings);
            await notificationRepository.AddRangeAsync(notifications);
            await ratingRepository.SaveChangesAsync();

            return ServiceResponse<int>.Success(ratings.Count);
        }


        private async Task<(ServiceStatus Status, string? Message, string? Title)>
            CheckAccessAsync(Guid matchId, Guid raterId)
        {
            var match = await matchRepository
                .Query()
                .Where(x => x.Id == matchId)
                .Select(x => new { x.Title, x.Status, x.IsDeleted })
                .SingleOrDefaultAsync();

            if (match == null || match.IsDeleted)
            {
                return (ServiceStatus.NotFound, null, null);
            }

            var attended = await participantRepository
                .Query()
                .AnyAsync(x => x.MatchId == matchId
                    && x.UserId == raterId
                    && x.Status == ParticipantStatus.Attended);

            if (!attended)
            {
                return (ServiceStatus.Forbidden, null, null);
            }

            if (match.Status != MatchStatus.Completed)
            {
                return (
                    ServiceStatus.Conflict,
                    "You can rate players after the match is completed.",
                    null);
            }

            return (ServiceStatus.Success, null, match.Title);
        }

        private static ServiceResponse<T>
            ToFailure<T>(ServiceStatus status, string? message)
        {
            return status switch
            {
                ServiceStatus.NotFound => ServiceResponse<T>.NotFound(),
                ServiceStatus.Forbidden => ServiceResponse<T>.Forbidden(),
                _ => ServiceResponse<T>.Conflict(message ?? "This action is not possible right now.")
            };
        }

        private async Task<IReadOnlyList<RatingPlayerResponseModel>>
            GetRatablePlayersAsync(Guid matchId, Guid raterId)
        {
            var alreadyRated = await ratingRepository
                .Query()
                .Where(x => x.MatchId == matchId && x.RaterId == raterId)
                .Select(x => x.RatedUserId)
                .ToListAsync();

            return await participantRepository
                .Query()
                .Where(x => x.MatchId == matchId
                    && x.Status == ParticipantStatus.Attended
                    && x.UserId != raterId
                    && !alreadyRated.Contains(x.UserId))
                .OrderBy(x => x.User.DisplayName)
                .Select(RatingDataBuilder.RatingPlayerProjection())
                .ToListAsync();
        }
    }
}
