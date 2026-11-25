namespace PlayOn.Services.Data.Ratings.ResponseModels
{
    public class RatingFormResponseModel
    {
        public required Guid MatchId { get; init; }
        public required string MatchTitle { get; init; }
        public required IReadOnlyList<RatingPlayerResponseModel> Players { get; init; }
    }

    public class RatingPlayerResponseModel
    {
        public required Guid UserId { get; init; }
        public required string DisplayName { get; init; }
        public required string City { get; init; }
    }
}
