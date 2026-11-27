using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminVenues;
using PlayOn.Services.Data.AdminVenues.RequestData;
using PlayOn.Services.Data.AdminVenues.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds venue projections, response models and entities for administration.
/// </summary>
public static class AdminVenueDataBuilder
{
    /// <summary>
    /// Creates a query projection from a venue to a row that includes booking state and the booked match title.
    /// </summary>
    /// <returns>An expression that projects a venue to an admin venue row.</returns>
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

    /// <summary>
    /// Maps a venue row to a list response model.
    /// </summary>
    /// <param name="source">The venue row.</param>
    /// <returns>The venue response model.</returns>
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

    /// <summary>
    /// Maps a venue entity and its booking state to an edit response model.
    /// </summary>
    /// <param name="source">The venue entity.</param>
    /// <param name="isBooked">Whether a match is booked on the venue.</param>
    /// <returns>The venue edit response model.</returns>
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

    /// <summary>
    /// Creates a new venue entity from the create request.
    /// </summary>
    /// <param name="source">The create request data.</param>
    /// <returns>A new venue populated from the request.</returns>
    public static Venue ToEntity(AdminVenueCreateRequestData source)
    {
        var result = new Venue();

        ApplyToEntity(source, result);

        return result;
    }

    /// <summary>
    /// Copies the request values onto a venue entity, trimming name, address and city.
    /// </summary>
    /// <param name="source">The request data.</param>
    /// <param name="target">The venue entity to update.</param>
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
