using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels;
using PlayOn.Web.ViewModels.Home;

namespace PlayOn.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly HomeDataBuilder homeDataBuilder;

        public HomeController(
            IHomeService homeService,
            HomeDataBuilder homeDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.homeService = homeService;

            this.homeDataBuilder = homeDataBuilder;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index(
                [FromQuery] HomeSearchInputModel searchQuery,
                [FromQuery] PageInputModel pageInput)
        {

            if (false == ModelState.IsValid)
            {
                var errorServiceResponse = await homeService
                        .GetSportOptionsAsync();

                var errorHomeViewModel = homeDataBuilder
                    .HomeToViewModel(searchQuery, errorServiceResponse.ResponseData);

                return View(errorHomeViewModel);
            }

            var searchRequestData = homeDataBuilder.HomeSearchToRequestData(searchQuery);

            var pageRequestData = homeDataBuilder.PageInputToPageRequestData(pageInput);

            var serviceResponse = await homeService
                .GetHomeAsync(searchRequestData, pageRequestData);

            AddServiceErrors(serviceResponse.Errors);

            var viewModel = homeDataBuilder
                .HomeToViewModel(searchQuery, serviceResponse.ResponseData);

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
