using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels.Ratings;

namespace PlayOn.Web.Controllers
{
    public class RatingsController : BaseController
    {
        private readonly IRatingService ratingService;
        private readonly RatingDataBuilder ratingDataBuilder;

        public RatingsController(
            IRatingService ratingService,
            RatingDataBuilder ratingDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.ratingService = ratingService;
            this.ratingDataBuilder = ratingDataBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid matchId)
        {
            if (matchId == Guid.Empty)
            {
                var emptyIdError = ratingDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var response = await ratingService.GetFormAsync(matchId, GetUserId());

            if (!response.IsSuccess)
            {
                var errorViewModel = ratingDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = ratingDataBuilder.RatingFormToViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RatingFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await ReturnInvalidCreateViewAsync(model);
            }

            var requestData = ratingDataBuilder
                .RatingFormToRequestData(GetUserId(), model);

            var response = await ratingService.SubmitAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return await ReturnInvalidCreateViewAsync(model);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = ratingDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "Thank you, your ratings were saved.";

            return RedirectToAction("Details", "Matches", new { id = model.MatchId });
        }

        private async Task<IActionResult> ReturnInvalidCreateViewAsync(RatingFormViewModel model)
        {
            if (model.MatchId == Guid.Empty)
            {
                var emptyIdError = ratingDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var response = await ratingService.GetFormAsync(model.MatchId, GetUserId());

            if (!response.IsSuccess)
            {
                var errorViewModel = ratingDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            ratingDataBuilder.RatingFormApplyDisplayData(model, response.ResponseData);

            return View(nameof(Create), model);
        }
    }
}