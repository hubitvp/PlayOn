using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;

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
                MyMatchesTab tab = MyMatchesTab.Upcoming,
                int page = 1)
        {
            page = Math.Max(1, page);

            if (!Enum.IsDefined(tab))
            {
                tab = MyMatchesTab.Upcoming;
            }

            var requestData = participationDataBuilder
                .MyMatchesToRequestData(GetUserId(), tab, page);

            var response = await participationService.GetMineAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(participationDataBuilder.MyMatchesToViewModel(tab, null, page));
            }

            var viewModel = participationDataBuilder
                .MyMatchesToViewModel(tab, response.ResponseData, page);

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

            if (response.IsSuccess)
            {
                TempData["Success"] = participationDataBuilder
                    .JoinToMessage(response.ResponseData);
            }
            else if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else
            {
                TempData["Error"] = "Error: The action could not be completed due to a conflict.";
            }

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

            if (response.IsSuccess)
            {
                TempData["Success"] = participationDataBuilder
                    .LeaveToMessage(response.ResponseData);
            }
            else if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else
            {
                TempData["Error"] = "Error: The action could not be completed due to a conflict.";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Details", "Matches", new { id, slug });
        }
    }
}
