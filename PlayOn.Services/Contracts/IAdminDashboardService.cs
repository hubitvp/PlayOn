using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminDashboard.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Provides aggregated statistics for the administration dashboard.
    /// </summary>
    public interface IAdminDashboardService
    {
        /// <summary>
        /// Builds the dashboard with platform-wide counts and the next upcoming matches.
        /// </summary>
        /// <returns>A successful response containing the dashboard summary.</returns>
        Task<ServiceResponse<AdminDashboardResponseModel>>
            GetDashboardAsync();
    }
}
