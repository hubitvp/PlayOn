using Microsoft.Extensions.Time.Testing;
using PlayOn.Services;
using PlayOn.Services.Contracts;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.DataBuilders.Formatting;

namespace PlayOn.Web.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection AddLocalResources(this IServiceCollection services)
        {


            var fakeTimeProvider = new FakeTimeProvider
            (
                new DateTimeOffset(2026, 10, 1, 0, 0, 0, TimeSpan.Zero)
            );


            services.AddSingleton(fakeTimeProvider);
            //services.AddSingleton(TimeProvider.System);
            services.AddSingleton<IDisplayFormatter, DisplayFormatter>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IHomeService, HomeService>();

            services.AddScoped<IVenueService, VenueService>();

            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<ISportService, SportService>();

            services.AddScoped<IParticipationService, ParticipationService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IPlayerService, PlayerService>();
            //services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<MatchDataBuilder>();
            services.AddScoped<SportDataBuilder>();
            services.AddScoped<HomeDataBuilder>();
            services.AddScoped<VenueDataBuilder>();
            services.AddScoped<ParticipationDataBuilder>();
            services.AddScoped<RatingDataBuilder>();
            services.AddScoped<PlayerDataBuilder>();
            //services.AddScoped<NotificationDataBuilder>();

            services.AddScoped<AdminDashboardDataBuilder>();
            services.AddScoped<AdminUserDataBuilder>();
            services.AddScoped<AdminSportDataBuilder>();
            services.AddScoped<AdminVenueDataBuilder>();

            return services;
        }
    }
}
