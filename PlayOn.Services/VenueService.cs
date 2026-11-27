using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Provides paged access to upcoming venues that host open or confirmed matches.
    /// </summary>
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> venueRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenueService"/> class.
        /// </summary>
        /// <param name="venueRepository">The repository used to query venues.</param>
        /// <param name="timeProvider">The time provider used to determine the current local date.</param>
        public VenueService(
            IRepository<Venue> venueRepository,
            TimeProvider timeProvider)
        {
            this.venueRepository = venueRepository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Validates the page request and returns one page of upcoming venues. Validation failures are returned as errors.
        /// </summary>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the page of venues, or the validation errors.</returns>
        public async Task<ServiceResponse<PageResult<VenueResponseModel>>>
            GetSinglePageAsync(PageRequestData pageRequestData)
        {
            ArgumentNullException.ThrowIfNull(pageRequestData);

            var errors = ServiceRequestDataValidator.Validate(pageRequestData);
            if (errors.Any())
            {
                return ServiceResponse<PageResult<VenueResponseModel>>
                    .Failure(errors);
            }

            var venueQuery = BuildVenueQuery();

            var page = await GetPageAsync(
                venueQuery,
                pageRequestData);

            return ServiceResponse<PageResult<VenueResponseModel>>
                .Success(page);
        }

        /// <summary>
        /// Builds the venue query for venues that are not deleted, have an event with a non-deleted open or confirmed match, and are dated today or later.
        /// </summary>
        /// <returns>The filtered venue query.</returns>
        private IQueryable<Venue> BuildVenueQuery()
        {
            var today = DateOnly.FromDateTime(
                timeProvider.GetLocalNow().DateTime);

            return venueRepository
                .Query()
                .Where(x => !x.IsDeleted
                    && x.Event != null
                    && !x.Event.Match.IsDeleted
                    && (x.Event.Match.Status == MatchStatus.Open
                        || x.Event.Match.Status == MatchStatus.Confirmed)
                    && x.Date >= today);
        }

        /// <summary>
        /// Counts the venues, clamps the page size to 1-50 and the page number to the valid range, and loads that page ordered by date, times, name, city and id.
        /// </summary>
        /// <param name="query">The filtered venue query.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>The page of venues with paging metadata.</returns>
        private async Task<PageResult<VenueResponseModel>>
            GetPageAsync(
                IQueryable<Venue> query,
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
                .OrderBy(x => x.Date)
                .ThenBy(x => x.OpensAt)
                .ThenBy(x => x.StartsAt)
                .ThenBy(x => x.ClosesAt)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.City)
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