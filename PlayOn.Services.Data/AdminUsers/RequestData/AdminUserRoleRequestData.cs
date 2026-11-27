using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminUsers.RequestData
{
    /// <summary>
    /// Request data for granting or revoking the administrator role of a user.
    /// </summary>
    public class AdminUserRoleRequestData
    {
        /// <summary>
        /// Identifier of the user whose role is changed.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The user is not valid.")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Identifier of the administrator performing the action.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The administrator is not valid.")]
        public Guid ActorId { get; set; }

        /// <summary>
        /// <c>true</c> to grant the administrator role; <c>false</c> to revoke it.
        /// </summary>
        public bool IsAdministrator { get; set; }
    }
}
