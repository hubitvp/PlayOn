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
    public class HomeDataBuilder
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

        public PageRequestData
            PageInputToPageRequestData(PageInputModel source)
        {
            var result = new PageRequestData
            {
                Page = source.PageNumber,
                PageSize = source.PageSize,
            };
            return result;
        }


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
                HomeSearchInputModel inputModel,
                IReadOnlyList<HomeSportOptionsResponseModel> sportOptions)
        {
            var result = new HomeViewModel
            {
                InputInquiry = inputModel,
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
                InputInquiry = inputModel,
                SportOptions = source.SportOptions
                    .Select(x => HomeSportOptionsToSelectListItem(x, inputModel.SportId)),
                Page = page,
            };

            return result;
        }
    }
}
