using PlayOn.Common.Helpers;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Repositories.Data.Output;

namespace PlayOn.Services.Repositories.Contracts
{
    public interface IVenueRepository
    {
        Task<PagedResult<VenueDto>>
            GetVenuesAsync(VenueQueryDto venueQuery);
    }
}
