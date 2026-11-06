using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Common.Helpers;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Venue;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Controllers
{
    public class VenuesController : BaseController
    {
        private readonly IVenueService venueService;
        private readonly WebMapper mapper = new();

        public VenuesController(
            IVenueService venueService,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.venueService = venueService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index([FromQuery] int page = 1)
        {
            page = Math.Max(1, page);

            var venueQueryRequestData = new VenueQueryRequestData
            {
                Page = page,
                PageSize = DefaultPageSize,
            };

            var serviceResponse =
                await venueService.GetVenuesAsync(venueQueryRequestData);

            var venues = serviceResponse
                .ResponseData
                .Items
                .Select(mapper.ToVenueViewModel)
                .Select(x =>
                {
                    x.MatchSlug = SlugHelper.Create(x.MatchTitle);
                    return x;
                })
                .ToList();


            var viewModel = new VenueViewModel
            {
                Venues = venues,
                TotalPages = serviceResponse.ResponseData.TotalPages,
                Page = serviceResponse.ResponseData.Page,
                PageSize = serviceResponse.ResponseData.PageSize,
            };


            return View(viewModel);
        }
    }
}
