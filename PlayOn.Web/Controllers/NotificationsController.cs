using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels;

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
        public async Task<IActionResult> Index([FromQuery] PageInputModel page)
        {
            var userId = GetUserId();

            var pageRequestData = notificationDataBuilder.PageInputToPageRequestData(page);

            var serviceResponse = await notificationService.GetMainResponseModelAsync(userId, pageRequestData);

            AddServiceErrors(serviceResponse.Errors);

            var viewModel = notificationDataBuilder
                .NotificationListToViewModel(serviceResponse.ResponseData);

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
