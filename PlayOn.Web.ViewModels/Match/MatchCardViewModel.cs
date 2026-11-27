using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// Summary card of a match shown in match lists.
    /// </summary>
    public class MatchCardViewModel
    {

        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// URL-friendly slug of the match, used to build its address.
        /// </summary>
        public required string Slug { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string Title { get; init; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required string SportIcon { get; init; }
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
        /// Duration of the match in minutes.
        /// </summary>
        public required int DurationMinutes { get; init; }
        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public required int Joined { get; init; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public required int MaxPlayers { get; init; }
        /// <summary>
        /// Number of players on the waiting list of the match.
        /// </summary>
        public required int Waitlisted { get; init; }
        /// <summary>
        /// Minimum skill level required to join the match, if any.
        /// </summary>
        public required SkillLevel? MinSkill { get; init; }
        /// <summary>
        /// Formatted price each player pays to join the match.
        /// </summary>
        public required string PricePerPlayer { get; init; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public required MatchStatus Status { get; init; }
    }
}
