using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Match.RequestData;
using PlayOn.Services.Data.Match.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Provides searching, viewing, creating, editing and cancelling of matches.
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Gets one page of matches together with the filter options for the search page.
        /// </summary>
        /// <param name="request">The search filters.</param>
        /// <param name="pageRequestData">The paging parameters.</param>
        /// <returns>A response with the match page and options, or the validation errors.</returns>
        Task<ServiceResponse<MatchResponseModel>>
            GetMainResponseModelAsync(
                MatchSearchRequestData request,
                PageRequestData pageRequestData);

        /// <summary>
        /// Gets a single page of upcoming open or confirmed matches matching the search filters.
        /// </summary>
        /// <param name="searchRequestData">The search filters and sort order.</param>
        /// <param name="pageRequestData">The paging parameters.</param>
        /// <returns>A response with the page of match cards, or the validation errors.</returns>
        Task<ServiceResponse<PageResult<MatchCardResponseModel>>>
        GetSinglePageAsync(
            MatchSearchRequestData searchRequestData,
            PageRequestData pageRequestData);

        /// <summary>
        /// Gets all sports as selectable options, ordered by name.
        /// </summary>
        /// <returns>A response with the sport options.</returns>
        Task<ServiceResponse<IReadOnlyList<SportOptionResponseModel>>>
            GetSportOptionsAsync();

        /// <summary>
        /// Gets the free, undeleted venue slots that can still be booked.
        /// </summary>
        /// <param name="from">The earliest date wanted, or null for the minimum allowed date.</param>
        /// <returns>A response with the venue slot options.</returns>
        Task<ServiceResponse<IReadOnlyList<VenueOptionResponseModel>>>
            GetVenueOptionsAsync(
                DateOnly? from);

        /// <summary>
        /// Gets the read-only display data of a match for the edit page.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="userIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the display data, or a not-found or forbidden result.</returns>
        Task<ServiceResponse<MatchEditDisplayDataResponseModel>>
            GetEditDisplayDataAsync(
                Guid matchId,
                Guid userId,
                bool userIsAdmin);

        /// <summary>
        /// Gets all courts, with the sports they support, as selectable options.
        /// </summary>
        /// <returns>A response with the court options.</returns>
        Task<ServiceResponse<IReadOnlyList<CourtOptionResponseModel>>>
            GetCourtOptionsAsync();

        /// <summary>
        /// Gets the sport, court and venue options needed to create a match.
        /// </summary>
        /// <param name="from">The earliest venue date wanted, or null for the default.</param>
        /// <returns>A response with the combined options.</returns>
        Task<ServiceResponse<MatchOptionsResponseModel>>
            GetOptionsAsync(
                DateOnly? from);

        /// <summary>
        /// Gets the full details of a match, including participants and the actions available to the viewer.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the viewing user, or null for anonymous.</param>
        /// <param name="isAdmin">Whether the viewer is an administrator.</param>
        /// <returns>A response with the match details, or a not-found result.</returns>
        Task<ServiceResponse<MatchDetailsResponseModel>>
            GetDetailsAsync(
                Guid matchId,
                Guid? userId,
                bool isAdmin);

        /// <summary>
        /// Gets the editable data of a match.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="userIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the edit model, a not-found or forbidden result, or a time-locked result carrying the model when changes are no longer allowed.</returns>
        Task<ServiceResponse<MatchEditResponseModel>>
            GetForEditAsync(
                Guid matchId,
                Guid userId,
                bool userIsAdmin);

        /// <summary>
        /// Updates an existing match.
        /// </summary>
        /// <param name="request">The updated match data.</param>
        /// <returns>A response with the match identifier on success, or a validation, not-found, forbidden, time-locked or conflict result.</returns>
        Task<ServiceResponse<Guid>>
            UpdateAsync(
                MatchUpdateRequestData request);

        /// <summary>
        /// Creates a new match with the organiser as its first participant.
        /// </summary>
        /// <param name="request">The new match data.</param>
        /// <returns>A response with the new match identifier, or a validation failure or conflict.</returns>
        Task<ServiceResponse<Guid>>
            CreateAsync(
                MatchCreateRequestData request);

        /// <summary>
        /// Gets the summary shown on the cancel confirmation page.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="userIsAdmin">Whether the requesting user is an administrator.</param>
        /// <returns>A response with the cancellation summary, or a not-found, forbidden or time-locked result.</returns>
        Task<ServiceResponse<MatchCancelResponseModel>>
            GetForCancelAsync(
                Guid matchId,
                Guid userId,
                bool userIsAdmin);

        /// <summary>
        /// Cancels a match and notifies its other active participants.
        /// </summary>
        /// <param name="request">The cancellation data, including the reason.</param>
        /// <returns>A response with the match identifier on success, or a validation, not-found, forbidden, time-locked or conflict result.</returns>
        Task<ServiceResponse<Guid>>
            CancelAsync(
                MatchCancelRequestData request);
    }
}
