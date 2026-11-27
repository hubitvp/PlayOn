using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Participation
{
    /// <summary>
    /// View model of the page listing the current user's matches.
    /// </summary>
    public class MyMatchesViewModel : PageViewModel
    {
        /// <summary>
        /// The tab (e.g. upcoming or past) selecting which of the user's matches are shown.
        /// </summary>
        public required MyMatchesTab Tab { get; init; }

        /// <summary>
        /// Matches on the current page.
        /// </summary>
        public required IReadOnlyList<MyMatchViewModel> Matches { get; init; }
    }

    /// <summary>
    /// A match in the list of the current user's matches.
    /// </summary>
    public class MyMatchViewModel
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
        /// Participation status of the user in the match.
        /// </summary>
        public required ParticipantStatus Status { get; init; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public required MatchStatus MatchStatus { get; init; }
        /// <summary>
        /// Position on the waiting list, or 0 if the user is not on it.
        /// </summary>
        public required int WaitlistPosition { get; init; }
        /// <summary>
        /// Indicates whether the current user can leave the match.
        /// </summary>
        public required bool CanLeave { get; init; }
        /// <summary>
        /// Indicates whether the current user can rate the players of the match.
        /// </summary>
        public required bool CanRate { get; init; }
    }
}
