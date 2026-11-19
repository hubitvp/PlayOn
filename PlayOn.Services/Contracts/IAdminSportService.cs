using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminSports.RequestData;
using PlayOn.Services.Data.AdminSports.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IAdminSportService
    {
        Task<ServiceResponse<PageResult<AdminSportResponseModel>>>
            GetSportsAsync(AdminSportQueryRequestData request);

        Task<ServiceResponse<AdminSportEditResponseModel>>
            GetForEditAsync(Guid sportId);

        Task<ServiceResponse<Guid>>
            CreateAsync(AdminSportCreateRequestData request);

        Task<ServiceResponse<Guid>>
            UpdateAsync(AdminSportUpdateRequestData request);

        Task<ServiceResponse<Guid>>
            DeleteAsync(Guid sportId);
    }
}
