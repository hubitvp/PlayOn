using PlayOn.Common.Helpers;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Repositories.Data.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Services.Repositories.Contracts
{
    public interface IVenueRepository
    {
        Task<PagedResult<VenueDto>>
            GetVenuesAsync(VenueQueryDto venueQuery);
    }
}
