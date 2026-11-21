using PlayOn.Services.Data;
using PlayOn.Services.Data.Players.RequestData;
using PlayOn.Services.Data.Players.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IPlayerService
    {
        Task<ServiceResponse<PlayerProfileResponseModel>>
            GetProfileAsync(
                Guid userId,
                Guid targetProfileId);

        Task<ServiceResponse<PlayerEditResponseModel>>
            GetForEditAsync(Guid userId);

        Task<ServiceResponse<Guid>>
            UpdateAsync(PlayerUpdateRequestData request);
    }
}
