namespace PlayOn.Services.Data.Match.ResponseModels
{
    public class MatchCancelResponseModel
    {
        public required string Title { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required DateOnly Date { get; init; }
        public required string VenueName { get; init; }
        public required int JoinedCount { get; init; }
    }
}
