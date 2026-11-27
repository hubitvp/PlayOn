namespace PlayOn.Services.Data.AdminVenues.ResponseModels
{
    /// <summary>
    /// Response model holding the data needed to populate the edit form of a venue.
    /// </summary>
    public class AdminVenueEditResponseModel
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
        /// Geographic latitude of the venue, between -90 and 90.
        /// </summary>
        public required decimal Latitude { get; init; }
        /// <summary>
        /// Geographic longitude of the venue, between -180 and 180.
        /// </summary>
        public required decimal Longitude { get; init; }
        /// <summary>
        /// Date of the venue slot.
        /// </summary>
        public required DateOnly Date { get; init; }
        /// <summary>
        /// Time the venue slot starts.
        /// </summary>
        public required TimeOnly StartsAt { get; init; }
        /// <summary>
        /// Time the venue opens.
        /// </summary>
        public required TimeOnly OpensAt { get; init; }
        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public required TimeOnly ClosesAt { get; init; }
        /// <summary>
        /// Soft-delete flag indicating whether the venue has been deleted.
        /// </summary>
        public required bool IsDeleted { get; init; }
        /// <summary>
        /// Indicates whether the venue slot is already booked for a match.
        /// </summary>
        public required bool IsBooked { get; init; }
    }
}
