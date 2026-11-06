using System.Globalization;
using Riok.Mapperly.Abstractions;
using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Web.ViewModels.Home;
using PlayOn.Web.ViewModels.Match;
using PlayOn.Web.ViewModels.Venue;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Mappers;

[Mapper]
public partial class WebMapper
{
    public HomeQueryRequestData ToHomeQuery(HomeSearchInputModel source, int page)
        => new()
        {
            SportId = source.SportId,
            City = source.City,
            From = source.From != null ? DateOnly.Parse(source.From) : null,
            Page = page,
            PageSize = DefaultPageSize
        };

    [MapperIgnoreTarget(nameof(MatchCardViewModel.Slug))]
    [MapperIgnoreTarget(nameof(MatchCardViewModel.SportIcon))]
    [MapperIgnoreTarget(nameof(MatchCardViewModel.StartsAt))]
    [MapperIgnoreTarget(nameof(MatchCardViewModel.PricePerPlayer))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.SportIcon))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.Date))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.StartsAt))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.PricePerPlayer))]
    public partial MatchCardViewModel MapMatchCardCore(MatchCardResponseModel source);

    public MatchCardViewModel ToMatchCardViewModel(MatchCardResponseModel source)
    {
        var target = MapMatchCardCore(source);
        target.SportIcon = source.SportIcon.ToCssClass();
        target.StartsAt = source.Date
            .ToDateTime(source.StartsAt, DateTimeKind.Utc)
            .ToString("dddd, dd MMMM  HH:mm", CultureInfo.InvariantCulture);
        target.PricePerPlayer = source.PricePerPlayer.ToFormattedString();
        return target;
    }

    [MapperIgnoreTarget(nameof(CourtViewModel.SportIconCss))]
    [MapperIgnoreTarget(nameof(CourtViewModel.Surface))]
    [MapperIgnoreTarget(nameof(CourtViewModel.HourlyPrice))]
    [MapperIgnoreSource(nameof(CourtResponseModel.SportIcon))]
    [MapperIgnoreSource(nameof(CourtResponseModel.Surface))]
    [MapperIgnoreSource(nameof(CourtResponseModel.HourlyPrice))]
    public partial CourtViewModel MapCourtCore(CourtResponseModel source);

    public CourtViewModel ToCourtViewModel(CourtResponseModel source)
    {
        var target = MapCourtCore(source);
        target.SportIconCss = source.SportIcon.ToCssClass();
        target.Surface = source.Surface.ToString();
        target.HourlyPrice = source.HourlyPrice.ToFormattedString();
        return target;
    }

    [MapperIgnoreTarget(nameof(VenueViewModelData.Court))]
    [MapperIgnoreTarget(nameof(VenueViewModelData.MatchSlug))]
    [MapperIgnoreSource(nameof(VenueResponseModel.Court))]
    public partial VenueViewModelData MapVenueCore(VenueResponseModel source);

    public VenueViewModelData ToVenueViewModel(VenueResponseModel source)
    {
        var target = MapVenueCore(source);
        target.Court = ToCourtViewModel(source.Court);
        return target;
    }
}
