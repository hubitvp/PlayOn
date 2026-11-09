using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Home.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.Mappers;

public static class HomeMapper
{
    public static Expression<Func<Match, MatchCardResponseModel>> MatchCardProjection()
    {
        return source => new MatchCardResponseModel
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

    public static Expression<Func<Sport, SportsResponseModel>> SportsProjection()
    {
        return source => new SportsResponseModel
        {
            Id = source.Id,
            Name = source.Name
        };
    }

    public static MatchCardResponseModel ToMatchCardResponse(Match source)
    {
        return new MatchCardResponseModel
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

    public static SportsResponseModel ToSportsResponse(Sport source)
    {
        return new SportsResponseModel
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}
