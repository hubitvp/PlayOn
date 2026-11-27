using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Notifications.ResponseModels;

namespace PlayOn.Services.Contracts
{
    /// <summary>
    /// Defines the operations for reading and marking a user's notifications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Builds the notifications model with one page of notifications and the unread count.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the notifications model, or the errors of the failed step.</returns>
        Task<ServiceResponse<NotificationResponseModel>>
            GetMainResponseModelAsync(
                Guid userId,
                PageRequestData pageRequestData);

        /// <summary>
        /// Returns one page of the user's notifications.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the page of notifications.</returns>
        Task<ServiceResponse<PageResult<NotificationResponseModelData>>>
            GetSinglePageAsync(
                Guid userId,
                PageRequestData pageRequestData);

        /// <summary>
        /// Counts the user's unread notifications.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>A response with the unread count.</returns>
        Task<ServiceResponse<int>>
            GetUnreadCountAsync(Guid userId);

        /// <summary>
        /// Marks one notification as read. Not found if it does not exist, forbidden if it belongs to another user.
        /// </summary>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="notificationId">The identifier of the notification to mark.</param>
        /// <returns>A response with the updated notification, or a not found or forbidden result.</returns>
        Task<ServiceResponse<NotificationResponseModelData>>
            MarkAsReadAsync(
                Guid userId,
                Guid notificationId);

        /// <summary>
        /// Marks all of the user's unread notifications as read.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>A response with the number of notifications marked as read.</returns>
        Task<ServiceResponse<int>>
            MarkAllAsReadAsync(Guid userId);
    }
}
