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
        return new NotificationResponseModelData
        {
            Id = source.Id,
            Type = source.Type,
            Message = source.Message,
            MatchId = source.MatchId,
            IsRead = source.IsRead,
            CreatedOn = source.CreatedOn
        };
    }

    public static NotificationResponseModel
        ToNotificationResponseModel(
            PageResult<NotificationResponseModelData> page,
            int unreadCount)
    {
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