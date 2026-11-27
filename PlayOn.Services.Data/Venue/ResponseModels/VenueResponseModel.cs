namespace PlayOn.Services.Data.Venue.ResponseModels
{
    /// <summary>
    /// Response model describing a venue together with its court and booked match.
    /// </summary>
    public class VenueResponseModel
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
        /// The court booked at the venue.
        /// </summary>
        public required VenueCourtResponseModel Court { get; init; }
        /// <summary>
        /// Identifier of the match booked at the venue.
        /// </summary>
        public required Guid MatchId { get; init; }
        /// <summary>
        /// Title of the match booked at the venue.
        /// </summary>
        public required string MatchTitle { get; init; }
    }
}
