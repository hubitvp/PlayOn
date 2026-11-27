using PlayOn.Common.Helpers;

namespace PlayOn.Services.Data.Home.ResponseModels
{
    /// <summary>
    /// Response model of the home page: a page of match cards and the available sport filter options.
    /// </summary>
    public class HomeResponseModel
    {
        /// <summary>
        /// Page of match cards.
        /// </summary>
        public required PageResult<HomeMatchCardResponseModel> Page { get; init; }
        /// <summary>
        /// Sports available in the filter.
        /// </summary>
        public required IReadOnlyList<HomeSportOptionsResponseModel> SportOptions { get; init; }
    }
}