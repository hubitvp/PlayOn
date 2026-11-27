using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Players.ResponseModels
{
    /// <summary>
    /// Response model holding the public profile and statistics of a player.
    /// </summary>
    public class PlayerProfileResponseModel
    {
        /// <summary>
        /// Unique identifier of the player.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Display name of the player.
        /// </summary>
        public required string DisplayName { get; init; }
        /// <summary>
        /// City of the player.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Reliability score of the player, if one has been calculated.
        /// </summary>
        public int? ReliabilityScore { get; init; }
        /// <summary>
        /// Total number of matches the player has played.
        /// </summary>
        public required int MatchesPlayed { get; init; }
        /// <summary>
        /// Number of matches the player attended.
        /// </summary>
        public required int AttendedCount { get; init; }
        /// <summary>
        /// Number of matches the player did not show up for.
        /// </summary>
        public required int NoShowCount { get; init; }
        /// <summary>
        /// Number of times the player cancelled too close to the start of a match.
        /// </summary>
        public required int LateCancelCount { get; init; }
        /// <summary>
        /// Number of ratings received by the player.
        /// </summary>
        public required int RatingCount { get; init; }
        /// <summary>
        /// Average score of the ratings received by the player, if any.
        /// </summary>
        public double? AverageRating { get; init; }
        /// <summary>
        /// Indicates whether the profile belongs to the current user.
        /// </summary>
        public required bool IsOwnProfile { get; init; }
        /// <summary>
        /// Skill levels of the player per sport.
        /// </summary>
        public required IReadOnlyList<PlayerSkillResponseModel> Skills { get; init; }
        /// <summary>
        /// Most recent ratings received by the player.
        /// </summary>
        public required IReadOnlyList<PlayerRatingResponseModel> RecentRatings { get; init; }
    }

    /// <summary>
    /// A skill level of a player in a sport, shown on the player profile.
    /// </summary>
    public class PlayerSkillResponseModel
    {
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required SportIcon SportIcon { get; init; }
        /// <summary>
        /// Skill level of the player in the sport.
        /// </summary>
        public required SkillLevel SkillLevel { get; init; }
    }

    /// <summary>
    /// A rating received by a player, shown on the player profile.
    /// </summary>
    public class PlayerRatingResponseModel
    {
        /// <summary>
        /// Display name of the user who gave the rating.
        /// </summary>
        public required string RaterName { get; init; }
        /// <summary>
        /// Score given by the rater.
        /// </summary>
        public required int Score { get; init; }
        /// <summary>
        /// Optional comment given by the rater.
        /// </summary>
        public string? Comment { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string MatchTitle { get; init; }
    }

    /// <summary>
    /// Response model holding the data needed to populate the player profile edit form.
    /// </summary>
    public class PlayerEditResponseModel
    {
        /// <summary>
        /// Identifier of the user.
        /// </summary>
        public required Guid UserId { get; init; }
        /// <summary>
        /// Display name of the player.
        /// </summary>
        public required string DisplayName { get; init; }
        /// <summary>
        /// City of the player.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Skill levels of the player per sport.
        /// </summary>
        public required IReadOnlyList<PlayerSkillOptionResponseModel> Skills { get; init; }
    }

    /// <summary>
    /// A sport together with the skill level of the player in it, shown on the profile edit form.
    /// </summary>
    public class PlayerSkillOptionResponseModel
    {
        /// <summary>
        /// Identifier of the sport.
        /// </summary>
        public required Guid SportId { get; init; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Skill level of the player in the sport, or <c>null</c> if not set.
        /// </summary>
        public SkillLevel? SkillLevel { get; init; }
    }
}
