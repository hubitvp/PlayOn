using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Notifications.RequestData;
using PlayOn.Services.Data.Notifications.ResponseModels;

namespace PlayOn.Services.Contracts
{
    public interface INotificationService
    {
        Task<ServiceResponse<NotificationResponseModel>>
            GetMainResponseModelAsync(
                Guid userId,
                PageRequestData pageRequestData);

        Task<ServiceResponse<PageResult<NotificationResponseModelData>>>
            GetSinglePageAsync(
                Guid userId,
                PageRequestData pageRequestData);

        Task<ServiceResponse<int>>
            GetUnreadCountAsync(Guid userId);

        Task<ServiceResponse<NotificationResponseModelData>>
            MarkAsReadAsync(
                Guid userId,
                Guid notificationId);

        Task<ServiceResponse<int>>
            MarkAllAsReadAsync(Guid userId);
    }
}
