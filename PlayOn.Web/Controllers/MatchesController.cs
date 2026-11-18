using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels;
using PlayOn.Web.ViewModels.Match;

namespace PlayOn.Web.Controllers
{
    public class MatchesController : BaseController
    {
        private readonly IMatchService matchService;
        private readonly ISportService sportService;
        private readonly IAttendanceService attendanceService;
        private readonly MatchDataBuilder matchDataBuilder;

        public MatchesController(
            IMatchService matchService,
            ISportService sportService,
            IAttendanceService attendanceService,
            MatchDataBuilder matchDataBuilder,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.matchService = matchService;
            this.sportService = sportService;
            this.attendanceService = attendanceService;
            this.matchDataBuilder = matchDataBuilder;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index([FromQuery][Bind(Prefix = "")] MatchViewModel searchModel, [FromQuery] PageInputModel page)
        {
            if (!ModelState.IsValid)
            {
                var optionsResponse = await matchService.GetOptionsAsync(null);

                var errorViewModel = matchDataBuilder.ToViewModel(searchModel, null, optionsResponse.ResponseData);

                return View(errorViewModel);
            }

            var searchRequestData = matchDataBuilder
                .MatchSearchToRequestData(searchModel);

            var pageRequestData = matchDataBuilder
                .PageInputToPageRequestData(page);

            var serviceResponse = await matchService
                .GetMainResponseModelAsync(searchRequestData, pageRequestData);

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrors(serviceResponse.Errors);
            }

            var viewModel = matchDataBuilder.ToViewModel(searchModel, serviceResponse.ResponseData);

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Details(
                Guid id,
                string? slug)
        {
            var response = await matchService
                .GetDetailsAsync(
                    id,
                    IsUserAuthenticated() ? GetUserId() : null,
                    IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var canonicalSlug = Slug.Create(response.ResponseData.Title);

            if ((slug ?? string.Empty) != canonicalSlug)
            {
                return RedirectToAction(
                    nameof(Details),
                    new { id, slug = canonicalSlug });
            }

            var viewModel = matchDataBuilder
                .MatchDetailsToViewModel(response.ResponseData, canonicalSlug);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await matchService
                .GetForEditAsync(id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var viewModel = matchDataBuilder
                .MatchEditToViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(
                Guid id,
                MatchEditViewModel model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                return await EditViewWithDisplayDataAsync(model);
            }

            var requestData = matchDataBuilder
                .MatchEditToRequestData(id, GetUserId(), IsAdmin(), model);

            var response = await matchService.UpdateAsync(requestData);

            if (response.IsSuccess)
            {
                TempData["Success"] = "Match updated.";

                return RedirectToAction(
                    nameof(Details),
                    new { id, slug = Slug.Create(model.Title) });
            }

            if (response.Status != ServiceStatus.Invalid)
            {
                return FromFailure(response);
            }

            AddServiceErrors(response.Errors);

            return await EditViewWithDisplayDataAsync(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? venueId)
        {
            var optionsResponse = await matchService.GetFormOptionsAsync();

            if (!optionsResponse.IsSuccess)
            {
                return FromFailure(optionsResponse);
            }

            var viewModel = matchDataBuilder
                .MatchFormToViewModel(venueId, optionsResponse.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await CreateViewWithOptionsAsync(model);
            }

            var requestData = matchDataBuilder
                .MatchFormToRequestData(GetUserId(), model);

            var response = await matchService.CreateAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return await CreateViewWithOptionsAsync(model);
            }

            TempData["Success"] = "Your match is open. Other players can join now.";

            return RedirectToAction(
                nameof(Details),
                new { id = response.ResponseData, slug = Slug.Create(model.Title) });
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var response = await matchService
                .GetForCancelAsync(id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var viewModel = matchDataBuilder
                .MatchCancelToViewModel(id, response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Cancel(
                Guid id,
                MatchCancelViewModel model)
        {
            model.MatchId = id;

            if (ModelState.IsValid)
            {
                var requestData = matchDataBuilder
                    .MatchCancelToRequestData(id, GetUserId(), IsAdmin(), model);

                var response = await matchService.CancelAsync(requestData);

                if (response.IsSuccess)
                {
                    TempData["Success"] = "The match was cancelled and the players were notified.";

                    return RedirectToAction(nameof(Details), new { id });
                }

                if (response.Status != ServiceStatus.Invalid)
                {
                    return FromFailure(response);
                }

                AddServiceErrors(response.Errors);
            }

            var cancelData = await matchService
                .GetForCancelAsync(id, GetUserId(), IsAdmin());

            if (!cancelData.IsSuccess)
            {
                return FromFailure(cancelData);
            }

            matchDataBuilder.MatchCancelApplyDisplayData(model, cancelData.ResponseData);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Attendance(Guid id)
        {
            var response = await attendanceService
                .GetAsync(id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                return AttendanceFailure(response, id);
            }

            var viewModel = matchDataBuilder
                .AttendanceToViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Attendance(
                Guid id,
                AttendanceViewModel model)
        {
            model.MatchId = id;

            if (ModelState.IsValid)
            {
                var requestData = matchDataBuilder
                    .AttendanceToRequestData(id, GetUserId(), IsAdmin(), model);

                var response = await attendanceService.SubmitAsync(requestData);

                if (response.IsSuccess)
                {
                    TempData["Success"] = "Attendance saved. Players can now rate each other.";

                    return RedirectToAction(nameof(Details), new { id });
                }

                if (response.Status != ServiceStatus.Invalid)
                {
                    return AttendanceFailure(response, id);
                }

                AddServiceErrors(response.Errors);
            }

            var attendanceData = await attendanceService
                .GetAsync(id, GetUserId(), IsAdmin());

            if (!attendanceData.IsSuccess)
            {
                return AttendanceFailure(attendanceData, id);
            }

            matchDataBuilder.AttendanceApplyDisplayData(model, attendanceData.ResponseData);

            return View(model);
        }

        private async Task<IActionResult>
            CreateViewWithOptionsAsync(MatchFormViewModel model)
        {
            var optionsResponse = await matchService.GetFormOptionsAsync();

            if (!optionsResponse.IsSuccess)
            {
                return FromFailure(optionsResponse);
            }

            matchDataBuilder.MatchFormOptionsToViewModel(model, optionsResponse.ResponseData);

            return View(model);
        }

        private async Task<IActionResult>
            EditViewWithDisplayDataAsync(MatchEditViewModel model)
        {
            var response = await matchService
                .GetForEditAsync(model.Id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            matchDataBuilder.MatchEditApplyDisplayData(model, response.ResponseData);

            return View(model);
        }

        private IActionResult AttendanceFailure<T>(ServiceResponse<T> response, Guid matchId)
        {
            if (response.Status == ServiceStatus.Conflict)
            {
                TempData["Warning"] = "Warning: The action could not be completed due to a conflict.";

                return RedirectToAction(nameof(Details), new { id = matchId });
            }

            return FromFailure(response);
        }

        private IActionResult FromFailure<T>(ServiceResponse<T> response)
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