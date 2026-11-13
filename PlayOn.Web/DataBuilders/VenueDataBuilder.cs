using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Venue.RequestData;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels.Venue;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.DataBuilders;

public class VenueDataBuilder
{
    private readonly IDisplayFormatter displayFormatter;

    public VenueDataBuilder(IDisplayFormatter displayFormatter)
    {
        this.displayFormatter = displayFormatter;
    }

    public VenueQueryRequestData
        VenueQueryToRequestData(int page)
    {
        var result = new VenueQueryRequestData
        {
            Page = page,
            PageSize = DefaultPageSize
        };

        return result;
    }

    public VenueViewModel
        VenueListToViewModel(
            PageResult<VenueResponseModel>? page,
            int requestedPage)
    {
        var result = new VenueViewModel
        {
            Venues = page?.Items
                .Select(VenueToViewModel)
                .ToList() ?? [],
            Page = page?.Page ?? requestedPage,
            PageSize = page?.PageSize ?? DefaultPageSize,
            TotalPages = page?.TotalPages ?? 0
        };

        return result;
    }

    public VenueViewModelData
        VenueToViewModel(VenueResponseModel source)
    {
        var result = new VenueViewModelData
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Date = displayFormatter.Date(source.Date),
            StartsAt = displayFormatter.Time(source.StartsAt),
            OpensAt = displayFormatter.Time(source.OpensAt),
            ClosesAt = displayFormatter.Time(source.ClosesAt),
            MatchId = source.MatchId,
            MatchTitle = source.MatchTitle,
            MatchSlug = Slug.Create(source.MatchTitle),
            Court = CourtToViewModel(source.Court)
        };

        return result;
    }

    public VenueCourtViewModel
        CourtToViewModel(VenueCourtResponseModel source)
    {
        var result = new VenueCourtViewModel
        {
            Name = source.Name,
            SportIconCss = source.SportIcon.ToCssClass(),
            Surface = source.Surface.ToString(),
            Capacity = source.Capacity,
            HourlyPrice = source.HourlyPrice.ToFormattedString()
        };

        return result;
    }
}
