using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayOn.Data;
using PlayOn.Data.Models;
using PlayOn.Services;
using PlayOn.Services.Contracts;
using PlayOn.Web.DataBuilders;
using PlayOn.Web.DataBuilders.Formatting;

namespace PlayOn.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("PlayOnDbContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<PlayOnDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;

                    builder.Configuration
                        .GetSection("Identity:Password")
                        .Bind(options.Password);
                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<PlayOnDbContext>();

            builder.Services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(
                        new AutoValidateAntiforgeryTokenAttribute()
                    );
                });


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy =
                    CookieSecurePolicy.Always;

                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddSingleton<IDisplayFormatter, DisplayFormatter>();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IHomeService, HomeService>();

            builder.Services.AddScoped<IVenueService, VenueService>();

            builder.Services.AddScoped<IMatchService, MatchService>();
            builder.Services.AddScoped<ISportService, SportService>();

            builder.Services.AddScoped<IParticipationService, ParticipationService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddScoped<MatchDataBuilder>();
            builder.Services.AddScoped<SportDataBuilder>();
            builder.Services.AddScoped<HomeDataBuilder>();
            builder.Services.AddScoped<VenueDataBuilder>();
            builder.Services.AddScoped<ParticipationDataBuilder>();
            builder.Services.AddScoped<RatingDataBuilder>();
            builder.Services.AddScoped<PlayerDataBuilder>();
            builder.Services.AddScoped<NotificationDataBuilder>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error/Default");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseStatusCodePagesWithRedirects("/Error/Index?statusCode={0}");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
