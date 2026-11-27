namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// A venue shown as a row in the administrator venues list.
    /// </summary>
    public class AdminVenueItemViewModel
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
        /// Formatted opening hours of the venue.
        /// </summary>
        public required string OpeningHours { get; init; }
        /// <summary>
        /// Indicates whether the venue has been deleted.
        /// </summary>
        public required bool IsDeleted { get; init; }
        /// <summary>
        /// Indicates whether the venue slot is already booked for a match.
        /// </summary>
        public required bool IsBooked { get; init; }
        /// <summary>
        /// Title of the match booked at the venue, if any.
        /// </summary>
        public string? MatchTitle { get; init; }
    }
}
