
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
            builder.HasIndex(x => x.NormalizedEmail).IsUnique();

            builder.HasData(
                new ApplicationUser
                {
                    Id = Guid.Parse(SeedUserIds.User001),
                    UserName = "ALEX.MORGAN@EXAMPLE.COM",
                    FullName = "Alex Morgan",
                    DisplayName = "Alex Morgan",
                    Address = "бул. Витоша 45",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Sofia",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "ALEX.MORGAN@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User002),
                    UserName = "JORDAN.LEE@EXAMPLE.COM",
                    FullName = "Jordan Lee",
                    DisplayName = "Jordan Lee",
                    Address = "ул. Иван Вазов 12",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Plovdiv",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 3,
                    NormalizedUserName = "JORDAN.LEE@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User003),
                    UserName = "SAM.TAYLOR@EXAMPLE.COM",
                    FullName = "Sam Taylor",
                    DisplayName = "Sam Taylor",
                    Address = "бул. Княз Борис I 88",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Varna",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "SAM.TAYLOR@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User004),
                    UserName = "CASEY.WILSON@EXAMPLE.COM",
                    FullName = "Casey Wilson",
                    DisplayName = "Casey Wilson",
                    Address = "ул. Александровска 24",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Burgas",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 3,
                    NormalizedUserName = "CASEY.WILSON@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User005),
                    UserName = "MORGAN.KIM@EXAMPLE.COM",
                    FullName = "Morgan Kim",
                    DisplayName = "Morgan Kim",
                    Address = "ул. Граф Игнатиев 18",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Sofia",
                    IsActive = true,
                    ReliabilityScore = 67,
                    MatchesPlayed = 2,
                    NormalizedUserName = "MORGAN.KIM@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User006),
                    UserName = "RILEY.ADAMS@EXAMPLE.COM",
                    FullName = "Riley Adams",
                    DisplayName = "Riley Adams",
                    Address = "ул. Борисова 32",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Ruse",
                    IsActive = true,
                    ReliabilityScore = null,
                    MatchesPlayed = 2,
                    NormalizedUserName = "RILEY.ADAMS@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User007),
                    UserName = "TAYLOR.BROWN@EXAMPLE.COM",
                    FullName = "Taylor Brown",
                    DisplayName = "Taylor Brown",
                    Address = "ул. Цар Асен 15",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Sofia",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 3,
                    NormalizedUserName = "TAYLOR.BROWN@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User008),
                    UserName = "JAMIE.DAVIS@EXAMPLE.COM",
                    FullName = "Jamie Davis",
                    DisplayName = "Jamie Davis",
                    Address = "ул. Христо Г. Данов 27",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Plovdiv",
                    IsActive = true,
                    ReliabilityScore = 0,
                    MatchesPlayed = 0,
                    NormalizedUserName = "JAMIE.DAVIS@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User009),
                    UserName = "QUINN.CLARK@EXAMPLE.COM",
                    FullName = "Quinn Clark",
                    DisplayName = "Quinn Clark",
                    Address = "ул. Преслав 16",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Varna",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "QUINN.CLARK@EXAMPLE.COM",
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
                    Id = Guid.Parse(SeedUserIds.User010),
                    UserName = "DREW.HALL@EXAMPLE.COM",
                    FullName = "Drew Hall",
                    DisplayName = "Drew Hall",
                    Address = "ул. Борисова 54",
                    JoinDate = new DateOnly(2025, 1, 1),
                    City = "Ruse",
                    IsActive = true,
                    ReliabilityScore = 100,
                    MatchesPlayed = 4,
                    NormalizedUserName = "DREW.HALL@EXAMPLE.COM",
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
