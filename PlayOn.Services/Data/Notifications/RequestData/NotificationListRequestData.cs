using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Notifications.RequestData
{
    public class NotificationListRequestData
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
