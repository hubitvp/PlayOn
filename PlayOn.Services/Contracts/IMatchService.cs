using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Match.RequestData;
using PlayOn.Services.Data.Match.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IMatchService
    {
        Task<ServiceResponse<PageResult<MatchCardResponseModel>>>
            GetMatchCardsAsync(
                MatchSearchRequestData request);

        Task<ServiceResponse<MatchDetailsResponseModel>>
            GetDetailsAsync(
                Guid matchId,
                Guid? userId,
                bool isAdmin);


        Task<ServiceResponse<MatchEditResponseModel>>
            GetForEditAsync(
                Guid matchId,
                Guid actorId,
                bool actorIsAdmin);


        Task<ServiceResponse<Guid>>
            UpdateAsync(MatchUpdateRequestData request);


        Task<ServiceResponse<Guid>>
            CreateAsync(
                MatchCreateRequestData request);

        Task<MatchFormOptionsResponseModel> GetFormOptionsAsync();


        Task<ServiceResponse<MatchEditResponseModel>>
            GetForCancelAsync(
                Guid matchId,
                Guid actorId,
                bool actorIsAdmin);

        Task<ServiceResponse<Guid>>
            CancelAsync(MatchCancelRequestData request);
    }
}
