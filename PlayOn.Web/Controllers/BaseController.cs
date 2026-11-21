using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Data.Models;
using PlayOn.Services.Data;
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

        protected virtual Guid GetUserId()
        {
            string? userId = userManager.GetUserId(User);

            if (Guid.TryParse(userId, out var parsedUserId))
            {
                return parsedUserId;
            }

            return Guid.Empty;
        }

        protected virtual bool IsUserAuthenticated()
        {
            return User.Identity?.IsAuthenticated ?? false;
        }

        protected virtual bool IsAdmin()
        {
            return User.IsInRole(RoleNames.Administrator);
        }

        protected virtual void AddServiceErrors(IEnumerable<ValidationResult> errors)
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


        protected virtual IActionResult GetFailStatusCode<T>(ServiceResponse<T> response)
        {
            switch (response.Status)
            {
                case ServiceStatus.NotFound:
                    {
                        return NotFound();
                    }
                case ServiceStatus.Forbidden:
                    {
                        return Forbid();
                    }
                case ServiceStatus.Conflict:
                    {
                        var message = response.Errors
                                .FirstOrDefault()?
                                .ErrorMessage
                                ?? "This action is not possible right now.";

                        var problemDetails = new ProblemDetails
                        {
                            Title = "Operation conflict",
                            Detail = message,
                            Status = StatusCodes.Status409Conflict,
                            Instance = HttpContext.Request.Path
                        };

                        return Conflict(problemDetails);
                    }
                default:
                    {
                        return BadRequest();
                    }
            }
        }
    }
}
