using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
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
            if (!ModelState.IsValid)
            {
                return await ReturnInvalidIndexViewAsync(page);
            }

            var userId = GetUserId();
            var pageRequestData = notificationDataBuilder.PageInputToPageRequestData(page);
            var serviceResponse = await notificationService.GetMainResponseModelAsync(userId, pageRequestData);

            if (serviceResponse.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(serviceResponse.Errors);
                return await ReturnInvalidIndexViewAsync(page);
            }

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = notificationDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

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
                var errorViewModel = notificationDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
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
            var response = await notificationService.MarkAllAsReadAsync(GetUserId());

            if (!response.IsSuccess)
            {
                var errorViewModel = notificationDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> ReturnInvalidIndexViewAsync(PageInputModel page)
        {
            var userId = GetUserId();
            var pageRequestData = notificationDataBuilder.PageInputToPageRequestData(page);
            var serviceResponse = await notificationService.GetMainResponseModelAsync(userId, pageRequestData);

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = notificationDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = notificationDataBuilder
                .NotificationListToViewModel(serviceResponse.ResponseData);

            return View(viewModel);
        }
    }
}