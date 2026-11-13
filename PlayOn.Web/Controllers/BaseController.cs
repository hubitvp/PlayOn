using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        protected BaseController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        protected Guid GetUserId()
        {
            string? userId = userManager.GetUserId(User);

            if (Guid.TryParse(userId, out var parsedUserId))
            {
                return parsedUserId;
            }

            return Guid.Empty;
        }

        protected bool IsUserAuthenticated()
        {
            return User.Identity?.IsAuthenticated ?? false;
        }

        protected bool IsAdmin()
        {
            return User.IsInRole(RoleNames.Administrator);
        }

        protected void AddServiceErrors(IEnumerable<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                var message = error.ErrorMessage ?? "Input data is not correct.";

                foreach (var member in error.MemberNames)
                {
                    ModelState.AddModelError(member, message);
                }
            }
        }
    }
}
