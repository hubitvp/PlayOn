using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public const string SeedPassword = "Password123!";

        private const string SeedPasswordHash =
            "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==";

        private const string SeedSecurityStamp = "364894f5-ab00-5eb4-a4a7-40ac99d7f27f";

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.HasData(
                new ApplicationUser
                {
                    Id = SeedUserIds.User001,
                    UserName = "alex.morgan",
                    DisplayName = "Alex Morgan",
                    City = "Sofia",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "ALEX.MORGAN",
                    Email = "alex.morgan@example.com",
                    NormalizedEmail = "ALEX.MORGAN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "0b8d5f31-1f24-4b9b-8c4a-0c1a8c21f001",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User002,
                    UserName = "jordan.lee",
                    DisplayName = "Jordan Lee",
                    City = "Plovdiv",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 3,
                    NormalizedUserName = "JORDAN.LEE",
                    Email = "jordan.lee@example.com",
                    NormalizedEmail = "JORDAN.LEE@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "3e9a1d72-5c66-4a80-9f31-52e2d4b8f002",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User003,
                    UserName = "sam.taylor",
                    DisplayName = "Sam Taylor",
                    City = "Varna",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "SAM.TAYLOR",
                    Email = "sam.taylor@example.com",
                    NormalizedEmail = "SAM.TAYLOR@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "7a4c2e90-8b13-46fd-a275-91c3e6f4a003",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User004,
                    UserName = "casey.wilson",
                    DisplayName = "Casey Wilson",
                    City = "Burgas",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 3,
                    NormalizedUserName = "CASEY.WILSON",
                    Email = "casey.wilson@example.com",
                    NormalizedEmail = "CASEY.WILSON@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "d18f6b42-2a97-4e51-b803-6c7d9a5f4004",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User005,
                    UserName = "morgan.kim",
                    DisplayName = "Morgan Kim",
                    City = "Sofia",
                    IsActive = true,
                    ReliabilityScore = 67,
                    MatchesPlayed = 2,
                    NormalizedUserName = "MORGAN.KIM",
                    Email = "morgan.kim@example.com",
                    NormalizedEmail = "MORGAN.KIM@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "f5b27c68-9d40-43ae-8b16-2e7a4c9d5005",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User006,
                    UserName = "riley.adams",
                    DisplayName = "Riley Adams",
                    City = "Ruse",
                    IsActive = true,
                    ReliabilityScore = null,
                    MatchesPlayed = 2,
                    NormalizedUserName = "RILEY.ADAMS",
                    Email = "riley.adams@example.com",
                    NormalizedEmail = "RILEY.ADAMS@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "6c1e8a53-74b9-4fd2-a605-3b9c7e2f6006",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User007,
                    UserName = "taylor.brown",
                    DisplayName = "Taylor Brown",
                    City = "Sofia",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 3,
                    NormalizedUserName = "TAYLOR.BROWN",
                    Email = "taylor.brown@example.com",
                    NormalizedEmail = "TAYLOR.BROWN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "a92d4f17-6e80-4b35-9c21-7d5a8e3f7007",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User008,
                    UserName = "jamie.davis",
                    DisplayName = "Jamie Davis",
                    City = "Plovdiv",
                    IsActive = true,
                    ReliabilityScore = 0,
                    MatchesPlayed = 0,
                    NormalizedUserName = "JAMIE.DAVIS",
                    Email = "jamie.davis@example.com",
                    NormalizedEmail = "JAMIE.DAVIS@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "4f7b2c19-a638-45de-8b04-6e9d1a5c8008",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User009,
                    UserName = "quinn.clark",
                    DisplayName = "Quinn Clark",
                    City = "Varna",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "QUINN.CLARK",
                    Email = "quinn.clark@example.com",
                    NormalizedEmail = "QUINN.CLARK@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "8e3a6d25-1b74-4c90-a582-9f6b2d7e9009",
                    LockoutEnabled = false
                },
                new ApplicationUser
                {
                    Id = SeedUserIds.User010,
                    UserName = "drew.hall",
                    DisplayName = "Drew Hall",
                    City = "Ruse",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "DREW.HALL",
                    Email = "drew.hall@example.com",
                    NormalizedEmail = "DREW.HALL@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = SeedPasswordHash,
                    SecurityStamp = SeedSecurityStamp,
                    ConcurrencyStamp = "b6c1f843-5a29-47de-9082-4e7d3a1f0010",
                    LockoutEnabled = false
                });
        }

    }
}
