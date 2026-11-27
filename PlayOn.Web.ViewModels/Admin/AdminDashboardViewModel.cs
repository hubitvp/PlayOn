namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// View model of the administrator dashboard: aggregated statistics and the upcoming matches.
    /// </summary>
    public class AdminDashboardViewModel
    {
        /// <summary>
        /// Total number of users.
        /// </summary>
        public required int UsersCount { get; init; }
        /// <summary>
        /// Number of active users.
        /// </summary>
        public required int ActiveUsersCount { get; init; }
        /// <summary>
        /// Total number of matches.
        /// </summary>
        public required int MatchesCount { get; init; }
        /// <summary>
        /// Number of upcoming matches.
        /// </summary>
        public required int UpcomingMatchesCount { get; init; }
        /// <summary>
        /// Number of cancelled matches.
        /// </summary>
        public required int CancelledMatchesCount { get; init; }
        /// <summary>
        /// Total number of sports.
        /// </summary>
        public required int SportsCount { get; init; }
        /// <summary>
        /// Total number of venues.
        /// </summary>
        public required int VenuesCount { get; init; }
        /// <summary>
        /// Number of venues that are not booked.
        /// </summary>
        public required int FreeVenuesCount { get; init; }
        /// <summary>
        /// Total number of ratings given.
        /// </summary>
        public required int RatingsCount { get; init; }

        /// <summary>
        /// Upcoming matches shown on the dashboard.
        /// </summary>
        public required IReadOnlyList<AdminUpcomingMatchViewModel> UpcomingMatches { get; init; } = [];
    }
}
