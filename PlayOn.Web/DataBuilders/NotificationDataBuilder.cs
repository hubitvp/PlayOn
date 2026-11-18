//using PlayOn.Common.Enums;
//using PlayOn.Common.Helpers;
//using PlayOn.Services.Data.Notifications.RequestData;
//using PlayOn.Services.Data.Notifications.ResponseModels;
//using PlayOn.Web.DataBuilders.Formatting;
//using PlayOn.Web.ViewModels.Notifications;
//using static PlayOn.Common.ApplicationConstants.PaginationConstants;

//namespace PlayOn.Web.DataBuilders;

//public class NotificationDataBuilder
//{
//    private readonly IDisplayFormatter displayFormatter;
//    private readonly TimeProvider timeProvider;

//    public NotificationDataBuilder(
//        IDisplayFormatter displayFormatter,
//        TimeProvider timeProvider)
//    {
//        this.displayFormatter = displayFormatter;
//        this.timeProvider = timeProvider;
//    }


//    public NotificationListRequestData
//        NotificationListToRequestData(
//            Guid userId,
//            int page)
//    {
//        var result = new NotificationListRequestData
//        {
//            UserId = userId,
//            Page = page,
//            PageSize = DefaultPageSize
//        };

//        return result;
//    }

//    public NotificationListViewModel
//        NotificationListToViewModel(
//            PageResult<NotificationResponseModel>? page,
//            int requestedPage,
//            int unreadCount)
//    {
//        var result = new NotificationListViewModel
//        {
//            Notifications = page?.PageItems
//                .Select(NotificationToViewModel)
//                .ToList() ?? [],
//            UnreadCount = unreadCount,
//        };

//        return result;
//    }

//    public NotificationViewModel
//        NotificationToViewModel(NotificationResponseModel source)
//    {
//        var utc = DateTime.SpecifyKind(source.CreatedOn, DateTimeKind.Utc);
//        var local = TimeZoneInfo.ConvertTimeFromUtc(utc, timeProvider.LocalTimeZone);

//        var result = new NotificationViewModel
//        {
//            Id = source.Id,
//            Message = source.Message,
//            MatchId = source.MatchId,
//            IsRead = source.IsRead,
//            CreatedOn = displayFormatter.DateTime(local),
//            IconCss = source.Type switch
//            {
//                NotificationType.Promoted => "bi bi-arrow-up-circle text-success",
//                NotificationType.MatchConfirmed => "bi bi-check-circle text-primary",
//                NotificationType.MatchCancelled => "bi bi-x-circle text-danger",
//                NotificationType.RatingReceived => "bi bi-star text-warning",
//                NotificationType.StrikeAdded => "bi bi-exclamation-triangle text-danger",
//                _ => "bi bi-bell"
//            }
//        };

//        return result;
//    }
//}
