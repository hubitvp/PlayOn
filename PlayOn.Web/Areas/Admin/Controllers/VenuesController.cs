using Microsoft.AspNetCore.Mvc;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.ViewModels.Admin;

namespace PlayOn.Web.Areas.Admin.Controllers
{
    public class VenuesController : AdminBaseController
    {
        private readonly IAdminVenueService venueService;
        private readonly AdminVenueDataBuilder venueDataBuilder;

        public VenuesController(
            IAdminVenueService venueService,
            AdminVenueDataBuilder venueDataBuilder)
        {
            this.venueService = venueService;
            this.venueDataBuilder = venueDataBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AdminVenueSearchInputModel input)
        {
            input.Page = Math.Max(1, input.Page);

            if (!ModelState.IsValid)
            {
                return await ReturnInvalidIndexViewAsync(input);
            }

            var requestData = venueDataBuilder.VenueQueryToRequestData(input);

            var response = await venueService.GetVenuesAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return await ReturnInvalidIndexViewAsync(input);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            return View(venueDataBuilder.VenueListToViewModel(response.ResponseData, input));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(venueDataBuilder.NewVenueForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminVenueFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = venueDataBuilder.VenueFormToCreateRequestData(model);

            var response = await venueService.CreateAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return View(model);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "The venue slot was created.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = venueDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var response = await venueService.GetForEditAsync(id);

            if (!response.IsSuccess)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            return View(venueDataBuilder.VenueToFormViewModel(response.ResponseData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AdminVenueFormViewModel model)
        {
            model.Id = id;

            if (id == Guid.Empty)
            {
                var emptyIdError = venueDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = venueDataBuilder.VenueFormToUpdateRequestData(id, model);

            var response = await venueService.UpdateAsync(requestData);

            if (response.Status == ServiceStatus.InvalidInput)
            {
                AddServiceErrors(response.Errors);
                return View(model);
            }

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = "The venue slot was updated.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = venueDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var response = await venueService.DeleteAsync(id);

            return HandleToggle(response, "The venue slot was deleted.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            if (id == Guid.Empty)
            {
                var emptyIdError = venueDataBuilder.ServiceStatusToErrorViewModel(ServiceStatus.NotFound);
                return View("Error", emptyIdError);
            }

            var response = await venueService.RestoreAsync(id);

            return HandleToggle(response, "The venue slot was restored.");
        }

        private async Task<IActionResult> ReturnInvalidIndexViewAsync(AdminVenueSearchInputModel input)
        {
            var defaultInput = new AdminVenueSearchInputModel { Page = 1 };
            var requestData = venueDataBuilder.VenueQueryToRequestData(defaultInput);

            var response = await venueService.GetVenuesAsync(requestData);

            if (response.Status != ServiceStatus.Success)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            var viewModel = venueDataBuilder.VenueListToViewModel(response.ResponseData, defaultInput);

            return View(nameof(Index), viewModel);
        }

        private IActionResult HandleToggle(ServiceResponse<Guid> response, string successMessage)
        {
            if (!response.IsSuccess)
            {
                var errorViewModel = venueDataBuilder.ServiceStatusToErrorViewModel(response.Status);
                return View("Error", errorViewModel);
            }

            TempData["Success"] = successMessage;

            return RedirectToAction(nameof(Index));
        }
    }
}