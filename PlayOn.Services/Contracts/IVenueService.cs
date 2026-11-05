using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Data.Venue.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Services.Contracts
{
    public interface IVenueService
    {
        Task<ServiceResponse<PagedResult<VenueResponseModel>>>
            GetVenuesAsync(VenueQueryRequestData venueQuery);
    }
}
