using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminUsers.RequestData
{
    /// <summary>
    /// Request data for activating or deactivating a user account.
    /// </summary>
    public class AdminUserStatusRequestData
    {
        /// <summary>
        /// Identifier of the user whose status is changed.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The user is not valid.")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Identifier of the administrator performing the action.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The administrator is not valid.")]
        public Guid ActorId { get; set; }

        /// <summary>
        /// <c>true</c> to activate the user; <c>false</c> to deactivate it.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
