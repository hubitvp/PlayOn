using Microsoft.AspNetCore.Mvc;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels.Admin;

namespace PlayOn.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IAdminUserService userService;
        private readonly AdminUserDataBuilder userDataBuilder;

        public UsersController(
            IAdminUserService userService,
            AdminUserDataBuilder userDataBuilder)
        {
            this.userService = userService;
            this.userDataBuilder = userDataBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AdminUserSearchInputModel input)
        {
            input.Page = Math.Max(1, input.Page);

            if (!ModelState.IsValid)
            {
                return await ReturnInvalidIndexViewAsync(input);
            }

            var requestData = userDataBuilder.UserQueryToRequestData(input);

            var response = await userService.GetUsersAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return await ReturnInvalidIndexViewAsync(input);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = userDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = userDataBuilder.UserListToViewModel(response.ResponseData, input, GetUserId());

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(Guid id, bool isActive, AdminUserSearchInputModel filter)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = userDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var requestData = userDataBuilder.UserStatusToRequestData(id, GetUserId(), isActive);

            var response = await userService.SetActiveAsync(requestData);

            if (!response.IsSuccess)
            {
                var errorViewModel = userDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = isActive
                ? "The account was activated."
                : "The account was deactivated.";

            return RedirectToIndex(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAdministrator(Guid id, bool isAdministrator, AdminUserSearchInputModel filter)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = userDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var requestData = userDataBuilder.UserRoleToRequestData(id, GetUserId(), isAdministrator);

            var response = await userService.SetAdministratorAsync(requestData);

            if (!response.IsSuccess)
            {
                var errorViewModel = userDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = isAdministrator
                ? "The user is now an administrator."
                : "Administrator rights were removed.";

            return RedirectToIndex(filter);
        }

        private async Task<IActionResult> ReturnInvalidIndexViewAsync(AdminUserSearchInputModel input)
        {
            var defaultInput = new AdminUserSearchInputModel { Page = 1 };
            var requestData = userDataBuilder.UserQueryToRequestData(defaultInput);

            var response = await userService.GetUsersAsync(requestData);

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = userDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = userDataBuilder.UserListToViewModel(response.ResponseData, defaultInput, GetUserId());

            return View(nameof(Index), viewModel);
        }

        private IActionResult RedirectToIndex(AdminUserSearchInputModel filter)
        {
            return RedirectToAction(nameof(Index), new { filter.Search, filter.Status, filter.Page });
        }
    }
}