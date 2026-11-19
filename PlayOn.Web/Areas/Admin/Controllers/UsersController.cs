using Microsoft.AspNetCore.Mvc;
using PlayOn.Services.Contracts;
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

            var requestData = userDataBuilder.UserQueryToRequestData(input);

            var response = await userService.GetUsersAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(userDataBuilder.UserListToViewModel(null, input, GetUserId()));
            }

            var viewModel = userDataBuilder.UserListToViewModel(response.ResponseData, input, GetUserId());

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(Guid id, bool isActive, AdminUserSearchInputModel filter)
        {
            var requestData = userDataBuilder.UserStatusToRequestData(id, GetUserId(), isActive);

            var response = await userService.SetActiveAsync(requestData);

            if (response.IsSuccess)
            {
                TempData["Success"] = isActive ? "The account was activated." : "The account was deactivated.";
            }
            else
            {
                TempData["Error"] = "";// response.FirstErrorMessage();
            }

            return RedirectToIndex(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAdministrator(Guid id, bool isAdministrator, AdminUserSearchInputModel filter)
        {
            var requestData = userDataBuilder.UserRoleToRequestData(id, GetUserId(), isAdministrator);

            var response = await userService.SetAdministratorAsync(requestData);

            if (response.IsSuccess)
            {
                TempData["Success"] = isAdministrator
                    ? "The user is now an administrator."
                    : "Administrator rights were removed.";
            }
            else
            {
                TempData["Error"] = "";// response.FirstErrorMessage();
            }

            return RedirectToIndex(filter);
        }

        private IActionResult RedirectToIndex(AdminUserSearchInputModel filter)
        {
            return RedirectToAction(nameof(Index), new { filter.Search, filter.Status, filter.Page });
        }
    }
}
