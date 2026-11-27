using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminUsers.RequestData;
using PlayOn.Services.Data.AdminUsers.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines administrative management operations for users.
    /// </summary>
    public interface IAdminUserService
    {
        /// <summary>
        /// Gets a filtered, paged list of users with their administrator flag.
        /// </summary>
        /// <param name="request">The search, status filter and paging options.</param>
        /// <returns>A page of users, or a failure response when the request is invalid.</returns>
        Task<ServiceResponse<PageResult<AdminUserResponseModel>>>
            GetUsersAsync(AdminUserQueryRequestData request);

        /// <summary>
        /// Activates or deactivates a user account.
        /// </summary>
        /// <param name="request">The target user, the desired state and the acting administrator.</param>
        /// <returns>The identifier of the user, or a not-found, conflict or failure response.</returns>
        Task<ServiceResponse<Guid>>
            SetActiveAsync(AdminUserStatusRequestData request);

        /// <summary>
        /// Grants or revokes administrator rights for a user.
        /// </summary>
        /// <param name="request">The target user, the desired role state and the acting administrator.</param>
        /// <returns>The identifier of the user, or a not-found, conflict or failure response.</returns>
        Task<ServiceResponse<Guid>>
            SetAdministratorAsync(AdminUserRoleRequestData request);
    }
}
