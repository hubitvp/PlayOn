using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Services.DataBuilders;

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

        public async Task<ServiceResponse<PageResult<VenueResponseModel>>>
            GetSinglePageAsync(PageRequestData pageRequestData)
        {
            var venueQuery = BuildVenueQuery();

            var page = await GetSinglePageAsync(
                venueQuery,
                pageRequestData);

            return ServiceResponse<PageResult<VenueResponseModel>>
                .Success(page);
        }

        private IOrderedQueryable<Venue> BuildVenueQuery()
        {
            var today = DateOnly.FromDateTime(
                timeProvider.GetLocalNow().DateTime);

            return venueRepository
                .Query()
                .Where(x => !x.IsDeleted)
                .Where(x => x.Event != null)
                .Where(x =>
                    x.Event!.Match.Status == MatchStatus.Open
                    || x.Event!.Match.Status == MatchStatus.Confirmed)
                .Where(x => x.Date >= today)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.OpensAt)
                .ThenBy(x => x.StartsAt)
                .ThenBy(x => x.ClosesAt)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.City);
        }

        private async Task<PageResult<VenueResponseModel>>
            GetSinglePageAsync(
                IOrderedQueryable<Venue> query,
                PageRequestData pageRequestData)
        {
            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(
                pageRequestData.PageSize, 1, 50);

            var totalPages = Math.Max(
                1,
                (int)Math.Ceiling(totalCount / (double)pageSize));

            var pageNumber = Math.Clamp(
                pageRequestData.Page, 1, totalPages);

            var items = await query
                .ThenBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(VenueDataBuilder.VenueProjection())
                .ToListAsync();

            return new PageResult<VenueResponseModel>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }
    }
}
