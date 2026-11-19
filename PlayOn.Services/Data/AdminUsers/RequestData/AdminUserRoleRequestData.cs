using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminUsers.RequestData
{
    public class AdminUserRoleRequestData
    {
        [NotEmptyGuid(ErrorMessage = "The user is not valid.")]
        public Guid UserId { get; set; }

        [NotEmptyGuid(ErrorMessage = "The administrator is not valid.")]
        public Guid ActorId { get; set; }

        public bool IsAdministrator { get; set; }
    }
}
