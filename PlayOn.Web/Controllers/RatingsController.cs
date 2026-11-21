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
            var response = await ratingService.GetFormAsync(matchId, GetUserId());

            if (!response.IsSuccess)
            {
                return FromFailure(response, matchId);
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
                return await ShowFormAgainAsync(model);
            }

            var requestData = ratingDataBuilder
                .RatingFormToRequestData(GetUserId(), model);

            var response = await ratingService.SubmitAsync(requestData);

            if (response.IsSuccess)
            {
                TempData["Success"] = "Thank you, your ratings were saved.";

                return RedirectToAction("Details", "Matches", new { id = model.MatchId });
            }

            if (response.Status != ServiceStatus.Invalid)
            {
                return FromFailure(response, model.MatchId);
            }

            AddServiceErrors(response.Errors);

            return await ShowFormAgainAsync(model);
        }


        private async Task<IActionResult> ShowFormAgainAsync(RatingFormViewModel model)
        {
            var response = await ratingService.GetFormAsync(model.MatchId, GetUserId());

            if (!response.IsSuccess)
            {
                return FromFailure(response, model.MatchId);
            }

            ratingDataBuilder.RatingFormApplyDisplayData(model, response.ResponseData);

            return View(model);
        }

        private IActionResult FromFailure<T>(ServiceResponse<T> response, Guid matchId)
        {
            switch (response.Status)
            {
                case ServiceStatus.NotFound:
                    return NotFound();

                case ServiceStatus.Forbidden:
                    return Forbid();

                default:
                    TempData["Warning"] = "Error: The action could not be completed due to a conflict.";

                    return RedirectToAction("Details", "Matches", new { id = matchId });
            }
        }
    }
}
