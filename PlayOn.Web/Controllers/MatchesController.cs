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

                if (optionsResponse.Status != ServiceStatus.Success)
                {
                    var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(optionsResponse.Status);
                    return View("Error", errorViewModel);
                }

                var invalidViewModel = matchDataBuilder.ToViewModel(model, null, optionsResponse.ResponseData);
                return View(invalidViewModel);
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
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
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
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
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
                return await ReturnInvalidEditViewAsync(model);
            }

            var requestData = matchDataBuilder
                .MatchEditToRequestData(model.MatchId, GetUserId(), IsAdmin(), model);

            var serviceResponse = await matchService.UpdateAsync(requestData);

            if (serviceResponse.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(serviceResponse.Errors);
                return await ReturnInvalidEditViewAsync(model);
            }

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "Match updated.";

            return RedirectToAction(
                nameof(Details),
                new { id = model.MatchId, slug = Slug.Create(model.Title) });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var optionsResponse = await matchService.GetOptionsAsync(null);

            if (!optionsResponse.IsSuccess)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(optionsResponse.Status);
                return View("Error", errorViewModel);
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
                return await ReturnInvalidCreateViewAsync(model);
            }

            var requestData = matchDataBuilder
                .MatchCreateInputToRequestData(GetUserId(), model);

            var serviceResponse = await matchService.CreateAsync(requestData);

            if (serviceResponse.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(serviceResponse.Errors);
                return await ReturnInvalidCreateViewAsync(model);
            }

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
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
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
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

            if (!ModelState.IsValid)
            {
                return await ReturnInvalidCancelViewAsync(model);
            }

            var requestData = matchDataBuilder
                .MatchCancelToRequestData(model.MatchId, GetUserId(), IsAdmin(), model);

            var serviceResponse = await matchService.CancelAsync(requestData);

            if (serviceResponse.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(serviceResponse.Errors);
                return await ReturnInvalidCancelViewAsync(model);
            }

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "The match was cancelled and the players were notified.";

            return RedirectToAction(nameof(Details), new { id = model.MatchId });
        }

        [HttpGet]
        public async Task<IActionResult> Attendance(Guid id)
        {
            var response = await attendanceService
                .GetAsync(id, GetUserId(), IsAdmin());

            if (!response.IsSuccess)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
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
                MatchAttendanceViewModel model)
        {
            model.MatchId = id;

            if (!ModelState.IsValid)
            {
                return await ReturnInvalidAttendanceViewAsync(model);
            }

            var requestData = matchDataBuilder
                .AttendanceToRequestData(model.MatchId, GetUserId(), IsAdmin(), model);

            var serviceResponse = await attendanceService.SubmitAsync(requestData);

            if (serviceResponse.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(serviceResponse.Errors);
                return await ReturnInvalidAttendanceViewAsync(model);
            }

            if (serviceResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(serviceResponse.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "Attendance saved. Players can now rate each other.";

            return RedirectToAction(nameof(Details), new { id = model.MatchId });
        }

        private async Task<IActionResult> ReturnInvalidEditViewAsync(MatchEditViewModel model)
        {
            var displayData = await matchService.GetEditDisplayDataAsync(model.MatchId, GetUserId(), IsAdmin());

            if (displayData.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(displayData.Status);
                return View("Error", errorViewModel);
            }

            matchDataBuilder.MatchEditApplyDisplayData(model, displayData.ResponseData);

            return View(model);
        }

        private async Task<IActionResult> ReturnInvalidCreateViewAsync(MatchCreateInputModel model)
        {
            var optionsResponse = await matchService.GetOptionsAsync(null);

            if (optionsResponse.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(optionsResponse.Status);
                return View("Error", errorViewModel);
            }

            matchDataBuilder.EmbedOptionsToViewModel(model, optionsResponse.ResponseData);

            return View(model);
        }

        private async Task<IActionResult> ReturnInvalidCancelViewAsync(MatchCancelViewModel model)
        {
            var cancelData = await matchService.GetForCancelAsync(model.MatchId, GetUserId(), IsAdmin());

            if (cancelData.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(cancelData.Status);
                return View("Error", errorViewModel);
            }

            matchDataBuilder.MatchCancelApplyDisplayData(model, cancelData.ResponseData);

            return View(model);
        }

        private async Task<IActionResult> ReturnInvalidAttendanceViewAsync(MatchAttendanceViewModel model)
        {
            var attendanceData = await attendanceService.GetAsync(model.MatchId, GetUserId(), IsAdmin());

            if (attendanceData.Status != ServiceStatus.Success)
            {
                var errorViewModel = matchDataBuilder.ServiceStatusToErrorViewModel(attendanceData.Status);
                return View("Error", errorViewModel);
            }

            matchDataBuilder.AttendanceApplyDisplayData(model, attendanceData.ResponseData);

            return View(model);
        }
    }
}