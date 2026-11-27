using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminDashboard.ResponseModels
{
    /// <summary>
    /// Summary of an upcoming match shown on the administrator dashboard.
    /// </summary>
    public class AdminUpcomingMatchResponseModel
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
        /// Date on which the match takes place.
        /// </summary>
        public required DateOnly Date { get; init; }
        /// <summary>
        /// Time the match starts.
        /// </summary>
        public required TimeOnly StartsAt { get; init; }
        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public required int Joined { get; init; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public required int MaxPlayers { get; init; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public required MatchStatus Status { get; init; }
    }
}
