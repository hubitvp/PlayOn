using PlayOn.Services.Data;
using PlayOn.Services.Data.Players.RequestData;
using PlayOn.Services.Data.Players.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines operations for viewing and updating player profiles.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Gets the public profile of an active player.
        /// </summary>
        /// <param name="userId">The identifier of the viewing user.</param>
        /// <param name="targetProfileId">The identifier of the player whose profile is requested.</param>
        /// <returns>A service response with the profile, or not found.</returns>
        Task<ServiceResponse<PlayerProfileResponseModel>>
            GetProfileAsync(
                Guid userId,
                Guid targetProfileId);

        /// <summary>
        /// Gets the data needed to edit the user's own profile and sport skill levels.
        /// </summary>
        /// <param name="userId">The identifier of the user to edit.</param>
        /// <returns>A service response with the edit model, or not found.</returns>
        Task<ServiceResponse<PlayerEditResponseModel>>
            GetForEditAsync(Guid userId);

        /// <summary>
        /// Updates the user's display name, city and sport skill levels.
        /// </summary>
        /// <param name="request">The update request.</param>
        /// <returns>A service response with the user identifier, or validation errors or not found.</returns>
        Task<ServiceResponse<Guid>>
            UpdateAsync(PlayerUpdateRequestData request);
    }
}
