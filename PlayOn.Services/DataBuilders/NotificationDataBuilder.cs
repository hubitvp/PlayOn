using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Notifications.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class NotificationDataBuilder
{
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
        var result = new NotificationResponseModelData
        {
            Id = source.Id,
            Type = source.Type,
            Message = source.Message,
            MatchId = source.MatchId,
            IsRead = source.IsRead,
            CreatedOn = source.CreatedOn
        };

        return result;
    }

    public static NotificationResponseModel
        ToNotificationResponseModel(
            PageResult<NotificationResponseModelData> page,
            int unreadCount)
    {
        var result = new NotificationResponseModel
        {
            Page = page,
            UnreadCount = unreadCount
        };

        return result;
    }

    public static Notification
        NotificationToEntity(
            Guid userId,
            NotificationType type,
            string message,
            Guid? matchId,
            DateTime nowUtc)
    {
        var result = new Notification
        {
            UserId = userId,
            Type = type,
            Message = message,
            MatchId = matchId,
            IsRead = false,
            CreatedOn = nowUtc
        };

        return result;
    }
}
