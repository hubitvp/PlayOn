using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Match;

namespace PlayOn.Web.Controllers
{
    [AllowAnonymous]
    public class MatchesController : BaseController
    {
        private readonly IMatchService matchService;

        private readonly ISportService sportService;

        public MatchesController(
            IMatchService matchService,
            ISportService sportService,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.matchService = matchService;
            this.sportService = sportService;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index(
                [FromQuery] MatchListInputModel input)
        {
            if (!ModelState.IsValid)
            {
                var errorViewModel =
                    await BuildListViewModelAsync(input, null);

                return View(errorViewModel);
            }

            var request = MatchesMapper.ToMatchListRequestData(input);

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

            var data = response.ResponseData;
            var canonicalSlug = SlugHelper.Create(data.Title);

            if ((slug ?? string.Empty) != canonicalSlug)
            {
                return RedirectToAction(
                    nameof(Details),
                    new { id, slug = canonicalSlug });
            }

            var viewModel = MatchesMapper
                .ToMatchDetailsViewModel(data, canonicalSlug);

            return View(viewModel);
        }


        private async Task<MatchListViewModel>
            BuildListViewModelAsync(
                MatchListInputModel input,
                PagedResult<MatchCardResponseModel>? page)
        {
            var sports = (await sportService.GetSportsAsync()).ResponseData;

            var response = MatchesMapper.ToMatchListViewModel(input, page, sports);
            return response;
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

                        var result = Conflict(problemDetails);

                        return result;
                    }
                default:
                    {
                        return BadRequest();
                    }
            }
        }
    }
}
