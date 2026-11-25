namespace PlayOn.Services.Data.Venue.ResponseModels
{
    public class VenueResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly OpensAt { get; init; }
        public required TimeOnly ClosesAt { get; init; }
        public required VenueCourtResponseModel Court { get; init; }
        public required Guid MatchId { get; init; }
        public required string MatchTitle { get; init; }
    }
}
