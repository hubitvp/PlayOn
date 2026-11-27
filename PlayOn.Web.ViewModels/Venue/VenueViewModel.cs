namespace PlayOn.Web.ViewModels.Venue
{
    /// <summary>
    /// A venue (slot) shown on the venues page, together with its court and booked match.
    /// </summary>
    public class VenueViewModelData
    {
        /// <summary>
        /// Unique identifier of the venue.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string Name { get; init; }
        /// <summary>
        /// Street address of the venue.
        /// </summary>
        public required string Address { get; init; }
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Formatted date of the venue slot.
        /// </summary>
        public required string Date { get; init; }
        /// <summary>
        /// Formatted time the slot starts.
        /// </summary>
        public required string StartsAt { get; init; }
        /// <summary>
        /// Formatted time the venue opens.
        /// </summary>
        public required string OpensAt { get; init; }
        /// <summary>
        /// Formatted time the venue closes.
        /// </summary>
        public required string ClosesAt { get; init; }
        /// <summary>
        /// The court booked at the venue, if any.
        /// </summary>
        public VenueCourtViewModel? Court { get; init; }
        /// <summary>
        /// Identifier of the match booked at the venue, if any.
        /// </summary>
        public Guid? MatchId { get; init; }
        /// <summary>
        /// Title of the match booked at the venue, if any.
        /// </summary>
        public string? MatchTitle { get; init; }
        /// <summary>
        /// URL-friendly slug of the booked match, if any.
        /// </summary>
        public string? Slug { get; init; }
    }

    /// <summary>
    /// View model of the venues page: a page of venues.
    /// </summary>
    public class VenueViewModel : PageViewModel
    {
        /// <summary>
        /// Venues on the current page.
        /// </summary>
        public required IReadOnlyList<VenueViewModelData> Venues { get; init; }
    }
}