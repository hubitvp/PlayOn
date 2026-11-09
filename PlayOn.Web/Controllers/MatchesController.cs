using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Matches.RequestData;
using PlayOn.Web.Mappers;
using PlayOn.Web.ViewModels.Match;
using System.Globalization;

namespace PlayOn.Web.Controllers
{
    [AllowAnonymous]
    public class MatchesController : BaseController
    {
        private readonly IMatchService matchService;

        private readonly ISportService sportService;

        public MatchesController(
            IMatchService matchService,
            ISportService sportService,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.matchService = matchService;
            this.sportService = sportService;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Index(
                [FromQuery] MatchListInputModel input)
        {
            if (!ModelState.IsValid)
            {
                var errorViewModel =
                    await BuildListViewModelAsync(input, null);

                return View(errorViewModel);
            }

            var request = MatchesMapper.ToMatchListRequestData(input);

            var response = await matchService
                .GetMatchCardsAsync(request);

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);

                var errorViewModel =
                    await BuildListViewModelAsync(input, null);

                return View(errorViewModel);
            }

            var viewModel =
                await BuildListViewModelAsync(input, response.ResponseData);

            return View(viewModel);
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult>
            Details(
                int id,
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

            var data = response.ResponseData;
            var canonicalSlug = SlugHelper.Create(data.Title);

            if ((slug ?? string.Empty) != canonicalSlug)
            {
                return RedirectToAction(
                    nameof(Details),
                    new { id, slug = canonicalSlug });
            }

            var viewModel = MatchesMapper
                .ToMatchDetailsViewModel(data, canonicalSlug);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await matchService
                .GetForEditAsync(
                    id,
                    GetUserId(),
                    IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var data = response.ResponseData;

            var model = MatchesMapper.ToMatchEditViewModel(data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>
            Edit(
                int id,
                MatchEditViewModel model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                var serviceResponse = await matchService
                    .GetForEditAsync(
                        model.Id,
                        GetUserId(),
                        IsAdmin());

                if (!serviceResponse.IsSuccess)
                {
                    return FromFailure(serviceResponse);
                }

                var editModel = MatchesMapper
                    .ToMatchEditViewModel(serviceResponse.ResponseData);

                model.SportName = editModel.SportName;
                model.SportIcon = editModel.SportIcon;
                model.CourtName = editModel.CourtName;
                model.CourtCapacity = editModel.CourtCapacity;
                model.VenueName = editModel.VenueName;
                model.City = editModel.City;
                model.StartsAt = editModel.StartsAt;
                model.JoinedCount = editModel.JoinedCount;

                return View(model);
            }

            var response = await matchService.UpdateAsync(new MatchUpdateRequestData
            {
                MatchId = id,
                ActorId = GetUserId(),
                ActorIsAdmin = IsAdmin(),
                RowVersion = model.RowVersion,
                Title = model.Title,
                Description = model.Description,
                DurationMinutes = model.DurationMinutes,
                MinPlayers = model.MinPlayers,
                MaxPlayers = model.MaxPlayers,
                MinSkill = model.MinSkill,
                MaxSkill = model.MaxSkill,
                PricePerPlayer = model.PricePerPlayer
            });

            if (response.IsSuccess)
            {
                TempData["Success"] = "Match updated.";
                return RedirectToAction(nameof(Details), new { id, slug = SlugHelper.Create(model.Title) });
            }

            if (response.Status != ServiceStatus.Invalid)
            {
                return FromFailure(response);
            }

            AddServiceErrors(response.Errors);


            var databaseMatch = await matchService.GetForEditAsync(model.Id, GetUserId(), IsAdmin());

            if (!databaseMatch.IsSuccess)
            {
                return FromFailure(databaseMatch);
            }

            model.SportName = databaseMatch.ResponseData.SportName;
            model.SportIcon = databaseMatch.ResponseData.SportIcon.ToCssClass();
            model.CourtName = databaseMatch.ResponseData.CourtName;
            model.CourtCapacity = databaseMatch.ResponseData.CourtCapacity;
            model.VenueName = databaseMatch.ResponseData.VenueName;
            model.City = databaseMatch.ResponseData.City;
            model.StartsAt = databaseMatch.ResponseData.Date.ToDateTime(databaseMatch.ResponseData.StartsAt)
                .ToString("dddd, dd MMMM yyyy, HH:mm", CultureInfo.InvariantCulture);
            model.JoinedCount = databaseMatch.ResponseData.JoinedCount;


            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int? venueId)
        {
            var model = new MatchFormViewModel();

            if (venueId.HasValue)
            {
                model.VenueId = venueId.Value;
            }


            var options = await matchService.GetFormOptionsAsync();

            model.Sports = options.Sports
                .Select(x => new SelectListItem(x.Name, x.Id.ToString(), x.Id == model.SportId))
                .ToList();

            model.Courts = options.Courts
                .Select(x => new CourtOptionViewModel
                {
                    CourtId = x.CourtId,
                    Name = x.Name,
                    Surface = x.Surface.ToString(),
                    Capacity = x.Capacity,
                    SportIds = x.SportIds.ToArray()
                })
                .ToList();

            model.Slots = options.Slots
                .Select(x => new VenueSlotOptionViewModel
                {
                    Id = x.Id,
                    Label = string.Create(CultureInfo.InvariantCulture,
                        $"{x.Date.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture)}, {x.StartsAt.ToString("HH:mm", CultureInfo.InvariantCulture)} - {x.Name}, {x.City}"),
                    Date = x.Date.ToString("dddd, dd MMMM yyyy", CultureInfo.InvariantCulture),
                    StartMinutes = x.StartsAt.Hour * 60 + x.StartsAt.Minute,
                    CloseMinutes = x.ClosesAt.Hour * 60 + x.ClosesAt.Minute
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MatchFormViewModel model)
        {
            if (!ModelState.IsValid)
            {

                var options = await matchService.GetFormOptionsAsync();

                model.Sports = options.Sports
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString(), x.Id == model.SportId))
                    .ToList();

                model.Courts = options.Courts
                    .Select(x => new CourtOptionViewModel
                    {
                        CourtId = x.CourtId,
                        Name = x.Name,
                        Surface = x.Surface.ToString(),
                        Capacity = x.Capacity,
                        SportIds = x.SportIds.ToArray()
                    })
                    .ToList();

                model.Slots = options.Slots
                    .Select(x => new VenueSlotOptionViewModel
                    {
                        Id = x.Id,
                        Label = string.Create(CultureInfo.InvariantCulture,
                            $"{x.Date.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture)}, {x.StartsAt.ToString("HH:mm", CultureInfo.InvariantCulture)} - {x.Name}, {x.City}"),
                        Date = x.Date.ToString("dddd, dd MMMM yyyy", CultureInfo.InvariantCulture),
                        StartMinutes = x.StartsAt.Hour * 60 + x.StartsAt.Minute,
                        CloseMinutes = x.ClosesAt.Hour * 60 + x.ClosesAt.Minute
                    })
                    .ToList();
                return View(model);
            }

            var response = await matchService
                .CreateAsync(new MatchCreateRequestData
                {
                    OrganiserId = GetUserId(),
                    SportId = model.SportId,
                    CourtId = model.CourtId,
                    VenueId = model.VenueId,
                    Title = model.Title,
                    Description = model.Description,
                    DurationMinutes = model.DurationMinutes,
                    MinPlayers = model.MinPlayers,
                    MaxPlayers = model.MaxPlayers,
                    MinSkill = model.MinSkill,
                    MaxSkill = model.MaxSkill,
                    PricePerPlayer = model.PricePerPlayer
                });

            if (!response.IsSuccess)
            {
                AddServiceErrors(response.Errors);
                var options = await matchService.GetFormOptionsAsync();

                model.Sports = options.Sports
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString(), x.Id == model.SportId))
                    .ToList();

                model.Courts = options.Courts
                    .Select(x => new CourtOptionViewModel
                    {
                        CourtId = x.CourtId,
                        Name = x.Name,
                        Surface = x.Surface.ToString(),
                        Capacity = x.Capacity,
                        SportIds = x.SportIds.ToArray()
                    })
                    .ToList();

                model.Slots = options.Slots
                    .Select(x => new VenueSlotOptionViewModel
                    {
                        Id = x.Id,
                        Label = string.Create(CultureInfo.InvariantCulture,
                            $"{x.Date.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture)}, {x.StartsAt.ToString("HH:mm", CultureInfo.InvariantCulture)} - {x.Name}, {x.City}"),
                        Date = x.Date.ToString("dddd, dd MMMM yyyy", CultureInfo.InvariantCulture),
                        StartMinutes = x.StartsAt.Hour * 60 + x.StartsAt.Minute,
                        CloseMinutes = x.ClosesAt.Hour * 60 + x.ClosesAt.Minute
                    })
                    .ToList();
                return View(model);
            }

            TempData["Success"] = "Your match is open. Other players can join now.";

            return RedirectToAction(nameof(Details), new { id = response.ResponseData, slug = SlugHelper.Create(model.Title) });
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            var response = await matchService
                .GetForCancelAsync(
                    id,
                    GetUserId(),
                    IsAdmin());

            if (!response.IsSuccess)
            {
                return FromFailure(response);
            }

            var model = new MatchCancelViewModel { Id = id };

            model.Title = response.ResponseData.Title;
            model.VenueName = response.ResponseData.VenueName;
            model.StartsAt = response.ResponseData.Date.ToDateTime(response.ResponseData.StartsAt)
                .ToString("dddd, dd MMMM yyyy, HH:mm", CultureInfo.InvariantCulture);
            model.JoinedCount = response.ResponseData.JoinedCount;

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Cancel(int id, MatchCancelViewModel model)
        {
            model.Id = id;

            if (ModelState.IsValid)
            {
                var response = await matchService.CancelAsync(new MatchCancelRequestData
                {
                    MatchId = id,
                    ActorId = GetUserId(),
                    ActorIsAdmin = IsAdmin(),
                    Reason = model.Reason
                });

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

            var serviceResponse = await matchService.GetForCancelAsync(id, GetUserId(), IsAdmin());

            if (!serviceResponse.IsSuccess)
            {
                return FromFailure(serviceResponse);
            }

            model.Title = serviceResponse.ResponseData.Title;
            model.VenueName = serviceResponse.ResponseData.VenueName;
            model.StartsAt = serviceResponse.ResponseData.Date.ToDateTime(serviceResponse.ResponseData.StartsAt)
                .ToString("dddd, dd MMMM yyyy, HH:mm", CultureInfo.InvariantCulture);
            model.JoinedCount = serviceResponse.ResponseData.JoinedCount;
            return View(model);
        }


        private async Task<MatchListViewModel>
            BuildListViewModelAsync(
                MatchListInputModel input,
                PagedResult<MatchCardResponseModel>? page)
        {
            var sports = (await sportService.GetSportsAsync()).ResponseData;

            var response = MatchesMapper.ToMatchListViewModel(input, page, sports);
            return response;
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

                        var result = Conflict(problemDetails);

                        return result;
                    }
                default:
                    {
                        return BadRequest();
                    }
            }
        }
    }
}
