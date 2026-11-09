using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Matches;
using PlayOn.Services.Data.Matches.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.Mappers;

public static class MatchMapper
{
    public static Expression<Func<Match, DetailsRow>> DetailsProjection()
    {
        return match => new DetailsRow
        {
            Id = match.Id,
            Title = match.Title,
            Description = match.Description,
            Status = match.Status,
            CancelReason = match.CancelReason,
            IsHidden = match.IsHidden,
            DurationMinutes = match.DurationMinutes,
            MinPlayers = match.MinPlayers,
            MaxPlayers = match.MaxPlayers,
            MinSkill = match.MinSkill,
            MaxSkill = match.MaxSkill,
            PricePerPlayer = match.PricePerPlayer,
            OrganiserId = match.OrganiserId,
            OrganiserName = match.Organiser.DisplayName,
            OrganiserReliability = match.Organiser.ReliabilityScore,
            SportName = match.Event!.Sport.Name,
            SportIcon = match.Event.Sport.Icon,
            CourtName = match.Event.Court.Name,
            Surface = match.Event.Court.Surface,
            CourtCapacity = match.Event.Court.Capacity,
            VenueName = match.Event.Venue.Name,
            Address = match.Event.Venue.Address,
            City = match.Event.Venue.City,
            Date = match.Event.Venue.Date,
            StartsAt = match.Event.Venue.StartsAt,
            ClosesAt = match.Event.Venue.ClosesAt,
            Participants = match.Participants
                .Where(participant => participant.Status != ParticipantStatus.Cancelled)
                .OrderBy(participant => participant.JoinedOn)
                .ThenBy(participant => participant.Id)
                .Select(participant => new ParticipantRow
                {
                    UserId = participant.UserId,
                    DisplayName = participant.User.DisplayName,
                    City = participant.User.City,
                    Status = participant.Status,
                    Reliability = participant.User.ReliabilityScore
                })
                .ToList()
        };
    }

    public static MatchDetailsResponseModel ToMatchDetailsResponse(
        DetailsRow source,
        bool canManage)
    {
        return new MatchDetailsResponseModel
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            Status = source.Status,
            CancelReason = source.CancelReason,
            IsHidden = source.IsHidden,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer,
            OrganiserName = source.OrganiserName,
            OrganiserReliability = source.OrganiserReliability,
            SportName = source.SportName,
            SportIcon = source.SportIcon,
            CourtName = source.CourtName,
            Surface = source.Surface,
            CourtCapacity = source.CourtCapacity,
            VenueName = source.VenueName,
            Address = source.Address,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            ClosesAt = source.ClosesAt,
            Joined = source.Participants.Count(x => x.Status == ParticipantStatus.Joined),
            Waitlisted = source.Participants.Count(x => x.Status == ParticipantStatus.Waitlisted),
            CanManage = canManage,
            Participants = source.Participants
                .Select(x => new MatchParticipantResponseModel
                {
                    UserId = x.UserId,
                    DisplayName = x.DisplayName,
                    City = x.City,
                    Status = x.Status,
                    IsOrganiser = x.UserId == source.OrganiserId,
                    Reliability = x.Reliability
                })
                .ToList()
        };
    }
}
