using Microsoft.AspNetCore.Mvc;
using PlayOn.Services.Contracts;
using PlayOn.Web.DataBuilders;

namespace PlayOn.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private readonly IAdminDashboardService dashboardService;
        private readonly AdminDashboardDataBuilder dashboardDataBuilder;

        public DashboardController(
            IAdminDashboardService dashboardService,
            AdminDashboardDataBuilder dashboardDataBuilder)
        {
            this.dashboardService = dashboardService;
            this.dashboardDataBuilder = dashboardDataBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await dashboardService.GetDashboardAsync();

            if (!response.IsSuccess)
            {
                var errorViewModel = dashboardDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = dashboardDataBuilder.DashboardToViewModel(response.ResponseData);

            return View(viewModel);
        }
    }
}