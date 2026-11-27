using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Home.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Provides projections that map home page entities to response models.
/// </summary>
public static class HomeDataBuilder
{

    /// <summary>
    /// Creates a projection from a sport to a home sport option containing its id and name.
    /// </summary>
    /// <returns>An expression projecting a <see cref="Sport"/> to a <see cref="HomeSportOptionsResponseModel"/>.</returns>
    public static Expression<Func<Sport, HomeSportOptionsResponseModel>>
        HomeSportOptionsProjection()
    {
        Expression<Func<Sport, HomeSportOptionsResponseModel>> result =
            source => new HomeSportOptionsResponseModel
            {
                Id = source.Id,
                Name = source.Name
            };

        return result;
    }

    /// <summary>
    /// Creates a projection from a match to a home match card, including sport, venue, schedule, joined and waitlisted participant counts, skill and price.
    /// </summary>
    /// <returns>An expression projecting a <see cref="Match"/> to a <see cref="HomeMatchCardResponseModel"/>.</returns>
    public static Expression<Func<Match, HomeMatchCardResponseModel>>
        HomeMatchCardProjection()
    {
        return source => new HomeMatchCardResponseModel
        {
            Id = source.Id,
            Title = source.Title,
            SportName = source.Event!.Sport.Name,
            SportIcon = source.Event.Sport.Icon,
            VenueName = source.Event.Venue.Name,
            City = source.Event.Venue.City,
            Date = source.Event.Venue.Date,
            StartsAt = source.Event.Venue.StartsAt,
            DurationMinutes = source.DurationMinutes,
            Joined = source.Participants.Count(x => x.Status == ParticipantStatus.Joined),
            MaxPlayers = source.MaxPlayers,
            Waitlisted = source.Participants.Count(x => x.Status == ParticipantStatus.Waitlisted),
            MinSkill = source.MinSkill,
            PricePerPlayer = source.PricePerPlayer,
            Status = source.Status
        };
    }
}
