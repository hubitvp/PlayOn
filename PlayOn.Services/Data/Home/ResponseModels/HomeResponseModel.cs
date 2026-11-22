using PlayOn.Common.Helpers;

namespace PlayOn.Services.Data.Home.ResponseModels
{

    public class HomeResponseModel
    {
        public required PageResult<HomeMatchCardResponseModel>? Page { get; set; }

        public required IReadOnlyList<HomeSportOptionsResponseModel> SportOptions { get; init; }
    }
}
