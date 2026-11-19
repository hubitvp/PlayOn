using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminDashboard.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IAdminDashboardService
    {
        Task<ServiceResponse<AdminDashboardResponseModel>>
            GetDashboardAsync();
    }
}
