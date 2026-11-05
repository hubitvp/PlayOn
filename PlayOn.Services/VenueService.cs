using PlayOn.Common.Helpers;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Services.Repositories.Contracts;
using PlayOn.Services.Repositories.Data.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Services
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository venueRepository;
        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public async Task<ServiceResponse<PagedResult<VenueResponseModel>>>
            GetVenuesAsync(VenueQueryRequestData venueQuery)
        {

            var venueQueryDto = new VenueQueryDto
            {
                Page = venueQuery.Page,
                PageSize = venueQuery.PageSize,
            };


            var page = await venueRepository.GetVenuesAsync(venueQueryDto);


            var response = page.Map(x => new VenueResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                City = x.City,
                Date = x.Date,
                StartsAt = x.StartsAt,
                OpensAt = x.OpensAt,
                ClosesAt = x.ClosesAt,
                Court = new CourtResponseModel
                {
                    Name = x.Court.Name,
                    SportIcon = x.Court.SportIcon,
                    Surface = x.Court.Surface,
                    Capacity = x.Court.Capacity,
                    HourlyPrice = x.Court.HourlyPrice,
                },
                MatchId = x.MatchId,
                MatchTitle = x.MatchTitle,
            });


            return ServiceResponse<PagedResult<VenueResponseModel>>
                .Success(response);
        }
    }
}
