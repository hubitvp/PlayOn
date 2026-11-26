using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;

namespace PlayOn.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsApiController : BaseController
    {
        private readonly INotificationService notificationService;

        public NotificationsApiController(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            var userId = GetUserId();

            var response = await notificationService.GetMainResponseModelAsync(
                userId,
                new PageRequestData { Page = 1, PageSize = 5 });

            if (!response.IsSuccess)
            {
                return BadRequest();
            }

            var items = response.ResponseData.Page.PageItems
                .Select(n => new
                {
                    title = (string?)null,
                    message = n.Message,
                    link = n.MatchId.HasValue
                        ? Url.Action("Details", "Matches", new { id = n.MatchId.Value })
                        : Url.Action("Index", "Notifications"),
                    isRead = n.IsRead
                });

            return Ok(new
            {
                unread = response.ResponseData.UnreadCount,
                items
            });
        }

        [HttpPost("read")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllRead()
        {
            var response = await notificationService.MarkAllAsReadAsync(GetUserId());

            if (!response.IsSuccess)
            {
                return BadRequest();
            }

            return Ok(new { marked = response.ResponseData });
        }
    }
}
