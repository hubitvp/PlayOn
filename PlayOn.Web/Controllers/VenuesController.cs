using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels;

namespace PlayOn.Web.Controllers
{
    [AllowAnonymous]
    public class VenuesController : BaseController
    {
        private readonly IVenueService venueService;
        private readonly VenueDataBuilder venueDataBuilder;

        public VenuesController(
            IVenueService venueService,
            VenueDataBuilder venueDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.venueService = venueService;
            this.venueDataBuilder = venueDataBuilder;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index([FromQuery] PageInputModel page)
        {
            if (!ModelState.IsValid)
            {
                return await ReturnInvalidIndexViewAsync(page);
            }

            var requestData = venueDataBuilder.PageInputToPageRequestData(page);

            var response = await venueService.GetSinglePageAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return await ReturnInvalidIndexViewAsync(page);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = venueDataBuilder
                .VenueListToViewModel(response.ResponseData, requestData.Page);

            return View(viewModel);
        }

        private async Task<IActionResult> ReturnInvalidIndexViewAsync(PageInputModel page)
        {
            var requestData = venueDataBuilder.PageInputToPageRequestData(page);

            var response = await venueService.GetSinglePageAsync(requestData);

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = venueDataBuilder
                .VenueListToViewModel(response.ResponseData, requestData.Page);

            return View(nameof(Index), viewModel);
        }
    }
}