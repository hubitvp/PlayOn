using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Data.Matches.ResponseModels
{
    public class MatchFormOptionsResponseModel
    {
        public required IReadOnlyList<SportsResponseModel> Sports { get; init; }
        public required IReadOnlyList<CourtOptionResponseModel> Courts { get; init; }
        public required IReadOnlyList<VenueSlotResponseModel> Slots { get; init; }
    }



    public class VenueSlotResponseModel
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly ClosesAt { get; init; }
    }
}
