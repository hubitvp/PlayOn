using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Notifications.RequestData;
using PlayOn.Services.Data.Notifications.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> notificationRepository;

        public NotificationService(IRepository<Notification> notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<ServiceResponse<NotificationListResponseModel>>
            GetNotificationsAsync(
                NotificationListRequestData requestData,
                PageRequestData pageRequestData)
        {
            var validationErrors =
                ServiceRequestDataValidator.Validate(requestData);

            if (validationErrors.Any())
            {
                return ServiceResponse<NotificationListResponseModel>
                    .Failure(validationErrors);
            }

            var page = await BuildNotificationQuery(requestData.UserId)
                .ToPageResultAsync(
                    NotificationDataBuilder.NotificationProjection(),
                    pageRequestData);

            var unreadCount = await CountUnreadAsync(requestData.UserId);

            var responseData = new NotificationListResponseModel
            {
                Page = page,
                UnreadCount = unreadCount
            };

            return ServiceResponse<NotificationListResponseModel>
                .Success(responseData);
        }

        public async Task<ServiceResponse<int>>
            GetUnreadCountAsync(Guid userId)
        {
            var unreadCount = await CountUnreadAsync(userId);

            return ServiceResponse<int>.Success(unreadCount);
        }

        public async Task<ServiceResponse<NotificationResponseModel>>
            MarkAsReadAsync(
                Guid userId,
                Guid notificationId)
        {
            var notification = await notificationRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);

            if (notification == null)
            {
                return ServiceResponse<NotificationResponseModel>.NotFound();
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;

                await notificationRepository.SaveChangesAsync();
            }

            return ServiceResponse<NotificationResponseModel>
                .Success(NotificationDataBuilder.NotificationToResponseModel(notification));
        }

        public async Task<ServiceResponse<int>>
            MarkAllAsReadAsync(Guid userId)
        {
            var unread = await notificationRepository
                .Query(asNoTracking: false)
                .Where(x => x.UserId == userId && !x.IsRead)
                .ToListAsync();

            foreach (var notification in unread)
            {
                notification.IsRead = true;
            }

            if (unread.Count > 0)
            {
                await notificationRepository.SaveChangesAsync();
            }

            return ServiceResponse<int>.Success(unread.Count);
        }


        private IOrderedQueryable<Notification> BuildNotificationQuery(Guid userId)
        {
            return notificationRepository
                .Query()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id);
        }

        private async Task<int> CountUnreadAsync(Guid userId)
        {
            return await notificationRepository
                .Query()
                .CountAsync(x => x.UserId == userId && !x.IsRead);
        }
    }
}
