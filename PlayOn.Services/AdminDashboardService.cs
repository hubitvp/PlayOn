using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.AdminDashboard.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Implements the administration dashboard by aggregating counts from several repositories.
    /// </summary>
    public class AdminDashboardService : IAdminDashboardService
    {
        private const int UpcomingMatchesToShow = 5;

        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly IRepository<Venue> venueRepository;
        private readonly IRepository<PlayerRating> ratingRepository;
        private readonly TimeProvider timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminDashboardService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="matchRepository">The match repository.</param>
        /// <param name="sportRepository">The sport repository.</param>
        /// <param name="venueRepository">The venue repository.</param>
        /// <param name="ratingRepository">The player rating repository.</param>
        /// <param name="timeProvider">The time provider used to determine today's date.</param>
        public AdminDashboardService(
            IRepository<ApplicationUser> userRepository,
            IRepository<Match> matchRepository,
            IRepository<Sport> sportRepository,
            IRepository<Venue> venueRepository,
            IRepository<PlayerRating> ratingRepository,
            TimeProvider timeProvider)
        {
            this.userRepository = userRepository;
            this.matchRepository = matchRepository;
            this.sportRepository = sportRepository;
            this.venueRepository = venueRepository;
            this.ratingRepository = ratingRepository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Counts users, non-deleted booked matches, sports, venues and ratings, and lists the next few open or confirmed matches starting today or later.
        /// </summary>
        /// <returns>A successful response containing the dashboard summary.</returns>
        public async Task<ServiceResponse<AdminDashboardResponseModel>>
            GetDashboardAsync()
        {
            var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);

            var usersCount = await userRepository.Query().CountAsync();
            var activeUsersCount = await userRepository.Query().CountAsync(x => x.IsActive);

            var visibleMatches = matchRepository
                .Query()
                .Where(x => !x.IsDeleted && x.Event != null);

            var matchesCount = await visibleMatches.CountAsync();

            var cancelledMatchesCount = await visibleMatches
                .CountAsync(x => x.Status == MatchStatus.Cancelled);

            var upcomingMatches = visibleMatches
                .Where(x => (x.Status == MatchStatus.Open || x.Status == MatchStatus.Confirmed)
                    && x.Event!.Venue.Date >= today);

            var upcomingMatchesCount = await upcomingMatches.CountAsync();

            var upcomingList = await upcomingMatches
                .OrderBy(x => x.Event!.Venue.Date)
                .ThenBy(x => x.Event!.Venue.StartsAt)
                .ThenBy(x => x.Id)
                .Take(UpcomingMatchesToShow)
                .Select(AdminDashboardDataBuilder.UpcomingMatchProjection())
                .ToListAsync();

            var sportsCount = await sportRepository.Query().CountAsync();

            var venuesCount = await venueRepository
                .Query()
                .CountAsync(x => !x.IsDeleted);

            var freeVenuesCount = await venueRepository
                .Query()
                .CountAsync(x => !x.IsDeleted && x.Event == null && x.Date >= today);

            var ratingsCount = await ratingRepository.Query().CountAsync();

            var response = AdminDashboardDataBuilder.DashboardToResponseModel(
                usersCount,
                activeUsersCount,
                matchesCount,
                upcomingMatchesCount,
                cancelledMatchesCount,
                sportsCount,
                venuesCount,
                freeVenuesCount,
                ratingsCount,
                upcomingList);

            return ServiceResponse<AdminDashboardResponseModel>.Success(response);
        }
    }
}