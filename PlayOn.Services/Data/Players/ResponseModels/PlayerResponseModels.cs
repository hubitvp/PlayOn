using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Players.ResponseModels
{
    public class PlayerProfileResponseModel
    {
        public required Guid Id { get; init; }
        public required string DisplayName { get; init; }
        public required string City { get; init; }
        public int? ReliabilityScore { get; init; }
        public required int MatchesPlayed { get; init; }
        public required int AttendedCount { get; init; }
        public required int NoShowCount { get; init; }
        public required int LateCancelCount { get; init; }
        public required int RatingCount { get; init; }
        public double? AverageRating { get; init; }
        public required IReadOnlyList<PlayerSkillResponseModel> Skills { get; init; }
        public required IReadOnlyList<PlayerRatingResponseModel> RecentRatings { get; init; }
    }

    public class PlayerSkillResponseModel
    {
        public required string SportName { get; init; }
        public required SportIcon SportIcon { get; init; }
        public required SkillLevel SkillLevel { get; init; }
    }

    public class PlayerRatingResponseModel
    {
        public required string RaterName { get; init; }
        public required int Score { get; init; }
        public string? Comment { get; init; }
        public required string MatchTitle { get; init; }
    }

    public class PlayerEditResponseModel
    {
        public required Guid UserId { get; init; }
        public required string DisplayName { get; init; }
        public required string City { get; init; }
        public required IReadOnlyList<PlayerSkillOptionResponseModel> Skills { get; init; }
    }

    public class PlayerSkillOptionResponseModel
    {
        public required Guid SportId { get; init; }
        public required string SportName { get; init; }
        public SkillLevel? SkillLevel { get; init; }
    }
}
