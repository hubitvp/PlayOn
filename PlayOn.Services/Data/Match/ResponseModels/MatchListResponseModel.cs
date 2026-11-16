using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Sport.ResponseModels;

namespace PlayOn.Services.Data.Match.ResponseModels
{
    
    public class MatchListResponseModel
    {
        public required PageResult<MatchCardResponseModel> Matches { get; init; }
        public required IReadOnlyList<SportResponseModel> Sports { get; init; }
    }
}
