using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Participation.RequestData;
using PlayOn.Services.Data.Participation.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IParticipationService
    {
        Task<ServiceResponse<JoinMatchResponseModel>>
            JoinAsync(ParticipationRequestData request);

        Task<ServiceResponse<LeaveMatchResponseModel>>
            LeaveAsync(ParticipationRequestData request);

        Task<ServiceResponse<PageResult<MyMatchResponseModel>>>
            GetMineAsync(MyMatchesRequestData request);
    }
}
