using PlayOn.Common.Helpers;

namespace PlayOn.Services.Data.Match.ResponseModels
{

    /// <summary>
    /// Response model of the match list: a page of match cards and the available filter options.
    /// </summary>
    public class MatchResponseModel
    {
        /// <summary>
        /// Page of match cards.
        /// </summary>
        public required PageResult<MatchCardResponseModel> Page { get; init; }
        /// <summary>
        /// Options available for selecting sport, court and venue.
        /// </summary>
        public required MatchOptionsResponseModel Options { get; init; }
    }
}
