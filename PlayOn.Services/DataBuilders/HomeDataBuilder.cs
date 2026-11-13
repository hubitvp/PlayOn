using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Home.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class HomeDataBuilder
{
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
