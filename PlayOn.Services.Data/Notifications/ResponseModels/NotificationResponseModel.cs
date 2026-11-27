using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;

namespace PlayOn.Services.Data.Notifications.ResponseModels
{
    /// <summary>
    /// A single notification of a user.
    /// </summary>
    public class NotificationResponseModelData
    {
        /// <summary>
        /// Unique identifier of the notification.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Type of the notification.
        /// </summary>
        public required NotificationType Type { get; init; }
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
        /// Date and time the notification was created.
        /// </summary>
        public required DateTime CreatedOn { get; init; }
    }

    /// <summary>
    /// Response model holding a page of notifications and the number of unread ones.
    /// </summary>
    public class NotificationResponseModel
    {
        /// <summary>
        /// Page of notifications.
        /// </summary>
        public required PageResult<NotificationResponseModelData> Page { get; init; }
        /// <summary>
        /// Number of unread notifications of the user.
        /// </summary>
        public required int UnreadCount { get; init; }
    }
}