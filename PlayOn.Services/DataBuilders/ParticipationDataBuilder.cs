using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Participation;
using PlayOn.Services.Data.Participation.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class ParticipationDataBuilder
{
    internal static Expression<Func<Match, JoinInfoRow>>
        JoinInfoProjection()
    {
        return source => new JoinInfoRow
        {
            Id = source.Id,
            IsDeleted = source.IsDeleted,
            IsHidden = source.IsHidden,
            Status = source.Status,
            Title = source.Title,
            OrganiserId = source.OrganiserId,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            DurationMinutes = source.DurationMinutes,
            SportId = source.Event!.SportId,
            Date = source.Event.Venue.Date,
            StartsAt = source.Event.Venue.StartsAt
        };
    }

    public static Expression<Func<MatchParticipant, MyMatchResponseModel>>
        MyMatchProjection()
    {
        return source => new MyMatchResponseModel
        {
            Id = source.MatchId,
            Title = source.Match.Title,
            SportName = source.Match.Event!.Sport.Name,
            SportIcon = source.Match.Event.Sport.Icon,
            VenueName = source.Match.Event.Venue.Name,
            City = source.Match.Event.Venue.City,
            Date = source.Match.Event.Venue.Date,
            StartsAt = source.Match.Event.Venue.StartsAt,
            DurationMinutes = source.Match.DurationMinutes,
            Status = source.Status,
            MatchStatus = source.Match.Status,
            WaitlistPosition = source.Status == ParticipantStatus.Waitlisted
                ? source.Match.Participants.Count(p =>
                    p.Status == ParticipantStatus.Waitlisted
                    && p.JoinedOn <= source.JoinedOn)
                : 0,
            IsOrganiser = source.Match.OrganiserId == source.UserId,
            CanRate = source.Match.Status == MatchStatus.Completed
                && source.Status == ParticipantStatus.Attended
        };
    }

    public static MatchParticipant
        ParticipantToEntity(
            Guid matchId,
            Guid userId,
            ParticipantStatus status,
            DateTime nowUtc)
    {
        var result = new MatchParticipant
        {
            MatchId = matchId,
            UserId = userId,
            Status = status,
            JoinedOn = nowUtc,
            StatusChangedOn = nowUtc,
            IsLateCancel = false
        };

        return result;
    }

    public static JoinMatchResponseModel
        JoinToResponseModel(
            ParticipantStatus status,
            int waitlistPosition,
            bool matchConfirmed)
    {
        var result = new JoinMatchResponseModel
        {
            Status = status,
            WaitlistPosition = waitlistPosition,
            MatchConfirmed = matchConfirmed
        };

        return result;
    }

    public static LeaveMatchResponseModel
        LeaveToResponseModel(
            bool isLateCancel,
            bool promotedAnotherPlayer)
    {
        var result = new LeaveMatchResponseModel
        {
            IsLateCancel = isLateCancel,
            PromotedAnotherPlayer = promotedAnotherPlayer
        };

        return result;
    }
}
