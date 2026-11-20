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
            Index([FromQuery] MatchViewModel model, [FromQuery] PageInputModel page)
        {
            if (!ModelState.IsValid)
            {
                var optionsResponse = await matchService.GetOptionsAsync(null);

                var errorViewModel = matchDataBuilder.ToViewModel(model, null, optionsResponse.ResponseData);

                return View(errorViewModel);
            }

            var searchRequestData = matchDataBuilder
                .MatchSearchToRequestData(model);

            var pageRequestData = matchDataBuilder
                .PageInputToPageRequestData(page);

            var serviceResponse = await matchService
                .GetMainResponseModelAsync(searchRequestData, pageRequestData);

            AddServiceErrors(serviceResponse.Errors);

            var viewModel = matchDataBuilder.ToViewModel(model, serviceResponse.ResponseData);

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
                return GetFailStatusCode(response);
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
                return GetFailStatusCode(response);
            }

            var viewModel = matchDataBuilder
                .MatchEditToViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MatchEditViewModel model)
        {
            model.MatchId = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = matchDataBuilder
                .MatchEditToRequestData(model.MatchId, GetUserId(), IsAdmin(), model);

            var serviceResponse = await matchService.UpdateAsync(requestData);


            if (serviceResponse.IsSuccess)
            {
                TempData["Success"] = "Match updated.";

                return RedirectToAction(
                    nameof(Details),
                    new { id = model.MatchId, slug = Slug.Create(model.Title) });
            }

            AddServiceErrors(serviceResponse.Errors);
            return GetFailStatusCode(serviceResponse);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var optionsResponse = await matchService.GetOptionsAsync(null);

            if (!optionsResponse.IsSuccess)
            {
                return GetFailStatusCode(optionsResponse);
            }

            var viewModel = matchDataBuilder
                .ToViewModel(optionsResponse.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchCreateInputModel model)
        {

            if (!ModelState.IsValid)
            {
                var optionsResponse = await matchService.GetOptionsAsync(null);

                matchDataBuilder.EmbedOptionsToViewModel(model, optionsResponse.ResponseData);

                return View(model);
            }

            var requestData = matchDataBuilder
                .MatchCreateInputToRequestData(GetUserId(), model);

            var serviceResponse = await matchService.CreateAsync(requestData);

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrors(serviceResponse.Errors);

                return GetFailStatusCode(serviceResponse);
            }

            TempData["Success"] = "Your match is open. Other players can join now.";

            return RedirectToAction(
                nameof(Details),
                new { id = serviceResponse.ResponseData, slug = Slug.Create(model.Title) });
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var serviceResponse = await matchService
                .GetForCancelAsync(id, GetUserId(), IsAdmin());

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrors(serviceResponse.Errors);
                return GetFailStatusCode(serviceResponse);
            }

            var viewModel = matchDataBuilder
                .MatchCancelToViewModel(id, serviceResponse.ResponseData);

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
                    .MatchCancelToRequestData(model.MatchId, GetUserId(), IsAdmin(), model);

                var response = await matchService.CancelAsync(requestData);

                if (response.IsSuccess)
                {
                    TempData["Success"] = "The match was cancelled and the players were notified.";

                    return RedirectToAction(nameof(Details), new { id = model.MatchId });
                }

                if (response.Status != ServiceStatus.Invalid)
                {
                    return GetFailStatusCode(response);
                }

                AddServiceErrors(response.Errors);
            }

            var cancelData = await matchService
                .GetForCancelAsync(model.MatchId, GetUserId(), IsAdmin());

            if (!cancelData.IsSuccess)
            {
                return GetFailStatusCode(cancelData);
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


        private IActionResult AttendanceFailure<T>(ServiceResponse<T> response, Guid matchId)
        {
            if (response.Status == ServiceStatus.Conflict)
            {
                TempData["Warning"] = "Warning: The action could not be completed due to a conflict.";

                return RedirectToAction(nameof(Details), new { id = matchId });
            }

            return GetFailStatusCode(response);
        }

    }
}