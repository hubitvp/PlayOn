namespace PlayOn.Services.Data.AdminUsers.ResponseModels
{
    /// <summary>
    /// Response model describing a user in the administrator users list.
    /// </summary>
    public class AdminUserResponseModel
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Display name of the player.
        /// </summary>
        public required string DisplayName { get; init; }
        /// <summary>
        /// Email address of the user, if any.
        /// </summary>
        public string? Email { get; init; }
        /// <summary>
        /// City of the user.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        public required bool IsActive { get; init; }
        /// <summary>
        /// Indicates whether the user has the administrator role.
        /// </summary>
        public required bool IsAdministrator { get; init; }
        /// <summary>
        /// Total number of matches the user has played.
        /// </summary>
        public required int MatchesPlayed { get; init; }
        /// <summary>
        /// Reliability score of the user, if one has been calculated.
        /// </summary>
        public int? ReliabilityScore { get; init; }
    }
}
