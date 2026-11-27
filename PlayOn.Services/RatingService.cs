using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Ratings;
using PlayOn.Services.Data.Ratings.RequestData;
using PlayOn.Services.Data.Ratings.ResponseModels;
using PlayOn.Services.DataBuilders;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.PlayerRatingConstants;

namespace PlayOn.Services
{
    /// <summary>
    /// Provides the rating form for a completed match and submission of ratings for fellow attendees.
    /// </summary>
    public class RatingService : IRatingService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<PlayerRating> ratingRepository;
        private readonly IRepository<Notification> notificationRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingService"/> class.
        /// </summary>
        /// <param name="matchRepository">Repository for matches.</param>
        /// <param name="participantRepository">Repository for match participants.</param>
        /// <param name="ratingRepository">Repository for player ratings.</param>
        /// <param name="notificationRepository">Repository for notifications.</param>
        /// <param name="timeProvider">Provider of the current local time.</param>
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

        /// <summary>
        /// Builds the rating form listing the attendees the rater has not rated yet.
        /// Only attendees of a completed match can rate; others get not found, forbidden or a conflict.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="raterId">The identifier of the rating user.</param>
        /// <returns>A service response with the form, or a failure if the match is missing or deleted, the rater did not attend, or the match is not completed.</returns>
        public async Task<ServiceResponse<RatingFormResponseModel>>
            GetFormAsync(
                Guid matchId,
                Guid raterId)
        {
            var info = await GetInfoAsync(matchId);

            var failure = await CheckAccessAsync<RatingFormResponseModel>(info, raterId);
            if (failure != null)
            {
                return failure;
            }

            var players = await GetRatablePlayersAsync(matchId, raterId);

            var responseData = RatingDataBuilder.RatingFormToResponseModel(
                matchId,
                info!.Title,
                players);

            return ServiceResponse<RatingFormResponseModel>.Success(responseData);
        }

        /// <summary>
        /// Validates and saves ratings from a rater for other attendees of a completed match and notifies each rated player.
        /// Fails if no entries are given, access checks fail, or any entry has an out-of-range score, too long a comment, or a player who is not ratable or is duplicated.
        /// </summary>
        /// <param name="request">The rating request with the match, rater and entries.</param>
        /// <returns>A service response with the number of ratings saved.</returns>
        public async Task<ServiceResponse<int>>
            SubmitAsync(RatingRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request).ToList();
            if (errors.Any())
            {
                return ServiceResponse<int>.Failure(errors);
            }

            var entries = request.Entries;
            if (entries.Count == 0)
            {
                return ServiceResponse<int>
                    .Failure("Rate at least one player.", "Players");
            }

            var info = await GetInfoAsync(request.MatchId);

            var failure = await CheckAccessAsync<int>(info, request.RaterId);
            if (failure != null)
            {
                return failure;
            }

            var ratablePlayers = await GetRatablePlayersAsync(
                request.MatchId,
                request.RaterId);

            var entryErrors = ValidateEntries(
                entries,
                ratablePlayers.Select(x => x.UserId).ToHashSet());

            if (entryErrors.Any())
            {
                return ServiceResponse<int>.Failure(entryErrors);
            }

            var nowUtc = timeProvider.GetLocalNow().LocalDateTime;

            var ratings = entries
                .Select(x => RatingDataBuilder.RatingToEntity(
                    request.MatchId,
                    request.RaterId,
                    x))
                .ToList();

            var notifications = entries
                .Select(x => NotificationDataBuilder.NotificationToEntity(
                    x.RatedUserId,
                    NotificationType.RatingReceived,
                    $"You received a new rating after \"{info!.Title}\".",
                    request.MatchId,
                    nowUtc))
                .ToList();

            await ratingRepository.AddRangeAsync(ratings);
            await notificationRepository.AddRangeAsync(notifications);
            await ratingRepository.SaveChangesAsync();

            return ServiceResponse<int>.Success(ratings.Count);
        }

        /// <summary>
        /// Loads the basic information of a match for rating.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <returns>The match information, or <c>null</c> if the match does not exist.</returns>
        private async Task<RatingInfoRow?> GetInfoAsync(Guid matchId)
        {
            return await matchRepository
                .Query()
                .Where(x => x.Id == matchId)
                .Select(RatingDataBuilder.RatingInfoProjection())
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Checks that the rater may rate in the match: it exists and is not deleted, the rater attended, and the match is completed.
        /// </summary>
        /// <typeparam name="T">The response data type of the service response.</typeparam>
        /// <param name="info">The match information, or <c>null</c> if not found.</param>
        /// <param name="raterId">The identifier of the rating user.</param>
        /// <returns>A not found, forbidden or conflict failure, or <c>null</c> if access is allowed.</returns>
        private async Task<ServiceResponse<T>?> CheckAccessAsync<T>(
            RatingInfoRow? info,
            Guid raterId)
        {
            if (info == null || info.IsDeleted)
            {
                return ServiceResponse<T>.NotFound();
            }

            var hasAttended = await participantRepository
                .Query()
                .AnyAsync(x => x.MatchId == info.Id
                    && x.UserId == raterId
                    && x.Status == ParticipantStatus.Attended);

            if (!hasAttended)
            {
                return ServiceResponse<T>.Forbidden();
            }

            if (info.Status != MatchStatus.Completed)
            {
                return ServiceResponse<T>
                    .Conflict("You can rate players after the match is completed.");
            }

            return null;
        }

        /// <summary>
        /// Gets the attendees of a match, other than the rater, whom the rater has not rated yet, ordered by display name.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="raterId">The identifier of the rating user.</param>
        /// <returns>The list of ratable players.</returns>
        private async Task<IReadOnlyList<RatingPlayerResponseModel>>
            GetRatablePlayersAsync(Guid matchId, Guid raterId)
        {
            var alreadyRatedIds = await ratingRepository
                .Query()
                .Where(x => x.MatchId == matchId && x.RaterId == raterId)
                .Select(x => x.RatedUserId)
                .ToListAsync();

            return await participantRepository
                .Query()
                .Where(x => x.MatchId == matchId
                    && x.Status == ParticipantStatus.Attended
                    && x.UserId != raterId
                    && !alreadyRatedIds.Contains(x.UserId))
                .OrderBy(x => x.User.DisplayName)
                .Select(RatingDataBuilder.RatingPlayerProjection())
                .ToListAsync();
        }

        /// <summary>
        /// Validates rating entries: the score must be in range, the comment within the maximum length, and each rated player must be ratable and appear only once.
        /// </summary>
        /// <param name="entries">The submitted rating entries.</param>
        /// <param name="ratablePlayerIds">The identifiers of players the rater may rate.</param>
        /// <returns>The validation errors, empty if all entries are valid.</returns>
        private static IReadOnlyList<ValidationResult> ValidateEntries(
            IReadOnlyList<RatingEntryRequestData> entries,
            HashSet<Guid> ratablePlayerIds)
        {
            var errors = new List<ValidationResult>();
            var seenPlayerIds = new HashSet<Guid>();

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                var scoreMember = $"Players[{i}].Score";

                if (entry.Score < MinRatingScore || entry.Score > MaxRatingScore)
                {
                    errors.Add(new ValidationResult(
                        $"The score must be between {MinRatingScore} and {MaxRatingScore}.",
                        new[] { scoreMember }));
                }

                if (entry.Comment is { } comment
                    && comment.Trim().Length > CommentMaxLength)
                {
                    errors.Add(new ValidationResult(
                        $"The comment can have at most {CommentMaxLength} characters.",
                        new[] { $"Players[{i}].Comment" }));
                }

                if (entry.RatedUserId == Guid.Empty
                    || !ratablePlayerIds.Contains(entry.RatedUserId)
                    || !seenPlayerIds.Add(entry.RatedUserId))
                {
                    errors.Add(new ValidationResult(
                        "This player cannot be rated for this match.",
                        new[] { scoreMember }));
                }
            }

            return errors;
        }
    }
}