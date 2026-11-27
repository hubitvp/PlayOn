using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines the operations for retrieving venues.
    /// </summary>
    public interface IVenueService
    {
        /// <summary>
        /// Returns one page of upcoming venues.
        /// </summary>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the page of venues, or validation errors.</returns>
        Task<ServiceResponse<PageResult<VenueResponseModel>>>
            GetSinglePageAsync(PageRequestData pageRequestData);
    }
}
