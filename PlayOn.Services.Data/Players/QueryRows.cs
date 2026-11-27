namespace PlayOn.Services.Data.Players
{
    /// <summary>
    /// Query row (projection) holding the basic data of a player.
    /// </summary>
    public sealed class PlayerRow
    {
        /// <summary>
        /// Unique identifier of the player.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Display name of the player.
        /// </summary>
        public string DisplayName { get; set; } = null!;
        /// <summary>
        /// City of the player.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Indicates whether the player's account is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Reliability score of the player, if one has been calculated.
        /// </summary>
        public int? ReliabilityScore { get; set; }
        /// <summary>
        /// Total number of matches the player has played.
        /// </summary>
        public int MatchesPlayed { get; set; }
    }
}
