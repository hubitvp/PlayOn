using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Participation.RequestData;
using PlayOn.Services.Data.Participation.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines operations for joining and leaving matches and listing a user's matches.
    /// </summary>
    public interface IParticipationService
    {
        /// <summary>
        /// Joins the user to a match, as a player or on the waitlist, subject to the participation rules.
        /// </summary>
        /// <param name="request">The participation request containing the match and user identifiers.</param>
        /// <returns>A service response with the participant status, waitlist position and whether the match was confirmed.</returns>
        Task<ServiceResponse<JoinMatchResponseModel>>
            JoinAsync(ParticipationRequestData request);

        /// <summary>
        /// Removes the user from a match; a late leave counts as a strike and a freed spot may promote a waitlisted player.
        /// </summary>
        /// <param name="request">The participation request containing the match and user identifiers.</param>
        /// <returns>A service response indicating whether the leave was a late cancel and whether another player was promoted.</returns>
        Task<ServiceResponse<LeaveMatchResponseModel>>
            LeaveAsync(ParticipationRequestData request);

        /// <summary>
        /// Gets one page of the user's matches for the selected tab.
        /// </summary>
        /// <param name="requestData">The request containing the user and the selected tab.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A service response with the page of matches.</returns>
        Task<ServiceResponse<PageResult<MyMatchResponseModel>>>
            GetSinglePageAsync(
                MyMatchesRequestData requestData,
                PageRequestData pageRequestData);
    }
}
