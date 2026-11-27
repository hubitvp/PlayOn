using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines the operations that supply data for the home page.
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// Builds the full home page model with one page of match cards and the sport options.
        /// </summary>
        /// <param name="searchRequestData">The search filters (sport, city, start date).</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the home model, or errors if validation or loading fails.</returns>
        Task<ServiceResponse<HomeResponseModel>>
            GetMainResponseModelAsync(
                HomeSearchRequestData searchRequestData,
                PageRequestData pageRequestData);

        /// <summary>
        /// Returns one page of match cards matching the search filters.
        /// </summary>
        /// <param name="searchRequestData">The search filters (sport, city, start date).</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the page of match cards, or validation errors.</returns>
        Task<ServiceResponse<PageResult<HomeMatchCardResponseModel>>>
            GetSinglePageAsync(
                HomeSearchRequestData searchRequestData,
                PageRequestData pageRequestData);

        /// <summary>
        /// Retrieves the sports available as filter options.
        /// </summary>
        /// <returns>A response with the sport options.</returns>
        Task<ServiceResponse<IReadOnlyList<HomeSportOptionsResponseModel>>>
            GetSportOptionsAsync();
    }
}
