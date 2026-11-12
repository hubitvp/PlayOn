using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels.Home;

namespace PlayOn.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly ISportService sportService;
        private readonly IHomeService homeService;
        private readonly HomeDataBuilder homeDataBuilder;

        public HomeController(
            ISportService sportService,
            IHomeService homeService,
            HomeDataBuilder homeDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.sportService = sportService;
            this.homeService = homeService;

            this.homeDataBuilder = homeDataBuilder;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index(
                [FromQuery] HomeSearchInputModel searchQuery,
                [FromQuery] int page = 1)
        {
            page = Math.Max(1, page);

            var sports = (await sportService.GetSportsAsync()).ResponseData;

            ModelState.Remove(nameof(page));
            if (!ModelState.IsValid)
            {
                var errorHomeViewModel = homeDataBuilder
                    .HomeToViewModel(searchQuery, null, sports);
                return View(errorHomeViewModel);
            }

            var searchRequestData = homeDataBuilder
                .HomeSearchToRequestData(searchQuery, page);

            var response = await homeService
                .GetMatchCardsAsync(searchRequestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);
                var errorHomeViewModel = homeDataBuilder
                    .HomeToViewModel(searchQuery, null, sports);
                return View(errorHomeViewModel);
            }

            var viewModel = homeDataBuilder
                .HomeToViewModel(searchQuery, response.ResponseData, sports);

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
