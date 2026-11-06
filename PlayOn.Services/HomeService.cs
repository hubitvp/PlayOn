using PlayOn.Common.Helpers;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Repositories.Contracts;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Mappers;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository repository;
        private readonly ServiceMapper mapper = new();
        public HomeService(IHomeRepository repository)
        {
            this.repository = repository;
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
                bool sportExists = await repository.SportExistsAsync(query.SportId.Value);

                if (!sportExists)
                {
                    var error = new ValidationResult(
                        "Select a valid sport.",
                        new[] { nameof(query.SportId) });

                    return ServiceResponse<PagedResult<MatchCardResponseModel>>
                        .Failure(new[] { error });
                }
            }

            var matchQuery = mapper.ToMatchQuery(query);
            matchQuery.City = query.City?.Trim();

            var page = await repository.GetMatchCardsAsync(matchQuery);


            var response = page.Map(mapper.ToMatchCardResponse);


            return ServiceResponse<PagedResult<MatchCardResponseModel>>
                .Success(response);
        }


        public async Task<IEnumerable<SportsResponseModel>> GetSportsAsync()
        {
            var spots = await repository.GetSportsAsync();

            return spots.Select(mapper.ToSportsResponse).ToList();
        }


        public async Task<bool> SportExistsAsync(int sportId)
        {
            return await repository.SportExistsAsync(sportId);
        }

    }
}
