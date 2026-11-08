using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Matches.ResponseModels;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Match;
using System.Globalization;

namespace PlayOn.Web.Controllers
{
    [AllowAnonymous]
    public class MatchesController : BaseController
    {
        private readonly IMatchService matchService;

        private readonly ISportService sportService;
        private readonly WebMapper mapper = new();

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
        public async Task<IActionResult> Index([FromQuery] MatchListInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(await BuildListViewModelAsync(input, null));
            }

            var request = mapper.ToMatchListRequestData(input);

            var response = await matchService
                .GetMatchCardsAsync(request);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);
                return View(await BuildListViewModelAsync(input, null));
            }

            return View(await BuildListViewModelAsync(input, response.ResponseData));
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id, string? slug)
        {
            var response = await matchService.GetDetailsAsync(
                id, IsUserAuthenticated() ? GetUserId() : null, IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var data = response.ResponseData;
            var canonicalSlug = SlugHelper.Create(data.Title);

            if ((slug ?? string.Empty) != canonicalSlug)
            {
                return RedirectToAction(nameof(Details), new { id, slug = canonicalSlug });
            }

            return View(mapper.ToMatchDetailsViewModel(data, canonicalSlug));
        }


        private async Task<MatchListViewModel> BuildListViewModelAsync(
            MatchListInputModel input,
            PagedResult<MatchCardResponseModel>? page)
        {
            var sports = (await sportService.GetSportsAsync()).ResponseData;

            var response = mapper.ToMatchListViewModel(input, page, sports);
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

        private static MatchDetailsViewModel ToDetailsViewModel(MatchDetailsResponseModel data, string slug)
        {
            var start = data.Date.ToDateTime(data.StartsAt);

            return new MatchDetailsViewModel
            {
                Id = data.Id,
                Slug = slug,
                Title = data.Title,
                Description = data.Description,
                Status = data.Status,
                CancelReason = data.CancelReason,
                IsHidden = data.IsHidden,
                SportName = data.SportName,
                SportIcon = data.SportIcon.ToCssClass(),
                StartsAt = start.ToString("dddd, dd MMMM yyyy, HH:mm", CultureInfo.InvariantCulture),
                EndsAt = start.AddMinutes(data.DurationMinutes).ToString("HH:mm", CultureInfo.InvariantCulture),
                DurationMinutes = data.DurationMinutes,
                VenueName = data.VenueName,
                Address = data.Address,
                City = data.City,
                CourtName = data.CourtName,
                Surface = data.Surface,
                MinPlayers = data.MinPlayers,
                MaxPlayers = data.MaxPlayers,
                Joined = data.Joined,
                Waitlisted = data.Waitlisted,
                MinSkill = data.MinSkill,
                MaxSkill = data.MaxSkill,
                PricePerPlayer = data.PricePerPlayer.ToFormattedString(),
                OrganiserName = data.OrganiserName,
                OrganiserReliability = data.OrganiserReliability,
                CanManage = data.CanManage,
                Participants = data.Participants
                    .Select(x => new MatchParticipantViewModel
                    {
                        DisplayName = x.DisplayName,
                        City = x.City,
                        Status = x.Status,
                        IsOrganiser = x.IsOrganiser,
                        Reliability = x.Reliability
                    })
                    .ToList()
            };
        }

        private IActionResult FromFailure<T>(ServiceResponse<T> response, int matchId)
        {
            switch (response.Status)
            {
                case ServiceStatus.NotFound:
                    return NotFound();

                case ServiceStatus.Forbidden:
                    return Forbid();

                case ServiceStatus.Conflict:
                    TempData["Error"] = response.Errors.FirstOrDefault()?.ErrorMessage
                        ?? "This action is not possible right now.";
                    return RedirectToAction(nameof(Details), new { id = matchId });

                default:
                    return BadRequest();
            }
        }
    }
}
