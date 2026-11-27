using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Notifications.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Provides mapping between notification entities and response models.
/// </summary>
public static class NotificationDataBuilder
{
    /// <summary>
    /// Creates a projection from a notification to its response model data.
    /// </summary>
    /// <returns>An expression projecting a <see cref="Notification"/> to a <see cref="NotificationResponseModelData"/>.</returns>
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

    /// <summary>
    /// Maps a notification entity to its response model data.
    /// </summary>
    /// <param name="source">The notification entity.</param>
    /// <returns>The mapped response model data.</returns>
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

    /// <summary>
    /// Combines a page of notifications and the unread count into the notifications response model.
    /// </summary>
    /// <param name="page">The page of notifications.</param>
    /// <param name="unreadCount">The number of unread notifications.</param>
    /// <returns>The notifications response model.</returns>
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

    /// <summary>
    /// Creates a new unread notification entity with the given values.
    /// </summary>
    /// <param name="userId">The identifier of the recipient user.</param>
    /// <param name="type">The notification type.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="matchId">The optional related match identifier.</param>
    /// <param name="nowUtc">The creation time in UTC.</param>
    /// <returns>The new notification entity, marked as unread.</returns>
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