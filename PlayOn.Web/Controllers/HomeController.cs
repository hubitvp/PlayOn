using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
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
                [FromQuery] HomeViewModel model,
                [FromQuery] PageInputModel pageInput)
        {
            if (false == ModelState.IsValid)
            {
                var sportOptionsServiceResponse = await homeService
                        .GetSportOptionsAsync();
                model.SportOptions = sportOptionsServiceResponse.ResponseData.Select(x => homeDataBuilder.HomeSportOptionsToSelectListItem(x, model.SportId)); ;
                return View(model);
            }

            var searchRequestData = homeDataBuilder.HomeSearchToRequestData(model);

            var pageRequestData = homeDataBuilder.PageInputToPageRequestData(pageInput);

            var serviceResponse = await homeService
                .GetMainResponseModelAsync(searchRequestData, pageRequestData);

            AddServiceErrors(serviceResponse.Errors);

            var viewModel = homeDataBuilder
                .HomeToViewModel(model, serviceResponse.ResponseData);

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
