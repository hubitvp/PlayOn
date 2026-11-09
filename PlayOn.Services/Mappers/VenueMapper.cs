using PlayOn.Data.Models;
using PlayOn.Services.Data.Venue.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.Mappers;

public static class VenueMapper
{
    public static Expression<Func<Venue, VenueResponseModel>> VenueProjection()
    {
        return source => new VenueResponseModel
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            Court = new CourtResponseModel
            {
                Name = source.Event!.Court.Name,
                SportIcon = source.Event.Sport.Icon,
                Surface = source.Event.Court.Surface,
                Capacity = source.Event.Court.Capacity,
                HourlyPrice = source.Event.Court.HourlyPrice
            },
            MatchId = source.Event.MatchId,
            MatchTitle = source.Event.Match.Title
        };
    }

    public static VenueResponseModel ToVenueResponse(Venue source)
    {
        return new VenueResponseModel
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            Court = ToCourtResponse(source.Event!),
            MatchId = source.Event!.MatchId,
            MatchTitle = source.Event.Match.Title
        };
    }

    public static CourtResponseModel ToCourtResponse(Event source)
    {
        return new CourtResponseModel
        {
            Name = source.Court.Name,
            SportIcon = source.Sport.Icon,
            Surface = source.Court.Surface,
            Capacity = source.Court.Capacity,
            HourlyPrice = source.Court.HourlyPrice
        };
    }
}
