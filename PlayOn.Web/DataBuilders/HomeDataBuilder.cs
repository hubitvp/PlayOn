using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Sport.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
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


        public HomeSearchRequestData
            HomeSearchToRequestData(
                HomeSearchInputModel source,
                int page)
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
                Page = page,
                PageSize = DefaultPageSize
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
                HomeSearchInputModel source,
                PageResult<HomeMatchCardResponseModel>? page,
                IEnumerable<SportResponseModel> sports)
        {
            var result = new HomeViewModel
            {
                SportId = source.SportId,
                City = source.City,
                From = source.From,
                Sports = sports
                    .Select(x => sportDataBuilder.SportToSelectListItem(x, source.SportId))
                    .ToList(),
                MatchCards = page?.Items
                    .Select(HomeMatchCardToViewModel)
                    .ToList() ?? [],
                Page = page?.Page ?? FirstPage,
                PageSize = page?.PageSize ?? DefaultPageSize,
                TotalPages = page?.TotalPages ?? 0,
                TotalItems = page?.TotalCount ?? 0
            };

            return result;
        }
    }
}
