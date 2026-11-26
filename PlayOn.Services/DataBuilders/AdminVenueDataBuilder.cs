using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminVenues;
using PlayOn.Services.Data.AdminVenues.RequestData;
using PlayOn.Services.Data.AdminVenues.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class AdminVenueDataBuilder
{
    internal static Expression<Func<Venue, AdminVenueRow>>
        VenueProjection()
    {
        return source => new AdminVenueRow
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            IsDeleted = source.IsDeleted,
            IsBooked = source.Event != null,
            MatchTitle = source.Event != null ? source.Event.Match.Title : null
        };
    }

    internal static AdminVenueResponseModel
        VenueToResponseModel(AdminVenueRow source)
    {
        var result = new AdminVenueResponseModel
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            IsDeleted = source.IsDeleted,
            IsBooked = source.IsBooked,
            MatchTitle = source.MatchTitle
        };

        return result;
    }

    public static AdminVenueEditResponseModel
        VenueToEditResponseModel(Venue source, bool isBooked)
    {
        var result = new AdminVenueEditResponseModel
        {
            Id = source.Id,
            Name = source.Name,
            Address = source.Address,
            City = source.City,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            IsDeleted = source.IsDeleted,
            IsBooked = isBooked
        };

        return result;
    }

    public static Venue ToEntity(AdminVenueCreateRequestData source)
    {
        var result = new Venue();

        ApplyToEntity(source, result);

        return result;
    }

    public static void ApplyToEntity(AdminVenueRequestDataBase source, Venue target)
    {
        target.Name = source.Name.Trim();
        target.Address = source.Address.Trim();
        target.City = source.City.Trim();
        target.Latitude = source.Latitude;
        target.Longitude = source.Longitude;
        target.Date = source.Date;
        target.StartsAt = source.StartsAt;
        target.OpensAt = source.OpensAt;
        target.ClosesAt = source.ClosesAt;
    }
}
