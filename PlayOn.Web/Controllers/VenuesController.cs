using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
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

            var requestData = venueDataBuilder.PageInputToPageRequestData(page);

            var response = await venueService
                .GetSinglePageAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                var errorViewModel = venueDataBuilder
                    .VenueListToViewModel(null, requestData.Page);

                return View(errorViewModel);
            }

            var viewModel = venueDataBuilder
                .VenueListToViewModel(response.ResponseData, requestData.Page);

            return View(viewModel);
        }
    }
}
