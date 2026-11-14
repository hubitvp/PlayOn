using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Web.DataBuilders;

namespace PlayOn.Web.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly INotificationService notificationService;
        private readonly NotificationDataBuilder notificationDataBuilder;

        public NotificationsController(
            INotificationService notificationService,
            NotificationDataBuilder notificationDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.notificationService = notificationService;
            this.notificationDataBuilder = notificationDataBuilder;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            page = Math.Max(1, page);

            var userId = GetUserId();

            var requestData = notificationDataBuilder
                .NotificationListToRequestData(userId, page);

            var response = await notificationService.GetNotificationsAsync(requestData);
            var unreadCount = await notificationService.GetUnreadCountAsync(userId);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(notificationDataBuilder
                    .NotificationListToViewModel(null, page, unreadCount));
            }

            var viewModel = notificationDataBuilder
                .NotificationListToViewModel(response.ResponseData, page, unreadCount);

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Read(Guid id)
        {
            var response = await notificationService.MarkAsReadAsync(GetUserId(), id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            if (response.ResponseData.MatchId is Guid matchId)
            {
                return RedirectToAction("Details", "Matches", new { id = matchId });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadAll()
        {
            await notificationService.MarkAllAsReadAsync(GetUserId());

            return RedirectToAction(nameof(Index));
        }
    }
}
