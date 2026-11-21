using PlayOn.Common.ApplicationConstants;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Attendance;
using PlayOn.Services.Data.Attendance.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class AttendanceDataBuilder
{
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
