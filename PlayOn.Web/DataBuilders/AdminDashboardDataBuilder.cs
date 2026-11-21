using PlayOn.Services.Data.AdminDashboard.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels.Admin;

namespace PlayOn.Web.DataBuilders;

public class AdminDashboardDataBuilder
{
    private readonly IDisplayFormatter displayFormatter;

    public AdminDashboardDataBuilder(IDisplayFormatter displayFormatter)
    {
        this.displayFormatter = displayFormatter;
    }

    public AdminDashboardViewModel
        DashboardToViewModel(AdminDashboardResponseModel source)
    {
        var result = new AdminDashboardViewModel
        {
            UsersCount = source.UsersCount,
            ActiveUsersCount = source.ActiveUsersCount,
            MatchesCount = source.MatchesCount,
            UpcomingMatchesCount = source.UpcomingMatchesCount,
            CancelledMatchesCount = source.CancelledMatchesCount,
            SportsCount = source.SportsCount,
            VenuesCount = source.VenuesCount,
            FreeVenuesCount = source.FreeVenuesCount,
            RatingsCount = source.RatingsCount,
            UpcomingMatches = source.UpcomingMatches
                .Select(UpcomingMatchToViewModel)
                .ToList()
        };

        return result;
    }

    public AdminUpcomingMatchViewModel
        UpcomingMatchToViewModel(AdminUpcomingMatchResponseModel source)
    {
        var result = new AdminUpcomingMatchViewModel
        {
            Id = source.Id,
            Title = source.Title,
            SportName = source.SportName,
            VenueName = source.VenueName,
            City = source.City,
            StartsAt = $"{displayFormatter.ShortDate(source.Date)}, {displayFormatter.Time(source.StartsAt)}",
            Joined = source.Joined,
            MaxPlayers = source.MaxPlayers,
            Status = source.Status.ToString()
        };

        return result;
    }
}
