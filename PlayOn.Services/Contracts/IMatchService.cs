using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Match.RequestData;
using PlayOn.Services.Data.Match.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface IMatchService
    {
        Task<ServiceResponse<MatchResponseModel>>
            GetMainResponseModelAsync(
                MatchSearchRequestData request,
                PageRequestData pageRequestData);

        Task<ServiceResponse<PageResult<MatchCardResponseModel>>>
        GetSinglePageAsync(
            MatchSearchRequestData searchRequestData,
            PageRequestData pageRequestData);

        Task<ServiceResponse<IReadOnlyList<SportOptionResponseModel>>>
            GetSportOptionsAsync();

        Task<ServiceResponse<IReadOnlyList<VenueOptionResponseModel>>>
            GetVenueOptionsAsync(DateOnly? from);

        Task<ServiceResponse<IReadOnlyList<CourtOptionResponseModel>>>
            GetCourtOptionsAsync();

        Task<ServiceResponse<MatchOptionsResponseModel>>
            GetOptionsAsync(DateOnly? from);

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

        Task<ServiceResponse<MatchEditResponseModel>>
            GetForCancelAsync(
                Guid matchId,
                Guid actorId,
                bool actorIsAdmin);

        Task<ServiceResponse<Guid>>
            CancelAsync(MatchCancelRequestData request);
    }
}
