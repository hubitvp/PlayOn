using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Notifications.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> notificationRepository;

        public NotificationService(
            IRepository<Notification> notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

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

            var unreadCountResponse = await GetUnreadCountAsync(
                userId);

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

        public async Task<ServiceResponse<PageResult<NotificationResponseModelData>>>
            GetSinglePageAsync(
                Guid userId,
                PageRequestData pageRequestData)
        {

            var notificationQuery = BuildNotificationQuery(userId);

            var page = await GetSinglePageAsync(
                notificationQuery,
                pageRequestData);

            return ServiceResponse<PageResult<NotificationResponseModelData>>
                .Success(page);
        }

        public async Task<ServiceResponse<int>>
            GetUnreadCountAsync(Guid userId)
        {
            var unreadCount = await notificationRepository
                .Query()
                .CountAsync(x => x.UserId == userId && !x.IsRead);

            return ServiceResponse<int>
                .Success(unreadCount);
        }

        public async Task<ServiceResponse<NotificationResponseModelData>>
            MarkAsReadAsync(
                Guid userId,
                Guid notificationId)
        {
            var notification = await notificationRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x =>
                    x.Id == notificationId && x.UserId == userId);

            if (notification == null)
            {
                return ServiceResponse<NotificationResponseModelData>
                    .NotFound();
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

        private IOrderedQueryable<Notification> BuildNotificationQuery(
            Guid userId)
        {
            return notificationRepository
                .Query()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn);
        }

        private async Task<PageResult<NotificationResponseModelData>>
            GetSinglePageAsync(
                IOrderedQueryable<Notification> query,
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
