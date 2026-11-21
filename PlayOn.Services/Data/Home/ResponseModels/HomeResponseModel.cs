using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Sport.ResponseModels;

namespace PlayOn.Services.Data.Home.ResponseModels
{

    public class HomeResponseModel
    {
        public required PageResult<HomeMatchCardResponseModel>? Page { get; set; }

        public required IReadOnlyList<HomeSportOptionsResponseModel> SportOptions { get; init; }
    }
}
