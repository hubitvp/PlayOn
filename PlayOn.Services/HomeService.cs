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
    public class HomeService : IHomeService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly TimeProvider timeProvider;

        public HomeService(
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository,
            TimeProvider timeProvider)
        {
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
            this.timeProvider = timeProvider;
        }

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

        private IQueryable<Match> ApplyBaseFilters(
            IQueryable<Match> query)
        {
            return query
                .Where(x => !x.IsDeleted && !x.IsHidden)
                .Where(x => x.Status == MatchStatus.Open ||
                            x.Status == MatchStatus.Confirmed)
                .Where(x => x.Event != null)
                .Where(x => !x.Event!.Venue.IsDeleted);
        }

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