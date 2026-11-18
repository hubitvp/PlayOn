using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Sport.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels;
using PlayOn.Web.ViewModels.Home;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.DataBuilders
{
    public class HomeDataBuilder : DataBuilder
    {
        private readonly SportDataBuilder sportDataBuilder;
        private readonly IDisplayFormatter displayFormatter;

        public HomeDataBuilder(
            SportDataBuilder sportDataBuilder,
            IDisplayFormatter displayFormatter)
        {
            this.sportDataBuilder = sportDataBuilder;
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

        // Option A: If HomeSearchToRequestData still accepts HomeSearchInputModel from the controller action
        public HomeSearchRequestData
            HomeSearchToRequestData(
                HomeSearchInputModel source)
        {
            var result = new HomeSearchRequestData
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

            return result;
        }

        // Option B (Alternative): If you pass HomeViewModel directly instead of HomeSearchInputModel, 
        // you can use this overload or map it similarly.

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
            ToViewModel(
                HomeSearchInputModel inputModel,
                IReadOnlyList<HomeSportOptionsResponseModel> sportOptions)
        {
            var result = new HomeViewModel
            {
                SportId = inputModel.SportId,
                City = inputModel.City,
                From = inputModel.From,
                SportOptions = sportOptions
                    .Select(x => HomeSportOptionsToSelectListItem(x, inputModel.SportId)),
                Page = null,
            };
            return result;
        }

        public HomeViewModel
            HomeToViewModel(
                HomeSearchInputModel inputModel,
                HomeResponseModel source)
        {
            PageResult<HomeMatchCardViewModel>? page = null;

            if (source.Page != null)
            {
                page = new PageResult<HomeMatchCardViewModel>
                (
                    pageItems: source.Page.PageItems.Select(HomeMatchCardToViewModel).ToList(),
                    pageMark: source.Page.PageMark,
                    pageSize: source.Page.PageSize,
                    totalItemsCount: source.Page.TotalItemsCount
                );
            }

            var result = new HomeViewModel
            {
                SportId = inputModel.SportId,
                City = inputModel.City,
                From = inputModel.From,
                SportOptions = source.SportOptions
                    .Select(x => HomeSportOptionsToSelectListItem(x, inputModel.SportId)),
                Page = page,
            };

            return result;
        }
    }
}