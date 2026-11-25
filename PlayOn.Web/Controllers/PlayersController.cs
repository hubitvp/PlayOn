using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels.Players;

namespace PlayOn.Web.Controllers
{
    public class PlayersController : BaseController
    {
        private readonly IPlayerService playerService;
        private readonly PlayerDataBuilder playerDataBuilder;

        public PlayersController(
            IPlayerService playerService,
            PlayerDataBuilder playerDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.playerService = playerService;
            this.playerDataBuilder = playerDataBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = playerDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var userId = GetUserId();

            var response = await playerService.GetProfileAsync(userId, id);

            if (!response.IsSuccess)
            {
                var errorViewModel = playerDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = playerDataBuilder
                .PlayerProfileToViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = GetUserId();

            var serviceResponse = await playerService.GetForEditAsync(userId);

            if (!serviceResponse.IsSuccess)
            {
                var errorViewModel = playerDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = playerDataBuilder.PlayerEditToViewModel(serviceResponse.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await ReturnInvalidEditViewAsync(model);
            }

            var requestData = playerDataBuilder
                .PlayerEditToRequestData(GetUserId(), model);

            var serviceResponse = await playerService.UpdateAsync(requestData);

            if (serviceResponse.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(serviceResponse.Errors);
                return await ReturnInvalidEditViewAsync(model);
            }

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = playerDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "Your profile was saved.";

            return RedirectToAction(nameof(Details), new { id = GetUserId() });
        }

        private async Task<IActionResult> ReturnInvalidEditViewAsync(ProfileEditViewModel model)
        {
            var response = await playerService.GetForEditAsync(GetUserId());

            if (!response.IsSuccess)
            {
                var errorViewModel = playerDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            playerDataBuilder.PlayerEditApplyDisplayData(model, response.ResponseData);

            return View(nameof(Edit), model);
        }
    }
}