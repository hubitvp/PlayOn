using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminDashboard.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class AdminDashboardDataBuilder
{
    public static Expression<Func<Match, AdminUpcomingMatchResponseModel>>
        UpcomingMatchProjection()
    {
        return source => new AdminUpcomingMatchResponseModel
        {
            Id = source.Id,
            Title = source.Title,
            SportName = source.Event!.Sport.Name,
            VenueName = source.Event.Venue.Name,
            City = source.Event.Venue.City,
            Date = source.Event.Venue.Date,
            StartsAt = source.Event.Venue.StartsAt,
            Joined = source.Participants.Count(x => x.Status == ParticipantStatus.Joined),
            MaxPlayers = source.MaxPlayers,
            Status = source.Status
        };
    }

    public static AdminDashboardResponseModel
        DashboardToResponseModel(
            int usersCount,
            int activeUsersCount,
            int matchesCount,
            int upcomingMatchesCount,
            int cancelledMatchesCount,
            int sportsCount,
            int venuesCount,
            int freeVenuesCount,
            int ratingsCount,
            IReadOnlyList<AdminUpcomingMatchResponseModel> upcomingMatches)
    {
        var result = new AdminDashboardResponseModel
        {
            UsersCount = usersCount,
            ActiveUsersCount = activeUsersCount,
            MatchesCount = matchesCount,
            UpcomingMatchesCount = upcomingMatchesCount,
            CancelledMatchesCount = cancelledMatchesCount,
            SportsCount = sportsCount,
            VenuesCount = venuesCount,
            FreeVenuesCount = freeVenuesCount,
            RatingsCount = ratingsCount,
            UpcomingMatches = upcomingMatches
        };

        return result;
    }
}
