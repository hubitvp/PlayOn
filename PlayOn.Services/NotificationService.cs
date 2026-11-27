using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Notifications.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Provides retrieval and read-state management of a user's notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> notificationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="notificationRepository">The repository used to query and update notifications.</param>
        public NotificationService(
            IRepository<Notification> notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        /// <summary>
        /// Builds the notifications model containing one page of the user's notifications and their unread count. Fails with the underlying errors if either step fails.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A response with the notifications model on success, or the errors of the failed step.</returns>
        public async Task<ServiceResponse<NotificationResponseModel>>
            GetMainResponseModelAsync(
                Guid userId,
                PageRequestData pageRequestData)
        {
            var pageResponse = await GetSinglePageAsync(
                userId,
                pageRequestData);

            if (!pageResponse.IsSuccess)
            {
                return ServiceResponse<NotificationResponseModel>
                    .Failure(pageResponse.Errors);
            }

            var unreadCountResponse = await GetUnreadCountAsync(userId);

            if (!unreadCountResponse.IsSuccess)
            {
                return ServiceResponse<NotificationResponseModel>
                    .Failure(unreadCountResponse.Errors);
            }

            var responseData = NotificationDataBuilder
                .ToNotificationResponseModel(
                    pageResponse.ResponseData,
                    unreadCountResponse.ResponseData);

            return ServiceResponse<NotificationResponseModel>
                .Success(responseData);
        }

        /// <summary>
        /// Returns one page of the user's notifications, newest first.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>A successful response with the page of notifications.</returns>
        public async Task<ServiceResponse<PageResult<NotificationResponseModelData>>>
            GetSinglePageAsync(
                Guid userId,
                PageRequestData pageRequestData)
        {
            ArgumentNullException.ThrowIfNull(pageRequestData);

            var notificationQuery = BuildNotificationQuery(userId);

            var page = await GetPageAsync(
                notificationQuery,
                pageRequestData);

            return ServiceResponse<PageResult<NotificationResponseModelData>>
                .Success(page);
        }

        /// <summary>
        /// Counts the user's unread notifications.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>A successful response with the number of unread notifications.</returns>
        public async Task<ServiceResponse<int>>
            GetUnreadCountAsync(Guid userId)
        {
            var unreadCount = await notificationRepository
                .Query()
                .CountAsync(x => x.UserId == userId && !x.IsRead);

            return ServiceResponse<int>
                .Success(unreadCount);
        }

        /// <summary>
        /// Marks a single notification as read, saving only if it was unread. Returns not found if it does not exist and forbidden if it belongs to another user.
        /// </summary>
        /// <param name="userId">The identifier of the requesting user.</param>
        /// <param name="notificationId">The identifier of the notification to mark.</param>
        /// <returns>A response with the updated notification, or a not found or forbidden result.</returns>
        public async Task<ServiceResponse<NotificationResponseModelData>>
            MarkAsReadAsync(
                Guid userId,
                Guid notificationId)
        {
            var notification = await notificationRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == notificationId);

            if (notification == null)
            {
                return ServiceResponse<NotificationResponseModelData>
                    .NotFound();
            }

            if (notification.UserId != userId)
            {
                return ServiceResponse<NotificationResponseModelData>
                    .Forbidden();
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await notificationRepository.SaveChangesAsync();
            }

            var responseData = NotificationDataBuilder
                .NotificationToResponseModel(notification);

            return ServiceResponse<NotificationResponseModelData>
                .Success(responseData);
        }

        /// <summary>
        /// Marks all of the user's unread notifications as read and saves the changes.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>A successful response with the number of notifications marked as read (0 if none).</returns>
        public async Task<ServiceResponse<int>>
            MarkAllAsReadAsync(Guid userId)
        {
            var unreadNotifications = await notificationRepository
                .Query(asNoTracking: false)
                .Where(x => x.UserId == userId && !x.IsRead)
                .ToListAsync();

            if (unreadNotifications.Count == 0)
            {
                return ServiceResponse<int>.Success(0);
            }

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            await notificationRepository.SaveChangesAsync();

            return ServiceResponse<int>
                .Success(unreadNotifications.Count);
        }

        /// <summary>
        /// Creates a query for the notifications that belong to the given user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>The user's notification query.</returns>
        private IQueryable<Notification> BuildNotificationQuery(
            Guid userId)
        {
            return notificationRepository
                .Query()
                .Where(x => x.UserId == userId);
        }

        /// <summary>
        /// Counts the notifications, clamps the page size to 1-50 and the page number to the valid range, and loads that page ordered by creation date then id, both descending.
        /// </summary>
        /// <param name="query">The notification query.</param>
        /// <param name="pageRequestData">The requested page number and page size.</param>
        /// <returns>The page of notifications with paging metadata.</returns>
        private async Task<PageResult<NotificationResponseModelData>>
            GetPageAsync(
                IQueryable<Notification> query,
                PageRequestData pageRequestData)
        {
            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(
                pageRequestData.PageSize, 1, 50);

            var totalPages = Math.Max(
                1,
                (int)Math.Ceiling(totalCount / (double)pageSize));

            var pageNumber = Math.Clamp(
                pageRequestData.Page, 1, totalPages);

            var items = await query
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(NotificationDataBuilder.NotificationProjection())
                .ToListAsync();

            return new PageResult<NotificationResponseModelData>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }
    }
}