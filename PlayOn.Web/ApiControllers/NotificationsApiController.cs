
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Web.Controllers;

namespace PlayOn.Web.ApiControllers
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

            var response = await notificationService.GetUnreadCountAsync(userId);

            if (!response.IsSuccess)
            {
                return BadRequest();
            }

            return Ok(new
            {
                unread = response.ResponseData
            });
        }

    }
}
