using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminVenues.RequestData;
using PlayOn.Services.Data.AdminVenues.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IAdminVenueService
    {
        Task<ServiceResponse<PageResult<AdminVenueResponseModel>>>
            GetVenuesAsync(AdminVenueQueryRequestData request);

        Task<ServiceResponse<AdminVenueEditResponseModel>>
            GetForEditAsync(Guid venueId);

        Task<ServiceResponse<Guid>>
            CreateAsync(AdminVenueCreateRequestData request);

        Task<ServiceResponse<Guid>>
            UpdateAsync(AdminVenueUpdateRequestData request);

        Task<ServiceResponse<Guid>>
            DeleteAsync(Guid venueId);

        Task<ServiceResponse<Guid>>
            RestoreAsync(Guid venueId);
    }
}
