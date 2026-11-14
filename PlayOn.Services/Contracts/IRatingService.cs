using PlayOn.Services.Data;
using PlayOn.Services.Data.Ratings.RequestData;
using PlayOn.Services.Data.Ratings.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IRatingService
    {
        Task<ServiceResponse<RatingFormResponseModel>>
            GetFormAsync(
                Guid matchId,
                Guid raterId);

        Task<ServiceResponse<int>>
            SubmitAsync(RatingRequestData request);
    }
}
