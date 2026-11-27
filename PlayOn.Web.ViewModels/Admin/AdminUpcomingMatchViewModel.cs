namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// An upcoming match shown on the administrator dashboard.
    /// </summary>
    public class AdminUpcomingMatchViewModel
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string Title { get; init; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string VenueName { get; init; }
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Formatted date and time the match starts.
        /// </summary>
        public required string StartsAt { get; init; }
        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public required int Joined { get; init; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public required int MaxPlayers { get; init; }
        /// <summary>
        /// Display text of the match status.
        /// </summary>
        public required string Status { get; init; }
    }
}
