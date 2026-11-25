namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public required int UsersCount { get; init; }
        public required int ActiveUsersCount { get; init; }
        public required int MatchesCount { get; init; }
        public required int UpcomingMatchesCount { get; init; }
        public required int CancelledMatchesCount { get; init; }
        public required int SportsCount { get; init; }
        public required int VenuesCount { get; init; }
        public required int FreeVenuesCount { get; init; }
        public required int RatingsCount { get; init; }

        public required IReadOnlyList<AdminUpcomingMatchViewModel> UpcomingMatches { get; init; } = [];
    }
}
