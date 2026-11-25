using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
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
            if (!ModelState.IsValid)
            {
                return await ReturnInvalidIndexViewAsync(model);
            }

            var searchRequestData = homeDataBuilder.HomeSearchToRequestData(model);

            var pageRequestData = homeDataBuilder.PageInputToPageRequestData(pageInput);

            var serviceResponse = await homeService
                .GetMainResponseModelAsync(searchRequestData, pageRequestData);

            if (serviceResponse.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(serviceResponse.Errors);
                return await ReturnInvalidIndexViewAsync(model);
            }

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = homeDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

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

        private async Task<IActionResult> ReturnInvalidIndexViewAsync(HomeViewModel model)
        {
            var sportOptionsServiceResponse = await homeService.GetSportOptionsAsync();

            if (sportOptionsServiceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = homeDataBuilder.ServiceStatusToErrorViewModel(sportOptionsServiceResponse.Status);
                return View("Error", errorViewModel);
            }

            model.SportOptions = sportOptionsServiceResponse.ResponseData
                .Select(x => homeDataBuilder.HomeSportOptionsToSelectListItem(x, model.SportId));

            return View(model);
        }
    }
}