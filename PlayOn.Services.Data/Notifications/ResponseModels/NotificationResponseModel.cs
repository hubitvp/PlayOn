using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;

namespace PlayOn.Services.Data.Notifications.ResponseModels
{
    public class NotificationResponseModelData
    {
        public required Guid Id { get; init; }
        public required NotificationType Type { get; init; }
        public required string Message { get; init; }
        public Guid? MatchId { get; init; }
        public required bool IsRead { get; init; }
        public required DateTime CreatedOn { get; init; }
    }

    public class NotificationResponseModel
    {
        public required PageResult<NotificationResponseModelData> Page { get; init; }
        public required int UnreadCount { get; init; }
    }
}