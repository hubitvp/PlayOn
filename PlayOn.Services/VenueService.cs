using PlayOn.Common.Helpers;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Services.Repositories.Contracts;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Mappers;

namespace PlayOn.Services
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository venueRepository;
        private readonly ServiceMapper mapper = new();
        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public async Task<ServiceResponse<PagedResult<VenueResponseModel>>>
            GetVenuesAsync(VenueQueryRequestData venueQuery)
        {

            var venueQueryDto = mapper.ToVenueQuery(venueQuery);


            var page = await venueRepository.GetVenuesAsync(venueQueryDto);


            var response = page.Map(mapper.ToVenueResponse);


            return ServiceResponse<PagedResult<VenueResponseModel>>
                .Success(response);
        }
    }
}
