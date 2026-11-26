
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Web.ViewModels.Error;

namespace PlayOn.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [HttpGet]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Index(int statusCode = StatusCodes.Status500InternalServerError)
        {
            return statusCode switch
            {
                StatusCodes.Status404NotFound =>
                    NotFoundError(),

                StatusCodes.Status500InternalServerError =>
                    InternalServerError(),

                _ => OtherError(statusCode)
            };
        }

        [HttpGet]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Default()
        {
            return InternalServerError();
        }

        private IActionResult NotFoundError()
        {
            Response.StatusCode = StatusCodes.Status404NotFound;

            var model = new ErrorViewModel
            {
                Code = StatusCodes.Status404NotFound,
                Title = "The play has moved",
                Message = "We searched the whole field, but the page you requested is not here."
            };

            return View("NotFoundError404", model);
        }

        private IActionResult InternalServerError()
        {
            Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            var model = new ErrorViewModel
            {
                Code = StatusCodes.Status500InternalServerError,
                Title = "The scoreboard is offline",
                Message = "Something went wrong on our side. Please try again in a moment."
            };

            return View("InternalError500", model);
        }

        private IActionResult OtherError(int statusCode)
        {
            Response.StatusCode = statusCode;

            var model = new ErrorViewModel
            {
                Code = statusCode,
                Title = "Error",
                Message = "An error occurred while processing your request."
            };

            return View("Error", model);
        }
    }
}
