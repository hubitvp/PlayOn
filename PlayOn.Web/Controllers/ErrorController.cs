using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Web.ViewModels.Error;

namespace PlayOn.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int statusCode)
        {
            Response.StatusCode = statusCode;

            switch (statusCode)
            {
                case 404:
                    {
                        var model = new ErrorPageViewModel
                        {
                            Code = 404,
                            Title = "The play has moved",
                            Message = "We searched the whole field, but the page you requested is not here."
                        };

                        return View("NotFoundError404", model);
                    }

                case 500:
                    {
                        var model = new ErrorPageViewModel
                        {
                            Code = 500,
                            Title = "The scoreboard is offline",
                            Message = "Something went wrong on our side. Please try again in a moment."
                        };

                        return View("InternalError500", model);
                    }

                default:
                    return View("Error", new ErrorPageViewModel
                    {
                        Code = statusCode,
                        Title = "Error",
                        Message = "An error occurred while processing your request."
                    });
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Default()
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;

            return View("Error", new ErrorPageViewModel
            {
                Title = "Game over",
                Message = "The play could not be completed. Please try again or return to the home page."
            });
        }
    }
}
