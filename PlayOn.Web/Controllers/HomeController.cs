using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Home;
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
        public async Task<IActionResult>
            Index(
                [FromQuery] HomeSearchInputModel searchQuery,
                [FromQuery] int page = 1)
        {
            ModelState.Remove(nameof(page));
            page = Math.Max(1, page);
            if (!ModelState.IsValid)
            {
                var errorViewModel = await BuildViewModelAsync(searchQuery);

                return View(errorViewModel);
            }

            var serviceResponse = await service.GetMatchCardsAsync(HomeMapper.ToHomeQuery(searchQuery, page));

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrors(serviceResponse.Errors);

                var errorViewModel = await BuildViewModelAsync(searchQuery);

                return View(errorViewModel);
            }

            var viewModel =
                await BuildViewModelAsync(
                    searchQuery,
                    serviceResponse.ResponseData);

            return View(viewModel);
        }

        private async Task<HomeViewModel>
            BuildViewModelAsync(
                HomeSearchInputModel searchQuery,
                PagedResult<MatchCardResponseModel>? page = null)
        {
            var model = new HomeViewModel
            {
                SportId = searchQuery.SportId,
                City = searchQuery.City,
                From = searchQuery.From,
                Sports = await GetSportsSelectListAsync(searchQuery.SportId),
                MatchCards = page?.Items
                    .Select(x => HomeMapper.ToMatchCardViewModel(x))
                    .Select(x =>
                    {
                        x.Slug = SlugHelper.Create(x.Title);
                        return x;
                    })
                    .ToList() ?? [],
                Page = page?.Page ?? FirstPage,
                PageSize = page?.PageSize ?? DefaultPageSize,
                TotalPages = page?.TotalPages ?? 0,
                TotalItems = page?.TotalCount ?? 0,
            };

            return model;
        }


        private async Task<List<SelectListItem>>
            GetSportsSelectListAsync(int? selectedSportId)
        {
            var sports = await service.GetSportsAsync();

            return sports.ResponseData
                .Select(x =>
                    HomeMapper.ToSportSelectListItem(x, selectedSportId))
                .ToList();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
