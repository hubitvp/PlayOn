using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Web.ViewModels;
using PlayOn.Web.ViewModels.Home;
using PlayOn.Web.ViewModels.Match;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IHomeService service;

        public HomeController(
            IHomeService service,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.service = service;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] HomeSearchInputModel searchQuery, [FromQuery] int page = 1)
        {
            ModelState.Remove(nameof(page));
            page = Math.Max(1, page);
            if (!ModelState.IsValid)
            {
                return View(await BuildViewModelAsync(searchQuery));
            }

            var serviceResponse = await service.GetMatchCardsAsync(MapToQueryRequest(searchQuery, page));

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrorsToModelState(serviceResponse.Errors);
                return View(await BuildViewModelAsync(searchQuery));
            }

            return View(await BuildViewModelAsync(searchQuery, serviceResponse.ResponseData));
        }

        private static HomeQueryRequestData MapToQueryRequest(HomeSearchInputModel searchQuery, int page)
        {
            return new HomeQueryRequestData
            {
                SportId = searchQuery.SportId,
                City = searchQuery.City,
                From = searchQuery.From != null ? DateOnly.Parse(searchQuery.From) : null,
                Page = page,
                PageSize = DefaultPageSize
            };
        }

        private void AddServiceErrorsToModelState(IEnumerable<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                foreach (var memberName in error.MemberNames)
                {
                    ModelState.AddModelError(
                        MapToViewModelProperty(memberName),
                        error.ErrorMessage ?? "Input data is not correct");
                }
            }
        }

        private static string MapToViewModelProperty(string memberName)
        {
            return memberName switch
            {
                nameof(HomeQueryRequestData.SportId) => nameof(HomeViewModel.SportId),
                nameof(HomeQueryRequestData.City) => nameof(HomeViewModel.City),
                nameof(HomeQueryRequestData.From) => nameof(HomeViewModel.From),
                _ => string.Empty
            };
        }

        private async Task<HomeViewModel> BuildViewModelAsync(
            HomeSearchInputModel searchQuery,
            PagedResult<MatchCardResponseModel>? page = null)
        {
            var model = new HomeViewModel
            {
                SportId = searchQuery.SportId,
                City = searchQuery.City,
                From = searchQuery.From,
                Sports = await GetSportsSelectListAsync(searchQuery.SportId),
                MatchCards = page?.Items.Select(MapToMatchCardViewModel).ToList() ?? [],
                Page = page?.Page ?? FirstPage,
                PageSize = page?.PageSize ?? DefaultPageSize,
                TotalPages = page?.TotalPages ?? 0,
            };

            return model;
        }


        private async Task<List<SelectListItem>>
            GetSportsSelectListAsync(int? selectedSportId)
        {
            var sports = await service.GetSportsAsync();

            return sports
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = selectedSportId.HasValue && x.Id == selectedSportId.Value
                })
                .ToList();
        }

        private static MatchCardViewModel
            MapToMatchCardViewModel(MatchCardResponseModel response)
        {
            return new MatchCardViewModel
            {
                Id = response.Id,
                Slug = SlugHelper.Create(response.Title),
                Title = response.Title,
                SportName = response.SportName,
                SportIcon = response.SportIcon.ToCssClass(),
                VenueName = response.VenueName,
                City = response.City,
                StartsAt = response.Date
                    .ToDateTime(response.StartsAt, DateTimeKind.Utc)
                    .ToString("dddd, dd MMMM  HH:mm", CultureInfo.InvariantCulture),
                DurationMinutes = response.DurationMinutes,
                Joined = response.Joined,
                MaxPlayers = response.MaxPlayers,
                Waitlisted = response.Waitlisted,
                MinSkill = response.MinSkill,
                PricePerPlayer = response.PricePerPlayer.ToFormattedString(),
                Status = response.Status
            };
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
