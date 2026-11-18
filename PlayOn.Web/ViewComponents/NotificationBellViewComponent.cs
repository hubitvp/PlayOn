//using Microsoft.AspNetCore.Mvc;
//using PlayOn.Services.Contracts;
//using System.Security.Claims;

//namespace PlayOn.Web.ViewComponents
//{
//    public class NotificationBellViewComponent : ViewComponent
//    {
//        private readonly INotificationService notificationService;

//        public NotificationBellViewComponent(INotificationService notificationService)
//        {
//            this.notificationService = notificationService;
//        }

//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var hasUserId = Guid.TryParse(
//                UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
//                out var userId);

//            var unreadCount = hasUserId
//                ? await notificationService.GetUnreadCountAsync(userId)
//                : 0;

//            return View(unreadCount);
//        }
//    }
//}
