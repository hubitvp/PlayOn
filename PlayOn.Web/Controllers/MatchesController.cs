using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Matches.RequestData;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Match;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

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

            var response = await matchService.GetMatchCardsAsync(new MatchListRequestData
            {
                SearchTerm = input.SearchTerm,
                SportId = input.SportId,
                City = input.City,
                From =
                    DateOnly.TryParse(input.From, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                    ? date
                    : null,
                Skill = input.Skill,
                OnlyWithFreeSpots = input.OnlyWithFreeSpots,
                Sort = input.Sort,
                Page = Math.Max(FirstPage, input.Page),
                PageSize = DefaultPageSize
            });

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);
                return View(await BuildListViewModelAsync(input, null));
            }

            return View(await BuildListViewModelAsync(input, response.ResponseData));
        }



        private async Task<MatchListViewModel> BuildListViewModelAsync(
            MatchListInputModel input,
            PlayOn.Common.Helpers.PagedResult<PlayOn.Services.Data.Home.ResponseModels.MatchCardResponseModel>? page)
        {
            var sports = (await sportService.GetSportsAsync()).ResponseData;

            return new MatchListViewModel
            {
                SearchTerm = input.SearchTerm,
                SportId = input.SportId,
                City = input.City,
                From = input.From,
                Skill = input.Skill,
                OnlyWithFreeSpots = input.OnlyWithFreeSpots,
                Sort = input.Sort,
                Sports = sports
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                        Selected = x.Id == input.SportId
                    })
                    .ToList(),
                Matches = page?.Items
                    .Select(mapper.ToMatchCardViewModel)
                    .Select(x =>
                    {
                        x.Slug = SlugHelper.Create(x.Title);
                        return x;
                    })
                    .ToList() ?? [],
                Page = page?.Page ?? FirstPage,
                PageSize = page?.PageSize ?? DefaultPageSize,
                TotalPages = page?.TotalPages ?? 0,
                TotalItems = page?.TotalCount ?? 0
            };
        }
    }
}
