using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.ApplicationConstants;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PlayOn.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Administrator)]
    public abstract class AdminBaseController : Controller
    {
        protected Guid GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userId, out var parsed) ? parsed : Guid.Empty;
        }

        protected void AddServiceErrors(IEnumerable<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                var message = error.ErrorMessage ?? "Input data is not correct.";

                var members = error.MemberNames.Any() ? error.MemberNames : [string.Empty];

                foreach (var member in members)
                {
                    ModelState.AddModelError(member, message);
                }
            }
        }
    }
}