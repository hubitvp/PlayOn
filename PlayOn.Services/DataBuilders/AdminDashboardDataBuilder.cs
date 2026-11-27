using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminDashboard.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds dashboard projections and response models.
/// </summary>
public static class AdminDashboardDataBuilder
{
    /// <summary>
    /// Creates a query projection from a match to an upcoming-match model, including sport, venue and joined participant count.
    /// </summary>
    /// <returns>An expression that projects a match to an upcoming-match response model.</returns>
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

    /// <summary>
    /// Assembles the dashboard response model from the computed counts and upcoming matches.
    /// </summary>
    /// <param name="usersCount">The total number of users.</param>
    /// <param name="activeUsersCount">The number of active users.</param>
    /// <param name="matchesCount">The total number of matches.</param>
    /// <param name="upcomingMatchesCount">The number of upcoming matches.</param>
    /// <param name="cancelledMatchesCount">The number of cancelled matches.</param>
    /// <param name="sportsCount">The number of sports.</param>
    /// <param name="venuesCount">The number of venues.</param>
    /// <param name="freeVenuesCount">The number of free venues.</param>
    /// <param name="ratingsCount">The number of player ratings.</param>
    /// <param name="upcomingMatches">The upcoming matches to display.</param>
    /// <returns>The populated dashboard response model.</returns>
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
