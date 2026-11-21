using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Data;
using PlayOn.Data.Models;
using PlayOn.Web.Extensions;

namespace PlayOn.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("PlayOnDbContext")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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


            builder.Services.AddLocalResources(builder.Configuration);


            var app = builder.Build();

            await IdentitySeeder.SeedAsync(app.Services);

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
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapControllerRoute(
                name: "with-slug",
                pattern: "{controller}/{action}/{id}/{slug}")
                .WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages()
               .WithStaticAssets();

            await app.RunAsync();
        }
    }



    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(IdentitySeeder));

            foreach (var roleName in new[] { RoleNames.User, RoleNames.Administrator })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var created = await roleManager.CreateAsync(new ApplicationRole { Name = roleName });

                    if (!created.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Could not create role '{roleName}': {Describe(created)}");
                    }
                }
            }

            await EnsureEveryoneHasUserRoleAsync(userManager);
            await EnsureAdministratorAsync(userManager, configuration, logger);
        }

        private static async Task EnsureEveryoneHasUserRoleAsync(UserManager<ApplicationUser> userManager)
        {
            var inRole = (await userManager.GetUsersInRoleAsync(RoleNames.User))
                .Select(x => x.Id)
                .ToHashSet();

            var missing = await userManager.Users
                .Where(x => !inRole.Contains(x.Id))
                .ToListAsync();

            foreach (var user in missing)
            {
                await userManager.AddToRoleAsync(user, RoleNames.User);
            }
        }

        private static async Task EnsureAdministratorAsync(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger logger)
        {
            var section = configuration.GetSection("Seed:Admin");

            var email = section["Email"];
            var password = section["Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger.LogInformation("Seed:Admin is not configured; no administrator was seeded.");
                return;
            }

            email = email.Trim();

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email.ToUpperInvariant(),
                    Email = email,
                    EmailConfirmed = true,
                    DisplayName = section["DisplayName"] ?? "Administrator",
                    City = section["City"] ?? "Sofia",
                    IsActive = true
                };

                var created = await userManager.CreateAsync(user, password);

                if (!created.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create the seeded administrator: {Describe(created)}");
                }
            }

            foreach (var roleName in new[] { RoleNames.User, RoleNames.Administrator })
            {
                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        private static string Describe(IdentityResult result)
        {
            return string.Join(" ", result.Errors.Select(x => x.Description));
        }
    }
}
