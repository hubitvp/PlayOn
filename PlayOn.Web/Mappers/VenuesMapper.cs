using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Web.ViewModels.Venue;

namespace PlayOn.Web.Mappers;

public static class VenuesMapper
{
    public static VenueViewModelData ToVenueViewModel(VenueResponseModel source)
    {
        var result = new VenueViewModelData
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            MatchId = source.MatchId,
            MatchTitle = source.MatchTitle,
            Court = ToCourtViewModel(source.Court),
            MatchSlug = string.Empty
        };

        return result;
    }

    public static CourtViewModel ToCourtViewModel(CourtResponseModel source)
    {
        var result = new CourtViewModel
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
