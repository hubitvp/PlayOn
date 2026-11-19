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

            var requestData = sportDataBuilder.SportQueryToRequestData(input);

            var response = await sportService.GetSportsAsync(requestData);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(sportDataBuilder.SportListToViewModel(null, input));
            }

            return View(sportDataBuilder.SportListToViewModel(response.ResponseData, input));
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

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(model);
            }

            TempData["Success"] = "The sport was created.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await sportService.GetForEditAsync(id);

            if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }

            return View(sportDataBuilder.SportToFormViewModel(response.ResponseData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AdminSportFormViewModel model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestData = sportDataBuilder.SportFormToUpdateRequestData(id, model);

            var response = await sportService.UpdateAsync(requestData);

            if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                return View(model);
            }

            TempData["Success"] = "The sport was updated.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await sportService.DeleteAsync(id);

            if (response.Status == ServiceStatus.NotFound)
            {
                return NotFound();
            }

            if (response.IsSuccess)
            {
                TempData["Success"] = "The sport was deleted.";
            }
            else
            {
                TempData["Error"] = "";//response.FirstErrorMessage();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
