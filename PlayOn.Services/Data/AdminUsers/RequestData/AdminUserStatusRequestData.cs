using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminUsers.RequestData
{
    public class AdminUserStatusRequestData
    {
        [NotEmptyGuid(ErrorMessage = "The user is not valid.")]
        public Guid UserId { get; set; }

        [NotEmptyGuid(ErrorMessage = "The administrator is not valid.")]
        public Guid ActorId { get; set; }

        public bool IsActive { get; set; }
    }
}
