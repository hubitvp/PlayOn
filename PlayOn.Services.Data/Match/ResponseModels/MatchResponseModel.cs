using PlayOn.Common.Helpers;

namespace PlayOn.Services.Data.Match.ResponseModels
{

    public class MatchResponseModel
    {
        public required PageResult<MatchCardResponseModel> Page { get; init; }
        public required MatchOptionsResponseModel Options { get; init; }
    }
}
