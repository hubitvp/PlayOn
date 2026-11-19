using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminUsers.RequestData;
using PlayOn.Services.Data.AdminUsers.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IAdminUserService
    {
        Task<ServiceResponse<PageResult<AdminUserResponseModel>>>
            GetUsersAsync(AdminUserQueryRequestData request);

        Task<ServiceResponse<Guid>>
            SetActiveAsync(AdminUserStatusRequestData request);

        Task<ServiceResponse<Guid>>
            SetAdministratorAsync(AdminUserRoleRequestData request);
    }
}
