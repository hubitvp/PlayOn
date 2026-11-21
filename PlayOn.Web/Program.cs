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
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id:guid?}")
                .WithStaticAssets();

            app.MapControllerRoute(
                name: "with-slug",
                pattern: "{controller}/{action}/{id:guid}/{slug:regex(^[a-z-]+$)}")
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
