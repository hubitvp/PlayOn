namespace PlayOn.Services.Data.AdminVenues.ResponseModels
{
    public class AdminVenueResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly OpensAt { get; init; }
        public required TimeOnly ClosesAt { get; init; }
        public required bool IsDeleted { get; init; }
        public required bool IsBooked { get; init; }
        public string? MatchTitle { get; init; }
    }
}
