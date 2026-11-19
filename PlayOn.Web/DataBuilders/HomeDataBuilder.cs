using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Sport.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels;
using PlayOn.Web.ViewModels.Home;
using System.Collections.Immutable;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.DataBuilders
{
    public class HomeDataBuilder : DataBuilder
    {
        private readonly IDisplayFormatter displayFormatter;

        public HomeDataBuilder(
            IDisplayFormatter displayFormatter)
        {
            this.displayFormatter = displayFormatter;
        }

        public SelectListItem
            HomeSportOptionsToSelectListItem(
                HomeSportOptionsResponseModel source,
                Guid? selectedSportId)
        {
            var result = new SelectListItem
            {
                Text = source.Name,
                Value = source.Id.ToString(),
                Selected = source.Id == selectedSportId
            };

            return result;
        }


        public HomeSearchRequestData
            HomeSearchToRequestData(
                HomeViewModel source)
        {
            var target = new HomeSearchRequestData
            {
                SportId = source.SportId,
                City = source.City,
                From = DateOnly.TryParse(
                    source.From,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var from)
                    ? from
                    : null,
            };

            return target;
        }


        public HomeMatchCardViewModel
            HomeMatchCardToViewModel(
                HomeMatchCardResponseModel source)
        {
            var result = new HomeMatchCardViewModel
            {
                Id = source.Id,
                Slug = Slug.Create(source.Title),
                Title = source.Title,
                SportName = source.SportName,
                SportIcon = source.SportIcon.ToCssClass(),
                VenueName = source.VenueName,
                City = source.City,
                StartsAt = displayFormatter.ShortDateTime(source.Date.ToDateTime(source.StartsAt)),
                DurationMinutes = source.DurationMinutes,
                Joined = source.Joined,
                MaxPlayers = source.MaxPlayers,
                Waitlisted = source.Waitlisted,
                MinSkill = source.MinSkill,
                PricePerPlayer = displayFormatter.Price(source.PricePerPlayer),
                Status = source.Status
            };

            return result;
        }



        public HomeViewModel
            HomeToViewModel(
                HomeViewModel model,
                HomeResponseModel source)
        {
            PageResult<HomeMatchCardViewModel>? page = null;

            if (source.Page != null)
            {
                page = new PageResult<HomeMatchCardViewModel>
                (
                    pageItems: source.Page.PageItems.Select(HomeMatchCardToViewModel).ToImmutableList(),
                    pageMark: source.Page.PageMark,
                    pageSize: source.Page.PageSize,
                    totalItemsCount: source.Page.TotalItemsCount
                );
            }

            model.SportOptions = source.SportOptions
                    .Select(x => HomeSportOptionsToSelectListItem(x, model.SportId))
                    .ToImmutableList();
            model.Page = page;

            return model;
        }
    }
}