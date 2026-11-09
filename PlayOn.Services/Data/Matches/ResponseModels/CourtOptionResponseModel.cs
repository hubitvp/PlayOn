using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Matches.ResponseModels
{
    public class CourtOptionResponseModel
    {
        public required int CourtId { get; init; }
        public required string Name { get; init; } = null!;
        public required Surface Surface { get; init; }

        public required int Capacity { get; init; }

        public required IReadOnlyCollection<int> SportIds { get; init; }
    }
}
