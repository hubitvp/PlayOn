namespace PlayOn.Services.Data.AdminUsers
{
    /// <summary>
    /// Query row (projection) holding user data, used by the administrator users list.
    /// </summary>
    public sealed class AdminUserRow
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Display name of the player.
        /// </summary>
        public string DisplayName { get; set; } = null!;
        /// <summary>
        /// Email address of the user, if any.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// City of the user.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Total number of matches the user has played.
        /// </summary>
        public int MatchesPlayed { get; set; }
        /// <summary>
        /// Reliability score of the user, if one has been calculated.
        /// </summary>
        public int? ReliabilityScore { get; set; }
    }
}
