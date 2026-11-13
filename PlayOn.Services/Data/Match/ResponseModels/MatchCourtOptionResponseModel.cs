using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match.ResponseModels
{
    public class MatchCourtOptionResponseModel
    {
        public required Guid CourtId { get; init; }
        public required string Name { get; init; } = null!;
        public required Surface Surface { get; init; }

        public required int Capacity { get; init; }

        public required IReadOnlyCollection<Guid> SportIds { get; init; }
    }
}
