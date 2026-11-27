using PlayOn.Common.ApplicationConstants;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Attendance;
using PlayOn.Services.Data.Attendance.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds projections and response models for match attendance.
/// </summary>
public static class AttendanceDataBuilder
{
    /// <summary>
    /// Creates a projection from a match to the attendance information row, including venue, date, time and duration.
    /// </summary>
    /// <returns>An expression projecting a match to an attendance information row.</returns>
    internal static Expression<Func<Match, AttendanceInfoRow>>
        AttendanceInfoProjection()
    {
        return source => new AttendanceInfoRow
        {
            Id = source.Id,
            IsDeleted = source.IsDeleted,
            OrganiserId = source.OrganiserId,
            Status = source.Status,
            Title = source.Title,
            VenueName = source.Event!.Venue.Name,
            City = source.Event.Venue.City,
            Date = source.Event.Venue.Date,
            StartsAt = source.Event.Venue.StartsAt,
            DurationMinutes = source.DurationMinutes
        };
    }

    /// <summary>
    /// Creates a projection from a match participant to an attendance player model.
    /// </summary>
    /// <returns>An expression projecting a participant to an attendance player model.</returns>
    public static Expression<Func<MatchParticipant, AttendancePlayerResponseModel>>
        AttendancePlayerProjection()
    {
        return source => new AttendancePlayerResponseModel
        {
            UserId = source.UserId,
            DisplayName = source.User.DisplayName,
            City = source.User.City
        };
    }

    /// <summary>
    /// Combines the match information row and the player list into the attendance response model.
    /// </summary>
    /// <param name="info">The match information row.</param>
    /// <param name="players">The players on the attendance sheet.</param>
    /// <returns>The attendance response model.</returns>
    internal static AttendanceResponseModel
        AttendanceToResponseModel(
            AttendanceInfoRow info,
            IReadOnlyList<AttendancePlayerResponseModel> players)
    {
        var result = new AttendanceResponseModel
        {
            MatchId = info.Id,
            Title = info.Title,
            VenueName = info.VenueName,
            City = info.City,
            Date = info.Date,
            StartsAt = info.StartsAt,
            DurationMinutes = info.DurationMinutes,
            Players = players
        };

        return result;
    }


    /// <summary>
    /// Calculates a player's reliability as the percentage of attended records among attended, no-show and late-cancel records, rounded to a whole number.
    /// </summary>
    /// <param name="attended">The number of matches attended.</param>
    /// <param name="noShow">The number of no-shows.</param>
    /// <param name="lateCancel">The number of late cancellations.</param>
    /// <returns>The reliability percentage, or null if there are fewer records than the minimum required.</returns>
    public static int? ReliabilityScore(int attended, int noShow, int lateCancel)
    {
        var total = attended + noShow + lateCancel;

        if (total < ParticipationRules.ReliabilityMinRecords)
        {
            return null;
        }

        return (int)Math.Round(attended * 100.0 / total);
    }
}
