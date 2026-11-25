using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
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

            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

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

        private async Task<RatingInfoRow?> GetInfoAsync(Guid matchId)
        {
            return await matchRepository
                .Query()
                .Where(x => x.Id == matchId)
                .Select(RatingDataBuilder.RatingInfoProjection())
                .SingleOrDefaultAsync();
        }

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