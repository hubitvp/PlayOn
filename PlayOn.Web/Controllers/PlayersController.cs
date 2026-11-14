using Microsoft.AspNetCore.Authorization;
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


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await playerService.GetProfileAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            var isOwnProfile = IsUserAuthenticated() && GetUserId() == id;

            var viewModel = playerDataBuilder
                .PlayerProfileToViewModel(response.ResponseData, isOwnProfile);

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var response = await playerService.GetForEditAsync(GetUserId());

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            var viewModel = playerDataBuilder.PlayerEditToViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await ShowFormAgainAsync(model);
            }

            var requestData = playerDataBuilder
                .PlayerEditToRequestData(GetUserId(), model);

            var response = await playerService.UpdateAsync(requestData);

            if (response.IsSuccess)
            {
                TempData["Success"] = "Your profile was saved.";

                return RedirectToAction(nameof(Details), new { id = GetUserId() });
            }

            if (response.Status != ServiceStatus.Invalid)
            {
                return NotFound();
            }

            AddServiceErrors(response.Errors);

            return await ShowFormAgainAsync(model);
        }


        private async Task<IActionResult> ShowFormAgainAsync(ProfileEditViewModel model)
        {
            var response = await playerService.GetForEditAsync(GetUserId());

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            playerDataBuilder.PlayerEditApplyDisplayData(model, response.ResponseData);

            return View(model);
        }
    }
}
