using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Notifications.RequestData
{
    /// <summary>
    /// Request data for retrieving the notifications of a user.
    /// </summary>
    public class NotificationRequestData
    {
        /// <summary>
        /// Identifier of the user whose notifications are requested.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
    }
}
