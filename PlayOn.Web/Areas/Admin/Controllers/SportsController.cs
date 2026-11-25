using Microsoft.AspNetCore.Mvc;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels.Admin;

namespace PlayOn.Web.Areas.Admin.Controllers
{
    public class SportsController : AdminBaseController
    {
        private readonly IAdminSportService sportService;
        private readonly AdminSportDataBuilder sportDataBuilder;

        public SportsController(
            IAdminSportService sportService,
            AdminSportDataBuilder sportDataBuilder)
        {
            this.sportService = sportService;
            this.sportDataBuilder = sportDataBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AdminSportSearchInputModel input)
        {
            input.Page = Math.Max(1, input.Page);

            if (!ModelState.IsValid)
            {
                return await ReturnInvalidIndexViewAsync(input);
            }

            var requestData = sportDataBuilder.SportQueryToRequestData(input);

            var response = await sportService.GetSportsAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return await ReturnInvalidIndexViewAsync(input);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = sportDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = sportDataBuilder.SportListToViewModel(response.ResponseData, input);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AdminSportFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminSportFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = sportDataBuilder.SportFormToCreateRequestData(model);

            var response = await sportService.CreateAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return View(model);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = sportDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "The sport was created.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = sportDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var response = await sportService.GetForEditAsync(id);

            if (!response.IsSuccess)
            {
                var errorViewModel = sportDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = sportDataBuilder.SportToFormViewModel(response.ResponseData);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AdminSportFormViewModel model)
        {
            model.Id = id;

            if (id == Guid.Empty)
            {
                var emptyIdError = sportDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = sportDataBuilder.SportFormToUpdateRequestData(id, model);

            var response = await sportService.UpdateAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return View(model);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = sportDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "The sport was updated.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = sportDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var response = await sportService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                var errorViewModel = sportDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "The sport was deleted.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> ReturnInvalidIndexViewAsync(AdminSportSearchInputModel input)
        {
            var defaultInput = new AdminSportSearchInputModel { Page = 1 };
            var requestData = sportDataBuilder.SportQueryToRequestData(defaultInput);

            var response = await sportService.GetSportsAsync(requestData);

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = sportDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = sportDataBuilder.SportListToViewModel(response.ResponseData, defaultInput);

            return View(nameof(Index), viewModel);
        }
    }
}