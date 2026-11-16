using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Helpers;
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

        public async Task<ServiceResponse<NotificationResponseModel>>
            GetNotificationsAsync(NotificationRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<NotificationResponseModel>
                    .Failure(errors);
            }

            var query = notificationRepository
                .Query()
                .Where(x => x.UserId == request.UserId);

            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(request.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var pageNumber = Math.Clamp(request.Page, 1, totalPages);

            var items = await query
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(NotificationDataBuilder.NotificationProjection())
                .ToListAsync();

            var page = new PageResult<NotificationResponseModel>(
                items,
                pageNumber,
                pageSize,
                totalCount);

            var response = new NotificationListResponseModel
            {
                Notifications = page,
                UnreadCount = await GetUnreadCountAsync(request.UserId)
            };

            return ServiceResponse<NotificationListResponseModel>
                .Success(response);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await notificationRepository
                .Query()
                .CountAsync(x => x.UserId == userId && !x.IsRead);
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


    }
}
