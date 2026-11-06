using PlayOn.Common.Helpers;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
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

            var matchQuery = new MatchQueryDto
            {
                SportId = query.SportId,
                City = query.City?.Trim(),
                From = query.From,

                Page = query.Page,
                PageSize = query.PageSize,
            };

            var page = await repository.GetMatchCardsAsync(matchQuery);


            var response = page.Map(x => new MatchCardResponseModel
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
            });


            return ServiceResponse<PagedResult<MatchCardResponseModel>>
                .Success(response);
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

    }
}
