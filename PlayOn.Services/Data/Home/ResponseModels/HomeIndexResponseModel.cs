namespace PlayOn.Services.Data.Home.ResponseModels
{
    public class HomeIndexResponseModel
    {

        public required IEnumerable<SportsResponseModel> Sports { get; init; }

        public required IEnumerable<MatchCardResponseModel> MatchCards { get; init; }
    }
}
