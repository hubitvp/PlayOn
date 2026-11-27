using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.DataBuilders;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services
{
    /// <summary>
    /// Provides the home page data: upcoming open or confirmed matches with filtering and paging, plus the sport options.
    /// </summary>
    public class HomeService : IHomeService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeService"/> class.
        /// </summary>
        /// <param name="matchRepository">The repository used to query matches.</param>
        /// <param name="sportRepository">The repository used to query sports.</param>
        /// <param name="timeProvider">The time provider used to determine the current local date.</param>
        public HomeService(
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Builds the full home page model containing one page of match cards and the list of sport options. Fails with the underlying errors if either the page or the sport options cannot be loaded.
        /// </summary>
        /// <param name="searchRequestData">The search filters (sport, city, start date).</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the home model on success, or the validation errors of the failed step.</returns>
        public async Task<ServiceResponse<HomeResponseModel>> GetMainResponseModelAsync(
            HomeSearchRequestData searchRequestData,
            PageRequestData pageRequestData)
        {
            var pageResponse = await GetSinglePageAsync(
                searchRequestData,
                pageRequestData);

            if (!pageResponse.IsSuccess)
            {
                return ServiceResponse<HomeResponseModel>
                    .Failure(pageResponse.Errors);
            }

            var sportsResponse = await GetSportOptionsAsync();

            if (!sportsResponse.IsSuccess)
            {
                return ServiceResponse<HomeResponseModel>
                    .Failure(sportsResponse.Errors);
            }

            var responseData = new HomeResponseModel
            {
                SportOptions = sportsResponse.ResponseData,
                Page = pageResponse.ResponseData
            };

            return ServiceResponse<HomeResponseModel>
                .Success(responseData);
        }

        /// <summary>
        /// Validates the search filters, checks that any selected sport exists, and returns one page of matching match cards. Validation failures are returned as errors instead of throwing.
        /// </summary>
        /// <param name="searchRequestData">The search filters (sport, city, start date).</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the page of match cards, or the validation errors.</returns>
        public async Task<ServiceResponse<PageResult<HomeMatchCardResponseModel>>>
            GetSinglePageAsync(
                HomeSearchRequestData searchRequestData,
                PageRequestData pageRequestData)
        {
            var validationErrors =
                ServiceRequestDataValidator.Validate(searchRequestData);

            if (validationErrors.Any())
            {
                return ServiceResponse<PageResult<HomeMatchCardResponseModel>>
                    .Failure(validationErrors);
            }

            var sportValidation =
                await ValidateSportAsync(searchRequestData.SportId);

            if (sportValidation.Any())
            {
                return ServiceResponse<PageResult<HomeMatchCardResponseModel>>
                    .Failure(sportValidation);
            }

            var matchQuery = BuildMatchQuery(searchRequestData);

            var page = await GetPageAsync(
                matchQuery,
                pageRequestData);

            return ServiceResponse<PageResult<HomeMatchCardResponseModel>>
                .Success(page);
        }

        /// <summary>
        /// Retrieves all sports ordered by name for use as filter options.
        /// </summary>
        /// <returns>A successful response with the sport options ordered by name.</returns>
        public async Task<ServiceResponse<IReadOnlyList<HomeSportOptionsResponseModel>>>
            GetSportOptionsAsync()
        {
            var sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(HomeDataBuilder.HomeSportOptionsProjection())
                .ToListAsync();

            return ServiceResponse<IReadOnlyList<HomeSportOptionsResponseModel>>
                .Success(sports);
        }

        /// <summary>
        /// Checks that the given sport identifier, when provided, refers to an existing sport. A missing identifier is considered valid.
        /// </summary>
        /// <param name="sportId">The optional sport identifier to validate.</param>
        /// <returns>An empty collection when valid; otherwise a validation result for the sport field.</returns>
        private async Task<IEnumerable<ValidationResult>> ValidateSportAsync(
            Guid? sportId)
        {
            if (!sportId.HasValue)
            {
                return Enumerable.Empty<ValidationResult>();
            }

            var sportExists = await sportRepository
                .Query()
                .AnyAsync(x => x.Id == sportId.Value);

            if (sportExists)
            {
                return Enumerable.Empty<ValidationResult>();
            }

            return new[]
            {
                new ValidationResult(
                    "Select a valid sport.",
                    new[] { nameof(HomeSearchRequestData.SportId) })
            };
        }

        /// <summary>
        /// Builds the match query by applying the base, sport, city and date filters in order.
        /// </summary>
        /// <param name="query">The search filters to apply.</param>
        /// <returns>The filtered match query.</returns>
        private IQueryable<Match> BuildMatchQuery(
            HomeSearchRequestData query)
        {
            IQueryable<Match> matchQuery = matchRepository.Query();

            matchQuery = ApplyBaseFilters(matchQuery);
            matchQuery = ApplySportFilter(matchQuery, query.SportId);
            matchQuery = ApplyCityFilter(matchQuery, query.City);
            matchQuery = ApplyDateFilter(matchQuery, query.From);

            return matchQuery;
        }

        /// <summary>
        /// Restricts matches to those that are not deleted, are open or confirmed, have an event, and whose venue is not deleted.
        /// </summary>
        /// <param name="query">The match query to filter.</param>
        /// <returns>The filtered match query.</returns>
        private IQueryable<Match> ApplyBaseFilters(
            IQueryable<Match> query)
        {
            return query
                .Where(x => !x.IsDeleted)
                .Where(x => x.Status == MatchStatus.Open ||
                            x.Status == MatchStatus.Confirmed)
                .Where(x => x.Event != null)
                .Where(x => !x.Event!.Venue.IsDeleted);
        }

        /// <summary>
        /// Restricts matches to the given sport when one is specified; otherwise returns the query unchanged.
        /// </summary>
        /// <param name="query">The match query to filter.</param>
        /// <param name="sportId">The optional sport identifier to filter by.</param>
        /// <returns>The filtered match query.</returns>
        private IQueryable<Match> ApplySportFilter(
            IQueryable<Match> query,
            Guid? sportId)
        {
            if (!sportId.HasValue)
            {
                return query;
            }

            return query.Where(x =>
                x.Event!.SportId == sportId.Value);
        }

        /// <summary>
        /// Restricts matches to venues whose city matches the trimmed input, by containment in either direction. A blank city leaves the query unchanged.
        /// </summary>
        /// <param name="query">The match query to filter.</param>
        /// <param name="city">The optional city text to filter by.</param>
        /// <returns>The filtered match query.</returns>
        private IQueryable<Match> ApplyCityFilter(
            IQueryable<Match> query,
            string? city)
        {
            city = city?.Trim();

            if (string.IsNullOrWhiteSpace(city))
            {
                return query;
            }

            return query.Where(x =>
                x.Event!.Venue.City.Contains(city) ||
                city.Contains(x.Event!.Venue.City));
        }

        /// <summary>
        /// Restricts matches to venue dates on or after the start date. The start date is the requested date if it is after today; otherwise it is tomorrow.
        /// </summary>
        /// <param name="query">The match query to filter.</param>
        /// <param name="from">The optional earliest requested date.</param>
        /// <returns>The filtered match query.</returns>
        private IQueryable<Match> ApplyDateFilter(
            IQueryable<Match> query,
            DateOnly? from)
        {
            DateOnly today = DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            DateOnly startDate = (from.HasValue && from.Value > today)
                ? from.Value
                : today.AddDays(1);

            return query.Where(x =>
                x.Event!.Venue.Date >= startDate);
        }

        /// <summary>
        /// Counts the matches, clamps the page size to 1-50 and the page number to the valid range, and loads that page ordered by venue date, start time, sport name, venue name and id.
        /// </summary>
        /// <param name="query">The filtered match query.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>The page of match cards with paging metadata.</returns>
        private async Task<PageResult<HomeMatchCardResponseModel>>
            GetPageAsync(
                IQueryable<Match> query,
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
                .OrderBy(x => x.Event!.Venue.Date)
                .ThenBy(x => x.Event!.Venue.StartsAt)
                .ThenBy(x => x.Event!.Sport.Name)
                .ThenBy(x => x.Event!.Venue.Name)
                .ThenBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(HomeDataBuilder.HomeMatchCardProjection())
                .ToListAsync();

            return new PageResult<HomeMatchCardResponseModel>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }
    }
}