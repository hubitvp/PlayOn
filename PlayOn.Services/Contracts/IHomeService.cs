using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IHomeService
    {

        Task<ServiceResponse<PageResult<HomeMatchCardResponseModel>>>
            GetMatchCardsAsync(HomeSearchRequestData query);
    }
}
