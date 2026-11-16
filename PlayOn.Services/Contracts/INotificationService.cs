using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Notifications.RequestData;
using PlayOn.Services.Data.Notifications.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface INotificationService
    {
        Task<ServiceResponse<NotificationResponseModel>>
            GetNotificationsAsync(NotificationRequestData request);

        Task<int>
            GetUnreadCountAsync(Guid userId);

        Task<ServiceResponse<NotificationResponseModel>>
            MarkAsReadAsync(
                Guid userId,
                Guid notificationId);

        Task<ServiceResponse<int>>
            MarkAllAsReadAsync(Guid userId);
    }
}
