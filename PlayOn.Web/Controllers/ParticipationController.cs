using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels;

namespace PlayOn.Web.Controllers
{
    public class ParticipationController : BaseController
    {
        private readonly IParticipationService participationService;
        private readonly ParticipationDataBuilder participationDataBuilder;

        public ParticipationController(
            IParticipationService participationService,
            ParticipationDataBuilder participationDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.participationService = participationService;
            this.participationDataBuilder = participationDataBuilder;
        }

        [HttpGet]
        public async Task<IActionResult>
            Mine(
                [FromQuery] PageInputModel page,
                MyMatchesTab tab = MyMatchesTab.Upcoming)
        {
            if (!Enum.IsDefined(tab))
            {
                tab = MyMatchesTab.Upcoming;
            }

            if (!ModelState.IsValid)
            {
                return await ReturnInvalidMineViewAsync(page, tab);
            }

            var matchesRequestData = participationDataBuilder
                .MyMatchesToRequestData(GetUserId(), tab);

            var pageRequestData = participationDataBuilder
                .PageInputToPageRequestData(page);

            var response = await participationService
                .GetSinglePageAsync(matchesRequestData, pageRequestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return await ReturnInvalidMineViewAsync(page, tab);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = participationDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = participationDataBuilder
                .MyMatchesToViewModel(tab, response.ResponseData, pageRequestData.Page);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Join(
                Guid id,
                string? slug)
        {
            var requestData = participationDataBuilder
                .ParticipationToRequestData(id, GetUserId());

            var response = await participationService.JoinAsync(requestData);

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = participationDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = participationDataBuilder
                .JoinToMessage(response.ResponseData);

            return RedirectToAction("Details", "Matches", new { id, slug });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Leave(
                Guid id,
                string? slug,
                string? returnUrl)
        {
            var requestData = participationDataBuilder
                .ParticipationToRequestData(id, GetUserId());

            var response = await participationService.LeaveAsync(requestData);

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = participationDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = participationDataBuilder
                .LeaveToMessage(response.ResponseData);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Details", "Matches", new { id, slug });
        }

        private async Task<IActionResult> ReturnInvalidMineViewAsync(
            PageInputModel page,
            MyMatchesTab tab)
        {
            var matchesRequestData = participationDataBuilder
                .MyMatchesToRequestData(GetUserId(), tab);

            var pageRequestData = participationDataBuilder
                .PageInputToPageRequestData(page);

            var response = await participationService
                .GetSinglePageAsync(matchesRequestData, pageRequestData);

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = participationDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = participationDataBuilder
                .MyMatchesToViewModel(tab, response.ResponseData, pageRequestData.Page);

            return View(nameof(Mine), viewModel);
        }
    }
}