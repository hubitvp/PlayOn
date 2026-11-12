using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Matches.RequestData;
using PlayOn.Services.Data.Matches.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IMatchService
    {
        Task<ServiceResponse<PageResult<MatchCardResponseModel>>>
            GetMatchCardsAsync(
                MatchSearchRequestData request);

        Task<ServiceResponse<MatchDetailsResponseModel>>
            GetDetailsAsync(
                int matchId,
                string? userId,
                bool isAdmin);


        Task<ServiceResponse<MatchEditResponseModel>>
            GetForEditAsync(
                int matchId,
                string actorId,
                bool actorIsAdmin);


        Task<ServiceResponse<int>>
            UpdateAsync(MatchUpdateRequestData request);


        Task<ServiceResponse<int>>
            CreateAsync(
                MatchCreateRequestData request);

        Task<MatchFormOptionsResponseModel> GetFormOptionsAsync();


        Task<ServiceResponse<MatchEditResponseModel>>
            GetForCancelAsync(
                int matchId,
                string actorId,
                bool actorIsAdmin);

        Task<ServiceResponse<int>>
            CancelAsync(MatchCancelRequestData request);
    }
}
