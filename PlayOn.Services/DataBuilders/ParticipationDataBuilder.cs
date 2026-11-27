using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Participation;
using PlayOn.Services.Data.Participation.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds projections, entities and response models for participation.
/// </summary>
public static class ParticipationDataBuilder
{
    /// <summary>
    /// Creates a projection from a match to the join information used by participation rules.
    /// </summary>
    /// <returns>The projection expression.</returns>
    internal static Expression<Func<Match, JoinInfoRow>>
        JoinInfoProjection()
    {
        return source => new JoinInfoRow
        {
            Id = source.Id,
            IsDeleted = source.IsDeleted,
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

    /// <summary>
    /// Creates a projection from a participation to a "my matches" list item, including waitlist position and rating eligibility.
    /// </summary>
    /// <returns>The projection expression.</returns>
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
                    && (p.JoinedOn < source.JoinedOn
                        || (p.JoinedOn == source.JoinedOn && p.UserId <= source.UserId)))
                : 0,
            IsOrganiser = source.Match.OrganiserId == source.UserId,
            CanRate = source.Match.Status == MatchStatus.Completed
                && source.Status == ParticipantStatus.Attended
        };
    }

    /// <summary>
    /// Creates a participant entity with the given status, using the current time for join and status-change times.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="status">The initial participant status.</param>
    /// <param name="nowLocal">The current local time.</param>
    /// <returns>The new participant entity.</returns>
    public static MatchParticipant
        ParticipantToEntity(
            Guid matchId,
            Guid userId,
            ParticipantStatus status,
            DateTime nowLocal)
    {
        var result = new MatchParticipant
        {
            MatchId = matchId,
            UserId = userId,
            Status = status,
            JoinedOn = nowLocal,
            StatusChangedOn = nowLocal,
            IsLateCancel = false
        };

        return result;
    }

    /// <summary>
    /// Creates the response model for a join operation.
    /// </summary>
    /// <param name="status">The resulting participant status.</param>
    /// <param name="waitlistPosition">The waitlist position, or 0 if not waitlisted.</param>
    /// <param name="matchConfirmed">Whether the join confirmed the match.</param>
    /// <returns>The join response model.</returns>
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

    /// <summary>
    /// Creates the response model for a leave operation.
    /// </summary>
    /// <param name="isLateCancel">Whether the leave counted as a late cancel.</param>
    /// <param name="promotedAnotherPlayer">Whether a waitlisted player was promoted.</param>
    /// <returns>The leave response model.</returns>
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