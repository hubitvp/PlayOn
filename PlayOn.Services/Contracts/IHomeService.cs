using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.Output;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IHomeService
    {

        Task<bool> SportExistsAsync(int sportId);

        Task<IEnumerable<SportsResponseModel>> GetSportsAsync();

        Task<ServiceResponse<IEnumerable<MatchCardResponseModel>>>
            GetMatchCardsAsync(HomeQueryRequestData query);
    }
}
