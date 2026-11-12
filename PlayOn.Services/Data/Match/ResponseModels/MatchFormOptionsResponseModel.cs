using PlayOn.Services.Data.Sport.ResponseModels;

namespace PlayOn.Services.Data.Matches.ResponseModels
{
    public class MatchFormOptionsResponseModel
    {
        public required IReadOnlyList<SportResponseModel> Sports { get; init; }
        public required IReadOnlyList<MatchCourtOptionResponseModel> Courts { get; init; }
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
