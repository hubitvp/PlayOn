using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Home;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IHomeService service;
        private readonly WebMapper mapper = new();


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

            var serviceResponse = await service.GetMatchCardsAsync(mapper.ToHomeQuery(searchQuery, page));

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrorsToModelState(serviceResponse.Errors);
                return View(await BuildViewModelAsync(searchQuery));
            }

            return View(await BuildViewModelAsync(searchQuery, serviceResponse.ResponseData));
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
                MatchCards = page?.Items
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
            };

            return model;
        }


        private async Task<List<SelectListItem>>
            GetSportsSelectListAsync(int? selectedSportId)
        {
            var sports = await service.GetSportsAsync();

            return sports.ResponseData
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = selectedSportId.HasValue && x.Id == selectedSportId.Value
                })
                .ToList();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
