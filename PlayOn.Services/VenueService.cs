using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Services.Mappers;

namespace PlayOn.Services
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> venueRepository;

        private readonly TimeProvider timeProvider;

        public VenueService(
            IRepository<Venue> venueRepository,
            TimeProvider timeProvider)
        {
            this.venueRepository = venueRepository;
            this.timeProvider = timeProvider;
        }

        public async Task<ServiceResponse<PagedResult<VenueResponseModel>>>
            GetVenuesAsync(VenueQueryRequestData venueQuery)
        {


            IQueryable<Venue> venueQuerySource = venueRepository
                .Query()
                .AsSplitQuery();


            venueQuerySource = venueQuerySource
                .Where(x => !x.IsDeleted)
                .Where(x => x.Event != null)
                .Where(x =>
                    x.Event!.Match.Status == MatchStatus.Open
                    || x.Event!.Match.Status == MatchStatus.Confirmed)
                .Where(x =>
                    x.Date >= DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime));


            var totalCount = await venueQuerySource.CountAsync();

            var pageSize = Math.Clamp(venueQuery.PageSize, 1, 50);

            var totalPages = Math.Max(
                1,
                (int)Math.Ceiling(totalCount / (double)pageSize));

            var pageNumber = Math.Clamp(
                venueQuery.Page,
                1,
                totalPages);


            venueQuerySource = venueQuerySource
                .OrderBy(x => x.Date)
                .ThenBy(x => x.OpensAt)
                .ThenBy(x => x.StartsAt)
                .ThenBy(x => x.ClosesAt)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.City)
                .ThenBy(x => x.Id);

            var items = await venueQuerySource
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(VenueMapper.VenueProjection())
                .ToListAsync();

            var page = new PagedResult<VenueResponseModel>(
                items,
                pageNumber,
                pageSize,
                totalCount);

            return ServiceResponse<PagedResult<VenueResponseModel>>
                .Success(page);
        }
    }
}
