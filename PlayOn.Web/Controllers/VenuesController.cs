using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Web.DataBuilders;
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
            Index([FromQuery] int page = 1)
        {
            page = Math.Max(1, page);

            var requestData = venueDataBuilder
                .VenueQueryToRequestData(page);

            var response = await venueService
                .GetVenuesAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                var errorViewModel = venueDataBuilder
                    .VenueListToViewModel(null, page);

                return View(errorViewModel);
            }

            var viewModel = venueDataBuilder
                .VenueListToViewModel(response.ResponseData, page);

            return View(viewModel);
        }
    }
}
