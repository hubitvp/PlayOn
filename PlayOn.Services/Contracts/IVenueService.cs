using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Data.Venue.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IVenueService
    {
        Task<ServiceResponse<PageResult<VenueResponseModel>>>
            GetVenuesAsync(VenueQueryRequestData venueQuery);
    }
}
