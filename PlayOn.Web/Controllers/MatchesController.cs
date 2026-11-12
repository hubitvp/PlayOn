using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Matches.ResponseModels;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels.Match;

namespace PlayOn.Web.Controllers
{
    public class MatchesController : BaseController
    {
        private readonly IMatchService matchService;
        private readonly ISportService sportService;
        private readonly MatchDataBuilder matchDataBuilder;

        public MatchesController(
            IMatchService matchService,
            ISportService sportService,
            MatchDataBuilder matchDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.matchService = matchService;
            this.sportService = sportService;
            this.matchDataBuilder = matchDataBuilder;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index([FromQuery] MatchSearchInputModel input)
        {
            if (!ModelState.IsValid)
            {
                var errorViewModel =
                    await BuildListViewModelAsync(input, null);

                return View(errorViewModel);
            }

            var request = matchDataBuilder
                .MatchSearchToRequestData(input);

            var response = await matchService
                .GetMatchCardsAsync(request);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                var errorViewModel =
                    await BuildListViewModelAsync(input, null);

                return View(errorViewModel);
            }

            var viewModel =
                await BuildListViewModelAsync(input, response.ResponseData);

            return View(viewModel);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Details(
                int id,
                string? slug)
        {
            var response = await matchService
                .GetDetailsAsync(
                    id,
                    IsUserAuthenticated() ? GetUserId() : null,
                    IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var canonicalSlug = Slug.Create(response.ResponseData.Title);

            if ((slug ?? string.Empty) != canonicalSlug)
            {
                return RedirectToAction(
                    nameof(Details),
                    new { id, slug = canonicalSlug });
            }

            var viewModel = matchDataBuilder
                .MatchDetailsToViewModel(response.ResponseData, canonicalSlug);

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await matchService
                .GetForEditAsync(id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var viewModel = matchDataBuilder
                .MatchEditToViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult>
            Edit(
                int id,
                MatchEditViewModel model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                return await EditViewWithDisplayDataAsync(model);
            }

            var requestData = matchDataBuilder
                .MatchEditToRequestData(id, GetUserId(), IsAdmin(), model);

            var response = await matchService.UpdateAsync(requestData);

            if (response.IsSuccess)
            {
                TempData["Success"] = "Match updated.";

                return RedirectToAction(
                    nameof(Details),
                    new { id, slug = Slug.Create(model.Title) });
            }

            if (response.Status != ServiceStatus.Invalid)
            {
                return FromFailure(response);
            }

            AddServiceErrors(response.Errors);

            return await EditViewWithDisplayDataAsync(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create(int? venueId)
        {
            var options = await matchService.GetFormOptionsAsync();

            var viewModel = matchDataBuilder
                .MatchFormToViewModel(venueId, options);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await CreateViewWithOptionsAsync(model);
            }

            var requestData = matchDataBuilder
                .MatchFormToRequestData(GetUserId(), model);

            var response = await matchService.CreateAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return await CreateViewWithOptionsAsync(model);
            }

            TempData["Success"] = "Your match is open. Other players can join now.";

            return RedirectToAction(
                nameof(Details),
                new { id = response.ResponseData, slug = Slug.Create(model.Title) });
        }


        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            var response = await matchService
                .GetForCancelAsync(id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var viewModel = matchDataBuilder
                .MatchCancelToViewModel(id, response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        
        public async Task<IActionResult>
            Cancel(
                int id,
                MatchCancelViewModel model)
        {
            model.Id = id;

            if (ModelState.IsValid)
            {
                var requestData = matchDataBuilder
                    .MatchCancelToRequestData(id, GetUserId(), IsAdmin(), model);

                var response = await matchService.CancelAsync(requestData);

                if (response.IsSuccess)
                {
                    TempData["Success"] = "The match was cancelled and the players were notified.";

                    return RedirectToAction(nameof(Details), new { id });
                }

                if (response.Status != ServiceStatus.Invalid)
                {
                    return FromFailure(response);
                }

                AddServiceErrors(response.Errors);
            }

            var cancelData = await matchService
                .GetForCancelAsync(id, GetUserId(), IsAdmin());

            if (!cancelData.IsSuccess)
            {
                return FromFailure(cancelData);
            }

            matchDataBuilder.MatchCancelApplyDisplayData(model, cancelData.ResponseData);

            return View(model);
        }


        private async Task<MatchListViewModel>
            BuildListViewModelAsync(
                MatchSearchInputModel input,
                PageResult<MatchCardResponseModel>? page)
        {
            var sports = (await sportService.GetSportsAsync()).ResponseData;

            return matchDataBuilder.MatchListToViewModel(input, page, sports);
        }

        private async Task<IActionResult>
            CreateViewWithOptionsAsync(MatchFormViewModel model)
        {
            var options = await matchService.GetFormOptionsAsync();

            matchDataBuilder.MatchFormOptionsToViewModel(model, options);

            return View(model);
        }

        private async Task<IActionResult>
            EditViewWithDisplayDataAsync(MatchEditViewModel model)
        {
            var response = await matchService
                .GetForEditAsync(model.Id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            matchDataBuilder.MatchEditApplyDisplayData(model, response.ResponseData);

            return View(model);
        }

        private IActionResult FromFailure<T>(ServiceResponse<T> response)
        {
            switch (response.Status)
            {
                case ServiceStatus.NotFound:
                    {
                        return NotFound();
                    }
                case ServiceStatus.Forbidden:
                    {
                        return Forbid();
                    }
                case ServiceStatus.Conflict:
                    {
                        var message = response.Errors
                                .FirstOrDefault()?
                                .ErrorMessage
                                ?? "This action is not possible right now.";

                        var problemDetails = new ProblemDetails
                        {
                            Title = "Operation conflict",
                            Detail = message,
                            Status = StatusCodes.Status409Conflict,
                            Instance = HttpContext.Request.Path
                        };

                        return Conflict(problemDetails);
                    }
                default:
                    {
                        return BadRequest();
                    }
            }
        }
    }
}
