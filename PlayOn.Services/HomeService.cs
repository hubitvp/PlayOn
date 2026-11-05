using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.Output;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Repositories.Contracts;
using PlayOn.Services.Repositories.Data.Input;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository repository;
        public HomeService(IHomeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ServiceResponse<IEnumerable<MatchCardResponseModel>>>
            GetMatchCardsAsync(HomeQueryRequestData query)
        {
            var validationErrors = ServiceRequestDataValidator.Validate(query);

            if (validationErrors.Any())
            {
                return ServiceResponse<IEnumerable<MatchCardResponseModel>>.Failure(validationErrors);
            }

            if (query.SportId.HasValue)
            {
                bool sportExists = await repository.SportExistsAsync(query.SportId.Value);

                if (!sportExists)
                {
                    var error = new ValidationResult("Select a valid sport.", new[] { nameof(query.SportId) });
                    return ServiceResponse<IEnumerable<MatchCardResponseModel>>
                        .Failure(new[] { error });
                }
            }

            var matchQuery = new MatchQueryDto
            {
                SportId = query.SportId,
                City = query.City?.Trim(),
                From = query.From
            };

            var matchCards = await repository.GetMatchCardsAsync(matchQuery);

            var responseData = matchCards
                .Select(x => new MatchCardResponseModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    SportName = x.SportName,
                    SportIcon = x.SportIcon,
                    VenueName = x.VenueName,
                    City = x.City,
                    Date = x.Date,
                    StartsAt = x.StartsAt,
                    Status = x.Status,
                    Waitlisted = x.Waitlisted,
                    PricePerPlayer = x.PricePerPlayer,
                    MinSkill = x.MinSkill,
                    MaxPlayers = x.MaxPlayers,
                    Joined = x.Joined,
                    DurationMinutes = x.DurationMinutes,
                })
                .ToList();



            return ServiceResponse<IEnumerable<MatchCardResponseModel>>
                .Success(responseData);
        }


        public async Task<IEnumerable<SportsResponseModel>> GetSportsAsync()
        {
            var spots = await repository.GetSportsAsync();

            return spots
                .Select(x => new SportsResponseModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
        }


        public async Task<bool> SportExistsAsync(int sportId)
        {
            return await repository.SportExistsAsync(sportId);
        }

        //public async Task<OutputHomeData> GetHomeDataAsync(HomeItemFilter itemFilter)
        //{

        //    int playersCount = await repository.GetPlayersCountAsync();
        //    int venuesCount = await repository.GetVenuesCountAsync();

        //    var sports = await repository.GetSportsAsync();

        //    var matches = await repository.GetMatchCardsAsync(itemFilter);


        //    var homeData = new OutputHomeData
        //    {
        //        PlayersCount = playersCount,
        //        VenuesCount = venuesCount,
        //        Sports = sports,
        //        MatchCards = matches,
        //    };
        //    return homeData;
        //}
    }
}
