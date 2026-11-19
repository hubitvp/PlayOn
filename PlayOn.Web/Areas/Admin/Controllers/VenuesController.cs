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

            var requestData = venueDataBuilder.VenueQueryToRequestData(input);

            var response = await venueService.GetVenuesAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(venueDataBuilder.VenueListToViewModel(null, input));
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

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(model);
            }

            TempData["Success"] = "The venue slot was created.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await venueService.GetForEditAsync(id);

            if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }

            return View(venueDataBuilder.VenueToFormViewModel(response.ResponseData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AdminVenueFormViewModel model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = venueDataBuilder.VenueFormToUpdateRequestData(id, model);

            var response = await venueService.UpdateAsync(requestData);

            if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(model);
            }

            TempData["Success"] = "The venue slot was updated.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await venueService.DeleteAsync(id);

            return HandleToggle(response, "The venue slot was deleted.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var response = await venueService.RestoreAsync(id);

            return HandleToggle(response, "The venue slot was restored.");
        }

        private IActionResult HandleToggle(ServiceResponse<Guid> response, string successMessage)
        {
            if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }

            if (response.IsSuccess)
            {
                TempData["Success"] = successMessage;
            }
            else
            {
                TempData["Error"] = "";// response.FirstErrorMessage();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
