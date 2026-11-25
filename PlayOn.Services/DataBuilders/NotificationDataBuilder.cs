using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Notifications.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class NotificationDataBuilder
{
    private static readonly Lazy<Func<Notification, NotificationResponseModelData>> CompiledProjection =
        new(() => NotificationProjection().Compile());

    public static Expression<Func<Notification, NotificationResponseModelData>>
        NotificationProjection()
    {
        return source => new NotificationResponseModelData
        {
            Id = source.Id,
            Type = source.Type,
            Message = source.Message,
            MatchId = source.MatchId,
            IsRead = source.IsRead,
            CreatedOn = source.CreatedOn
        };
    }

    public static NotificationResponseModelData
        NotificationToResponseModel(Notification source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return CompiledProjection.Value(source);
    }

    public static NotificationResponseModel
        ToNotificationResponseModel(
            PageResult<NotificationResponseModelData> page,
            int unreadCount)
    {
        ArgumentNullException.ThrowIfNull(page);

        return new NotificationResponseModel
        {
            Page = page,
            UnreadCount = unreadCount
        };
    }

    public static Notification
        NotificationToEntity(
            Guid userId,
            NotificationType type,
            string message,
            Guid? matchId,
            DateTime nowUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        return new Notification
        {
            UserId = userId,
            Type = type,
            Message = message,
            MatchId = matchId,
            IsRead = false,
            CreatedOn = nowUtc
        };
    }
}