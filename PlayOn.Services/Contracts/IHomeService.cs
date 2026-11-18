using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IHomeService
    {
        Task<ServiceResponse<HomeResponseModel>>
            GetMainResponseModelAsync(
                HomeSearchRequestData searchRequestData,
                PageRequestData pageRequestData);

        Task<ServiceResponse<PageResult<HomeMatchCardResponseModel>>>
            GetSinglePageAsync(
                HomeSearchRequestData searchRequestData,
                PageRequestData pageRequestData);

        Task<ServiceResponse<IReadOnlyList<HomeSportOptionsResponseModel>>>
            GetSportOptionsAsync();
    }
}
