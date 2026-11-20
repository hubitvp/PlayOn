using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Notifications.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels.Notifications;
using System.Collections.Immutable;

namespace PlayOn.Web.DataBuilders
{
    public class NotificationDataBuilder : DataBuilder
    {
        private readonly IDisplayFormatter displayFormatter;

        public NotificationDataBuilder(
            IDisplayFormatter displayFormatter)
        {
            this.displayFormatter = displayFormatter;
        }

        //public NotificationRequestData
        //    NotificationListToRequestData(Guid userId)
        //{
        //    var result = new NotificationRequestData
        //    {
        //        UserId = userId,
        //    };

        //    return result;
        //}

        public NotificationListViewModel
            NotificationListToViewModel(
                NotificationResponseModel source)
        {
            var pageViewModel = new PageResult<NotificationViewModel>
            (
                pageItems: source.Page.PageItems.Select(NotificationToViewModel).ToImmutableList(),
                pageMark: source.Page.PageMark,
                pageSize: source.Page.PageSize,
                totalItemsCount: source.Page.TotalItemsCount
            );


            var result = new NotificationListViewModel
            {
                Page = pageViewModel,
                UnreadCount = source.UnreadCount,
            };

            return result;
        }


        public NotificationViewModel
            NotificationToViewModel(NotificationResponseModelData source)
        {
            var result = new NotificationViewModel
            {
                Id = source.Id,
                Message = source.Message,
                MatchId = source.MatchId,
                IsRead = source.IsRead,
                CreatedOn = displayFormatter.DateTime(source.CreatedOn),
                IconCss = source.Type switch
                {
                    NotificationType.Promoted => "bi bi-arrow-up-circle text-success",
                    NotificationType.MatchConfirmed => "bi bi-check-circle text-primary",
                    NotificationType.MatchCancelled => "bi bi-x-circle text-danger",
                    NotificationType.RatingReceived => "bi bi-star text-warning",
                    NotificationType.StrikeAdded => "bi bi-exclamation-triangle text-danger",
                    _ => "bi bi-bell"
                }
            };

            return result;
        }
    }
}