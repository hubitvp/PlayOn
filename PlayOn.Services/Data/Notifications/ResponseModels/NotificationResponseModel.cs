using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Notifications.ResponseModels
{
    public class NotificationResponseModel
    {
        public required Guid Id { get; init; }
        public required NotificationType Type { get; init; }
        public required string Message { get; init; }
        public Guid? MatchId { get; init; }
        public required bool IsRead { get; init; }
        public required DateTime CreatedOn { get; init; }
    }
}
