using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Venue;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Controllers
{
    [AllowAnonymous]
    public class VenuesController : BaseController
    {
        private readonly IVenueService venueService;

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

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrors(serviceResponse.Errors);
                return View(new VenueViewModel
                {
                    Venues = [],
                    TotalPages = 0,
                    Page = page,
                    PageSize = DefaultPageSize
                });
            }

            var venues = serviceResponse
                .ResponseData
                .Items
                .Select(x => VenuesMapper.ToVenueViewModel(x))
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
