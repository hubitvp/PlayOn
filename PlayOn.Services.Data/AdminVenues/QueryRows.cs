namespace PlayOn.Services.Data.AdminVenues
{
    /// <summary>
    /// Query row (projection) holding a venue and its booking state, used by the administrator venues list.
    /// </summary>
    public sealed class AdminVenueRow
    {
        /// <summary>
        /// Unique identifier of the venue.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Street address of the venue.
        /// </summary>
        public string Address { get; set; } = null!;
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Date of the venue slot.
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Time the venue slot starts.
        /// </summary>
        public TimeOnly StartsAt { get; set; }
        /// <summary>
        /// Time the venue opens.
        /// </summary>
        public TimeOnly OpensAt { get; set; }
        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public TimeOnly ClosesAt { get; set; }
        /// <summary>
        /// Soft-delete flag indicating whether the venue has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Indicates whether the venue slot is already booked for a match.
        /// </summary>
        public bool IsBooked { get; set; }
        /// <summary>
        /// Title of the match booked at the venue, if any.
        /// </summary>
        public string? MatchTitle { get; set; }
    }
}
