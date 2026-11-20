using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Notifications.RequestData
{
    public class NotificationRequestData
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
