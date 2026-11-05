using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Data.Home.Output
{
    public class HomeIndexResponseModel
    {

        public required IEnumerable<SportsResponseModel> Sports { get; init; }

        public required IEnumerable<MatchCardResponseModel> MatchCards { get; init; }
    }
}
