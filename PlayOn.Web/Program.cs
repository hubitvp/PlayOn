using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using PlayOn.Data;
using PlayOn.Data.Models;
using PlayOn.Data.Seeding;
using PlayOn.Web.Extensions;

namespace PlayOn.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            string connectionStringName = "PlayOnDbContext";

            var connectionString = builder.Configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            builder.Services.AddDbContext<PlayOnDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(connectionString);

                    options.UseAsyncSeeding(async (dbContext, _, ct) =>
                    {
                        using var scope = serviceProvider.CreateScope();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();


                        await PlayOnDataSeeder.SeedAsync(
                            (PlayOnDbContext)dbContext,
                            userManager,
                            roleManager,
                            builder.Environment);
                    });

                    options.UseSeeding((dbContext, _) =>
                    {
                        using var scope = serviceProvider.CreateScope();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();


                        PlayOnDataSeeder.SeedAsync
                        (
                            (PlayOnDbContext)dbContext,
                            userManager,
                            roleManager,
                            builder.Environment
                        )
                        .GetAwaiter()
                        .GetResult();
                    });
                });

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

                options.TimeProvider = TimeProvider.System;
            });

            builder.Services.AddLocalResources(builder.Configuration, builder.Environment);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PlayOnDbContext>();
                await dbContext.Database.MigrateAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Default");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //app.UseStatusCodePagesWithReExecute(
            //    "/Error/Index",
            //    "?statusCode={0}");

            app.UseRouting();
            
            app.UseStatusCodePagesWithRedirects("/Error/Index?statusCode={0}");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();


            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id:guid?}")
                .WithStaticAssets();

            app.MapControllerRoute(
                name: "with-slug",
                pattern: "{controller}/{action}/{id:guid}/{slug:regex(^[a-z0-9-]+$)}")
                .WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id:guid?}")
                .WithStaticAssets();

            app.MapRazorPages()
               .WithStaticAssets();

            await app.RunAsync();
        }
    }
}
