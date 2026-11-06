using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Web.ViewModels.Venue;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Controllers
{
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

            var venues = serviceResponse
                .ResponseData
                .Items
                .Select(x => new VenueViewModelData
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Date = x.Date,
                    City = x.City,
                    StartsAt = x.StartsAt,
                    OpensAt = x.OpensAt,
                    ClosesAt = x.ClosesAt,
                    Court = new CourtViewModel
                    {
                        Name = x.Court.Name,
                        SportIconCss = x.Court.SportIcon.ToCssClass(),
                        Surface = x.Court.Surface.ToString(),
                        Capacity = x.Court.Capacity,
                        HourlyPrice = x.Court.HourlyPrice.ToFormattedString(),
                    },
                    MatchId = x.MatchId,
                    MatchTitle = x.MatchTitle,
                    MatchSlug = SlugHelper.Create(x.MatchTitle),
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
