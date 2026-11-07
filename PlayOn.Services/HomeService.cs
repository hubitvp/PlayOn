using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Mappers;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services
{
    public class HomeService : IHomeService
    {
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly ServiceMapper mapper = new();

        public HomeService(
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository)
        {
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
        }

        public async Task<ServiceResponse<PagedResult<MatchCardResponseModel>>>
            GetMatchCardsAsync(HomeQueryRequestData query)
        {
            var validationErrors = ServiceRequestDataValidator.Validate(query);


            if (validationErrors.Any())
            {
                return ServiceResponse<PagedResult<MatchCardResponseModel>>
                    .Failure(validationErrors);
            }

            if (query.SportId.HasValue)
            {
                bool sportExists = await sportRepository
                    .Query()
                    .AnyAsync(x => x.Id == query.SportId.Value);

                if (!sportExists)
                {
                    var error = new ValidationResult(
                        "Select a valid sport.",
                        new[] { nameof(query.SportId) });

                    return ServiceResponse<PagedResult<MatchCardResponseModel>>
                        .Failure(new[] { error });
                }
            }


            IQueryable<Match> matchQuerySource = matchRepository
                .Query()
                .AsSplitQuery()
                .Include(x => x.Event!)
                    .ThenInclude(x => x.Sport)
                .Include(x => x.Event!)
                    .ThenInclude(x => x.Venue)
                .Include(x => x.Participants);

            matchQuerySource = matchQuerySource
                .Where(x => !x.IsDeleted && !x.IsHidden)
                .Where(x => x.Status == MatchStatus.Open ||
                            x.Status == MatchStatus.Confirmed)
                .Where(x => x.Event != null)
                .Where(x => !x.Event!.Venue.IsDeleted);


            if (query.SportId.HasValue)
            {
                matchQuerySource = matchQuerySource
                    .Where(x =>
                        x.Event!.SportId == query.SportId.Value);
            }

            var city = query.City?.Trim();

            if (!string.IsNullOrWhiteSpace(city))
            {
                matchQuerySource = matchQuerySource
                    .Where(x =>
                        x.Event!.Venue.City.Contains(city)
                        || city.Contains(x.Event!.Venue.City));
            }

            if (query.From.HasValue)
            {
                matchQuerySource = matchQuerySource
                    .Where(x =>
                        x.Event!.Venue.Date >= query.From.Value);
            }

            var totalCount = await matchQuerySource.CountAsync();

            var pageSize = Math.Clamp(query.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(
                totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(query.Page, 1, totalPages);


            matchQuerySource = matchQuerySource
                .OrderBy(x => x.Event!.Venue.Date)
                .ThenBy(x => x.Event!.Venue.StartsAt)
                .ThenBy(x => x.Event!.Sport.Name)
                .ThenBy(x => x.Event!.Venue.Name)
                .ThenBy(x => x.Id);

            var items = await matchQuerySource
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var page = new PagedResult<Match>(
                items,
                pageNumber,
                pageSize,
                totalCount);


            PagedResult<MatchCardResponseModel> response = page
                .Map(mapper.ToMatchCardResponse);


            return ServiceResponse<PagedResult<MatchCardResponseModel>>
                .Success(response);
        }


        public async Task<ServiceResponse<IEnumerable<SportsResponseModel>>> GetSportsAsync()
        {
            IEnumerable<SportsResponseModel> sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(x => mapper.ToSportsResponse(x))
                .ToListAsync();

            return ServiceResponse<IEnumerable<SportsResponseModel>>.Success(sports);
        }


        public async Task<ServiceResponse<bool>> SportExistsAsync(int sportId)
        {
            bool exists = await sportRepository
                .Query()
                .AnyAsync(x => x.Id == sportId);

            return ServiceResponse<bool>.Success(exists);
        }

    }
}
