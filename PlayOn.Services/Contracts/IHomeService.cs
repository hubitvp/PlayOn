using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IHomeService
    {

        Task<bool> SportExistsAsync(int sportId);

        Task<IEnumerable<SportsResponseModel>> GetSportsAsync();

        Task<ServiceResponse<PagedResult<MatchCardResponseModel>>>
            GetMatchCardsAsync(HomeQueryRequestData query);
    }
}
