using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.Notifications.RequestData
{
    public class NotificationListRequestData
    {
        [Required]
        public Guid UserId { get; set; }

        public int Page { get; set; } = FirstPage;

        public int PageSize { get; set; } = DefaultPageSize;
    }
}
