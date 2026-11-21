
using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match.ResponseModels
{

    public class SportOptionResponseModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
    }


    public class CourtOptionResponseModel
    {
        public required Guid CourtId { get; init; }
        public required string Name { get; init; } = null!;
        public required Surface Surface { get; init; }

        public required int Capacity { get; init; }

        public required IReadOnlyCollection<Guid> SportIds { get; init; }
    }


    public class VenueOptionResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly ClosesAt { get; init; }
    }

    public class MatchOptionsResponseModel
    {
        public required IReadOnlyList<SportOptionResponseModel> SportOptions { get; init; }
        public required IReadOnlyList<CourtOptionResponseModel> CourtOptions { get; init; }
        public required IReadOnlyList<VenueOptionResponseModel> VenueOptions { get; init; }
    }

}
