using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Web.Filters;
using PlayOn.Web.ViewModels;
using PlayOn.Web.ViewModels.Home;
using PlayOn.Web.ViewModels.Match;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;

namespace PlayOn.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService service;

        public HomeController(IHomeService service)
        {
            this.service = service;
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] HomeSearchInputModel searchQuery)
        {
            if (!ModelState.IsValid)
            {
                return View(await BuildViewModelAsync(searchQuery));
            }

            var serviceResponse = await service.GetMatchCardsAsync(MapToQueryRequest(searchQuery));

            if (!serviceResponse.IsSuccess)
            {
                AddServiceErrorsToModelState(serviceResponse.Errors);
                return View(await BuildViewModelAsync(searchQuery));
            }

            return View(await BuildViewModelAsync(searchQuery, serviceResponse.ResponseData));
        }

        private static HomeQueryRequestData MapToQueryRequest(HomeSearchInputModel searchQuery)
        {
            return new HomeQueryRequestData
            {
                SportId = searchQuery.SportId,
                City = searchQuery.City,
                From = searchQuery.From != null ? DateOnly.Parse(searchQuery.From) : null
            };
        }

        private void AddServiceErrorsToModelState(IEnumerable<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                foreach (var memberName in error.MemberNames)
                {
                    ModelState.AddModelError(
                        MapToViewModelProperty(memberName),
                        error.ErrorMessage ?? "Input data is not correct");
                }
            }
        }

        private static string MapToViewModelProperty(string memberName)
        {
            return memberName switch
            {
                nameof(HomeQueryRequestData.SportId) => nameof(HomeViewModel.SportId),
                nameof(HomeQueryRequestData.City) => nameof(HomeViewModel.City),
                nameof(HomeQueryRequestData.From) => nameof(HomeViewModel.From),
                _ => string.Empty
            };
        }

        private async Task<HomeViewModel> BuildViewModelAsync(
            HomeSearchInputModel searchQuery,
            IEnumerable<MatchCardResponseModel>? matchCards = null)
        {

            var model = new HomeViewModel
            {
                SportId = searchQuery.SportId,
                City = searchQuery.City,
                From = searchQuery.From,
                Sports = await GetSportsSelectListAsync(searchQuery.SportId),
                MatchCards = matchCards?.Select(MapToMatchCardViewModel).ToList() ?? [],
            };

            return model;
        }


        private async Task<List<SelectListItem>>
            GetSportsSelectListAsync(int? selectedSportId)
        {
            var sports = await service.GetSportsAsync();

            return sports
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = selectedSportId.HasValue && x.Id == selectedSportId.Value
                })
                .ToList();
        }

        private static MatchCardViewModel
            MapToMatchCardViewModel(MatchCardResponseModel response)
        {
            return new MatchCardViewModel
            {
                Id = response.Id,
                Slug = SlugHelper.Create(response.Title),
                Title = response.Title,
                SportName = response.SportName,
                SportIcon = response.SportIcon ?? "bi-dribbble",
                VenueName = response.VenueName,
                City = response.City,
                StartsAt = response.Date
                    .ToDateTime(response.StartsAt, DateTimeKind.Utc)
                    .ToString("dddd, dd MMMM  HH:mm", CultureInfo.InvariantCulture),
                DurationMinutes = response.DurationMinutes,
                Joined = response.Joined,
                MaxPlayers = response.MaxPlayers,
                Waitlisted = response.Waitlisted,
                MinSkill = response.MinSkill,
                PricePerPlayer = response.PricePerPlayer.ToString("0.00", CultureInfo.InvariantCulture),
                Status = response.Status
            };
        }

        //[HttpGet]
        //public async Task<IActionResult> Index([FromQuery] HomeSearchInputModel searchQuery)
        //{
        //    if (false == ModelState.IsValid)
        //    {
        //        var invalidModel = new HomeViewModel
        //        {
        //            SportId = searchQuery.SportId,
        //            City = searchQuery.City,
        //            From = searchQuery.From,
        //            Sports = (await service.GetSportsAsync())
        //                .Select(x => new SelectListItem
        //                {
        //                    Text = x.Name,
        //                    Value = x.Id.ToString()
        //                })
        //        };
        //        return View(invalidModel);
        //    }

        //    HomeQueryRequestData itemFilter = new HomeQueryRequestData
        //    {
        //        SportId = searchQuery.SportId,
        //        City = searchQuery.City,
        //        From = searchQuery.From != null ? DateOnly.Parse(searchQuery.From) : null
        //    };


        //    var serviceResponse = await service.GetMatchCardsAsync(itemFilter);

        //    if (false == serviceResponse.IsSuccess)
        //    {
        //        foreach (var error in serviceResponse.Errors)
        //        {
        //            foreach (var memberName in error.MemberNames)
        //            {
        //                var viewModelProperty = memberName switch
        //                {
        //                    nameof(HomeQueryRequestData.SportId) => nameof(HomeViewModel.SportId),
        //                    nameof(HomeQueryRequestData.City) => nameof(HomeViewModel.City),
        //                    nameof(HomeQueryRequestData.From) => nameof(HomeViewModel.From),
        //                    _ => string.Empty
        //                };


        //                ModelState.AddModelError(
        //                    viewModelProperty,
        //                    error.ErrorMessage ?? "Input data is not correct"
        //                );
        //            }
        //        }

        //        var invalidModel = new HomeViewModel
        //        {
        //            SportId = searchQuery.SportId,
        //            City = searchQuery.City,
        //            From = searchQuery.From,
        //            Sports = (await service.GetSportsAsync())
        //                .Select(x => new SelectListItem
        //                {
        //                    Text = x.Name,
        //                    Value = x.Id.ToString(),
        //                    Selected = x.Id == searchQuery.SportId
        //                })
        //        };

        //        return View(invalidModel);
        //    }

        //    var matchCards = serviceResponse.ResponseData
        //        .Select(x => new MatchCardViewModel
        //        {
        //            Id = x.Id,
        //            Slug = SlugHelper.Create(x.Title),
        //            Title = x.Title,
        //            SportName = x.SportName,
        //            SportIcon = x.SportIcon ?? "bi-dribbble",
        //            VenueName = x.VenueName,
        //            City = x.City,
        //            StartsAt =
        //                x.Date.ToDateTime(x.StartsAt, DateTimeKind.Utc)
        //                .ToString("dddd, dd MMMM  HH:mm", CultureInfo.InvariantCulture),
        //            DurationMinutes = x.DurationMinutes,
        //            Joined = x.Joined,
        //            MaxPlayers = x.MaxPlayers,
        //            Waitlisted = x.Waitlisted,
        //            MinSkill = x.MinSkill,
        //            PricePerPlayer = x.PricePerPlayer.ToString("0.00", CultureInfo.InvariantCulture),
        //            Status = x.Status
        //        })
        //        .ToList();

        //    var sports = (await service.GetSportsAsync())
        //        .Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.Id.ToString()
        //        });

        //    var model = new HomeViewModel
        //    {
        //        SportId = searchQuery.SportId,
        //        City = searchQuery.City,
        //        From = searchQuery.From,

        //        MatchCards = matchCards,
        //        Sports = sports,
        //    };


        //    return View(model);
        //}



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
