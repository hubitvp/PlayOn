using PlayOn.Data.Models;
using PlayOn.Services.Data.Venue.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Provides mapping between venue entities and response models.
/// </summary>
public static class VenueDataBuilder
{
    /// <summary>
    /// Creates a projection from a venue to its response model, including schedule, court details and the related match.
    /// </summary>
    /// <returns>An expression projecting a <see cref="Venue"/> to a <see cref="VenueResponseModel"/>.</returns>
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
            Court = new VenueCourtResponseModel
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

    /// <summary>
    /// Maps a venue entity, with its event, court and match loaded, to a response model.
    /// </summary>
    /// <param name="source">The venue entity.</param>
    /// <returns>The mapped venue response model.</returns>
    public static VenueResponseModel VenueToResponseModel(Venue source)
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
            Court = CourtToResponseModel(source.Event!),
            MatchId = source.Event!.MatchId,
            MatchTitle = source.Event.Match.Title
        };
    }

    /// <summary>
    /// Maps an event to the court response model with court details and sport icon.
    /// </summary>
    /// <param name="source">The event whose court and sport are mapped.</param>
    /// <returns>The mapped court response model.</returns>
    public static VenueCourtResponseModel CourtToResponseModel(Event source)
    {
        return new VenueCourtResponseModel
        {
            Name = source.Court.Name,
            SportIcon = source.Sport.Icon,
            Surface = source.Court.Surface,
            Capacity = source.Court.Capacity,
            HourlyPrice = source.Court.HourlyPrice
        };
    }
}
