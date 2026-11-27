using PlayOn.Common.Helpers;

namespace PlayOn.Web.ViewModels.Notifications
{
    /// <summary>
    /// View model of the notifications page: a page of notifications and the number of unread ones.
    /// </summary>
    public class NotificationListViewModel
    {
        /// <summary>
        /// Page of notifications.
        /// </summary>
        public required PageResult<NotificationViewModel> Page { get; init; }

        /// <summary>
        /// Number of unread notifications of the user.
        /// </summary>
        public required int UnreadCount { get; init; }
    }

    /// <summary>
    /// A single notification shown to the user.
    /// </summary>
    public class NotificationViewModel
    {
        /// <summary>
        /// Unique identifier of the notification.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Text of the notification.
        /// </summary>
        public required string Message { get; init; }
        /// <summary>
        /// Identifier of the related match, if any.
        /// </summary>
        public Guid? MatchId { get; init; }
        /// <summary>
        /// Indicates whether the notification has been read.
        /// </summary>
        public required bool IsRead { get; init; }
        /// <summary>
        /// Formatted date and time the notification was created.
        /// </summary>
        public required string CreatedOn { get; init; }
        /// <summary>
        /// CSS class of the icon shown with the notification.
        /// </summary>
        public required string IconCss { get; init; }
    }
}
