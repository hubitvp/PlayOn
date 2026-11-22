using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlayOn.Data.Seeding
{
    public static class PlayOnDataSeeder
    {
        private const string AlphanumericChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new JsonStringEnumConverter() }
        };

        private static string GenerateAlphanumericPassword(int length = 14)
        {
            var upper = RandomNumberGenerator.GetItems("ABCDEFGHIJKLMNOPQRSTUVWXYZ".AsSpan(), 1)[0];
            var lower = RandomNumberGenerator.GetItems("abcdefghijklmnopqrstuvwxyz".AsSpan(), 1)[0];
            var digit = RandomNumberGenerator.GetItems("0123456789".AsSpan(), 1)[0];
            var rest = RandomNumberGenerator.GetItems(AlphanumericChars.AsSpan(), Math.Max(0, length - 3));

            char[] chars = [upper, lower, digit, .. rest];
            RandomNumberGenerator.Shuffle(chars);
            return new string(chars);
        }

        public static async Task SeedAsync(
            PlayOnDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IHostEnvironment hostEnvironment,
            string? jsonFilePath = null,
            CancellationToken cancellationToken = default)
        {
            var resolvedPath = jsonFilePath
                ?? Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "playon-seed.json");

            var bundle = await LoadOrCreateSeedBundleAsync(resolvedPath, cancellationToken);

            await SeedRolesAsync(roleManager, bundle.Roles, cancellationToken);

            var usersByEmail = await SeedUsersAndRolesAsync(userManager, bundle.Users, cancellationToken);
            var sportsByName = await SeedSportsAsync(context, bundle.Sports, cancellationToken);
            var venues = await SeedVenuesAsync(context, bundle.Venues, cancellationToken);
            var courtsByName = await SeedCourtsAsync(context, bundle.Courts, cancellationToken);

            await SeedSportPreferencesAsync(context, usersByEmail, sportsByName, bundle.SportPreferences, cancellationToken);
            await SeedCourtSportsAsync(context, courtsByName, sportsByName, bundle.CourtSports, cancellationToken);

            var matchesByTitle = await SeedMatchesAsync(context, usersByEmail, bundle.Matches, cancellationToken);

            await SeedEventsAsync(context, matchesByTitle, courtsByName, sportsByName, venues, bundle.Events, cancellationToken);
            await SeedMatchParticipantsAsync(context, matchesByTitle, usersByEmail, bundle.MatchParticipants, cancellationToken);
            await SeedNotificationsAsync(context, matchesByTitle, usersByEmail, bundle.Notifications, cancellationToken);
            await SeedPlayerRatingsAsync(context, matchesByTitle, usersByEmail, bundle.PlayerRatings, cancellationToken);
        }

        private static async Task<SeedDataBundle> LoadOrCreateSeedBundleAsync(
            string filePath,
            CancellationToken cancellationToken)
        {
            SeedDataBundle? loaded = null;

            if (File.Exists(filePath))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(filePath, cancellationToken);
                    loaded = JsonSerializer.Deserialize<SeedDataBundle>(json, JsonOptions);
                }
                catch (JsonException)
                {
                }
            }

            if (loaded != null &&
                (loaded.Roles.Count > 0 || loaded.Users.Count > 0 || loaded.Courts.Count > 0))
            {
                var defaults = CreateDefaultSeedBundle();

                if (loaded.Sports.Count == 0) loaded.Sports = defaults.Sports;
                if (loaded.Venues.Count == 0) loaded.Venues = defaults.Venues;
                if (loaded.SportPreferences.Count == 0) loaded.SportPreferences = defaults.SportPreferences;
                if (loaded.CourtSports.Count == 0) loaded.CourtSports = defaults.CourtSports;
                if (loaded.Events.Count == 0) loaded.Events = defaults.Events;

                await WriteSeedBundleAsync(filePath, loaded, cancellationToken);
                return loaded;
            }

            var defaultBundle = CreateDefaultSeedBundle();
            await WriteSeedBundleAsync(filePath, defaultBundle, cancellationToken);
            return defaultBundle;
        }

        private static async Task WriteSeedBundleAsync(
            string filePath,
            SeedDataBundle bundle,
            CancellationToken cancellationToken)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var writeStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(writeStream, bundle, JsonOptions, cancellationToken);
        }

        private static SeedDataBundle CreateDefaultSeedBundle() => new()
        {
            Roles = RoleNames.All.ToList(),
            Users =
            [
                new("alex.morgan@example.com","alex.morgan@example.com", "Alex Morgan", "Alex Morgan", "бул. Витоша 45", new DateOnly(2025, 1, 1), "Sofia", true, 100, 4, RoleNames.User, GenerateAlphanumericPassword()),
                new("jordan.lee@example.com","jordan.lee@example.com", "Jordan Lee", "Jordan Lee", "ул. Иван Вазов 12", new DateOnly(2025, 1, 1), "Plovdiv", true, 100, 3, RoleNames.User, GenerateAlphanumericPassword()),
                new("sam.taylor@example.com","sam.taylor@example.com", "Sam Taylor", "Sam Taylor", "бул. Княз Борис I 88", new DateOnly(2025, 1, 1), "Varna", true, 100, 4, RoleNames.User, GenerateAlphanumericPassword()),
                new("casey.wilson@example.com","casey.wilson@example.com", "Casey Wilson", "Casey Wilson", "ул. Александровска 24", new DateOnly(2025, 1, 1), "Burgas", true, 100, 3, RoleNames.User, GenerateAlphanumericPassword()),
                new("morgan.kim@example.com","morgan.kim@example.com", "Morgan Kim", "Morgan Kim", "ул. Граф Игнатиев 18", new DateOnly(2025, 1, 1), "Sofia", true, 67, 2, RoleNames.User, GenerateAlphanumericPassword()),
                new("riley.adams@example.com","riley.adams@example.com", "Riley Adams", "Riley Adams", "ул. Борисова 32", new DateOnly(2025, 1, 1), "Ruse", true, null, 2, RoleNames.User, GenerateAlphanumericPassword()),
                new("taylor.brown@example.com","taylor.brown@example.com", "Taylor Brown", "Taylor Brown", "ул. Цар Асен 15", new DateOnly(2025, 1, 1), "Sofia", true, 100, 3, RoleNames.User, GenerateAlphanumericPassword()),
                new("jamie.davis@example.com","jamie.davis@example.com", "Jamie Davis", "Jamie Davis", "ул. Христо Г. Данов 27", new DateOnly(2025, 1, 1), "Plovdiv", true, 0, 0, RoleNames.User, GenerateAlphanumericPassword()),
                new("quinn.clark@example.com","quinn.clark@example.com", "Quinn Clark", "Quinn Clark", "ул. Преслав 16", new DateOnly(2025, 1, 1), "Varna", true, 100, 4, RoleNames.User, GenerateAlphanumericPassword()),
                new("drew.hall@example.com", "drew.hall@example.com", "Drew Hall", "Drew Hall", "ул. Борисова 54", new DateOnly(2025, 1, 1), "Ruse", true, 100, 4, RoleNames.User, GenerateAlphanumericPassword()),
                new("ivan.petrov@example.com","ivan.petrov@example.com", "Ivan Petrov", "Ivan Petrov", "бул. България 12", new DateOnly(2025, 1, 1), "Sofia", true, 100, 0, RoleNames.Administrator, GenerateAlphanumericPassword()),
                new("maria.ivanova@example.com","maria.ivanova@example.com", "Maria Ivanova", "Maria Ivanova", "ул. Граф Игнатиев 28", new DateOnly(2025, 1, 1), "Plovdiv", true, 100, 0, RoleNames.Administrator, GenerateAlphanumericPassword()),
                new("georgi.dimitrov@example.com","georgi.dimitrov@example.com", "Georgi Dimitrov", "Georgi Dimitrov", "ул. Александровска 15", new DateOnly(2025, 1, 1), "Varna", true, 100, 0, RoleNames.Administrator, GenerateAlphanumericPassword())
            ],
            Sports =
            [
                new("Football", SportIcon.Football, 6, 22),
                new("Basketball", SportIcon.Basketball, 4, 10),
                new("Tennis", SportIcon.TableTennisPaddle, 2, 4),
                new("Volleyball", SportIcon.Volleyball, 4, 12),
                new("Padel", SportIcon.TableTennisPaddle, 2, 4),
                new("Badminton", SportIcon.Badminton, 2, 4),
                new("Handball", SportIcon.Handball, 6, 14),
                new("Rugby", SportIcon.Rugby, 10, 30),
                new("Hockey", SportIcon.HockeyPuck, 6, 22),
                new("Baseball", SportIcon.Baseball, 6, 18)
            ],
            Venues =
            [
                new(new DateOnly(2026, 10, 13), new TimeOnly(18, 0), "Central Sports Park", "1 Main Street", "Sofia", 42.697700m, 23.321900m, null, new TimeOnly(7, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 10, 14), new TimeOnly(19, 0), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m, null, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 10, 17), new TimeOnly(10, 0), "Riverside Courts", "8 River Road", "Varna", 43.214100m, 27.914700m, null, new TimeOnly(6, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 10, 18), new TimeOnly(9, 0), "South Field Complex", "24 South Park", "Burgas", 42.504800m, 27.462600m, null, new TimeOnly(7, 30), new TimeOnly(21, 30), false),
                new(new DateOnly(2026, 11, 9), new TimeOnly(18, 30), "Mountain Sports Hall", "5 Vitosha Avenue", "Sofia", 42.650700m, 23.347000m, null, new TimeOnly(8, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 10), new TimeOnly(17, 0), "West End Club", "44 West Street", "Sofia", 42.705000m, 23.270000m, null, new TimeOnly(9, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 11), new TimeOnly(19, 30), "Black Sea Sports", "19 Coast Lane", "Varna", 43.207000m, 27.922000m, null, new TimeOnly(7, 0), new TimeOnly(21, 0), false),
                new(new DateOnly(2026, 11, 14), new TimeOnly(11, 0), "Maritsa Activity Center", "31 Maritsa Road", "Plovdiv", 42.145000m, 24.760000m, null, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 10, 25), new TimeOnly(14, 0), "Green Court Club", "7 Garden Street", "Ruse", 43.835000m, 25.965000m, null, new TimeOnly(7, 30), new TimeOnly(21, 30), false),
                new(new DateOnly(2026, 11, 15), new TimeOnly(15, 0), "Danube Sports Hub", "16 Harbor Avenue", "Ruse", 43.848000m, 25.960000m, null, new TimeOnly(8, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 11, 7), new TimeOnly(10, 0), "Central Sports Park", "1 Main Street", "Sofia", 42.697700m, 23.321900m, null, new TimeOnly(7, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 13), new TimeOnly(20, 0), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m, null, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 16), new TimeOnly(18, 30), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m, null, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 18), new TimeOnly(19, 0), "Mountain Sports Hall", "5 Vitosha Avenue", "Sofia", 42.650700m, 23.347000m, null, new TimeOnly(8, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 22), new TimeOnly(9, 30), "Riverside Courts", "8 River Road", "Varna", 43.214100m, 27.914700m, null, new TimeOnly(6, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 11, 20), new TimeOnly(18, 0), "Central Sports Park", "1 Main Street", "Sofia", 42.697700m, 23.321900m, null, new TimeOnly(7, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 21), new TimeOnly(10, 0), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m, null, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 24), new TimeOnly(19, 0), "Riverside Courts", "8 River Road", "Varna", 43.214100m, 27.914700m, null, new TimeOnly(6, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 11, 27), new TimeOnly(17, 30), "South Field Complex", "24 South Park", "Burgas", 42.504800m, 27.462600m, null, new TimeOnly(7, 30), new TimeOnly(21, 30), false),
                new(new DateOnly(2026, 11, 28), new TimeOnly(11, 0), "Mountain Sports Hall", "5 Vitosha Avenue", "Sofia", 42.650700m, 23.347000m, null, new TimeOnly(8, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 12, 3), new TimeOnly(18, 0), "Danube Sports Hub", "16 Harbor Avenue", "Ruse", 43.848000m, 25.960000m, null, new TimeOnly(8, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 12, 5), new TimeOnly(15, 0), "West End Club", "44 West Street", "Sofia", 42.705000m, 23.270000m, null, new TimeOnly(9, 0), new TimeOnly(22, 0), false)
                ],
            SportPreferences =
            [
                new("alex.morgan@example.com", "Football", SkillLevel.Intermediate),
                new("jordan.lee@example.com", "Basketball", SkillLevel.Advanced),
                new("sam.taylor@example.com", "Tennis", SkillLevel.Beginner),
                new("casey.wilson@example.com", "Volleyball", SkillLevel.Intermediate),
                new("morgan.kim@example.com", "Padel", SkillLevel.Advanced),
                new("riley.adams@example.com", "Badminton", SkillLevel.Beginner),
                new("taylor.brown@example.com", "Handball", SkillLevel.Intermediate),
                new("jamie.davis@example.com", "Rugby", SkillLevel.Beginner),
                new("quinn.clark@example.com", "Hockey", SkillLevel.Advanced),
                new("drew.hall@example.com", "Baseball", SkillLevel.Intermediate),
                new("alex.morgan@example.com", "Basketball", SkillLevel.Intermediate),
                new("casey.wilson@example.com", "Basketball", SkillLevel.Intermediate),
                new("morgan.kim@example.com", "Basketball", SkillLevel.Intermediate),
                new("alex.morgan@example.com", "Padel", SkillLevel.Intermediate),
                new("jordan.lee@example.com", "Padel", SkillLevel.Intermediate),
                new("casey.wilson@example.com", "Padel", SkillLevel.Intermediate),
                new("quinn.clark@example.com", "Padel", SkillLevel.Advanced),
                new("quinn.clark@example.com", "Handball", SkillLevel.Intermediate),
                new("drew.hall@example.com", "Handball", SkillLevel.Intermediate)
                ],
            Courts =
            [
                new("Main Football Pitch", Surface.Artificial, 22, 80m),
                new("Arena Basketball 1", Surface.Hardwood, 10, 45m),
                new("Riverside Tennis 1", Surface.Clay, 4, 25m),
                new("South Volleyball Court", Surface.Sand, 12, 35m),
                new("Mountain Padel 1", Surface.Artificial, 4, 30m),
                new("West Badminton 1", Surface.Hardwood, 4, 20m),
                new("Sea Handball Court", Surface.Concrete, 14, 50m),
                new("Maritsa Rugby Field", Surface.Grass, 30, 90m),
                new("Green Hockey Field", Surface.Artificial, 22, 75m),
                new("Danube Baseball Field", Surface.Grass, 18, 65m)
            ],
            CourtSports =
            [
                new("Main Football Pitch", "Football"),
                new("Arena Basketball 1", "Basketball"),
                new("Riverside Tennis 1", "Tennis"),
                new("South Volleyball Court", "Volleyball"),
                new("Mountain Padel 1", "Padel"),
                new("West Badminton 1", "Badminton"),
                new("Sea Handball Court", "Handball"),
                new("Maritsa Rugby Field", "Rugby"),
                new("Green Hockey Field", "Hockey"),
                new("Danube Baseball Field", "Baseball"),
                new("Arena Basketball 1", "Volleyball"),
                new("Arena Basketball 1", "Handball"),
                new("Sea Handball Court", "Basketball"),
                new("Main Football Pitch", "Hockey")
            ],
            Matches =
            [
                new("Tuesday Evening Football", "alex.morgan@example.com", "Casual five-a-side football.", 90, 6, 22, SkillLevel.Beginner, SkillLevel.Advanced, 8m, MatchStatus.Completed, null),
                new("Downtown Basketball", "jordan.lee@example.com", "Competitive five-on-five game.", 90, 4, 10, SkillLevel.Intermediate, SkillLevel.Advanced, 5m, MatchStatus.Completed, null),
                new("Saturday Tennis Doubles", "sam.taylor@example.com", "Friendly doubles session.", 120, 2, 4, SkillLevel.Beginner, SkillLevel.Intermediate, 6m, MatchStatus.Completed, null),
                new("Beach Volleyball Morning", "casey.wilson@example.com", "Three-on-three beach volleyball.", 90, 6, 12, SkillLevel.Beginner, SkillLevel.Advanced, 7m, MatchStatus.Completed, null),
                new("Padel After Work", "morgan.kim@example.com", "Fast-paced padel games.", 90, 2, 4, SkillLevel.Intermediate, SkillLevel.Advanced, 9m, MatchStatus.Confirmed, null),
                new("Badminton Open Play", "riley.adams@example.com", "Singles and doubles rotation.", 120, 2, 4, SkillLevel.Beginner, SkillLevel.Intermediate, 4m, MatchStatus.Confirmed, null),
                new("Handball Skills Match", "taylor.brown@example.com", "A structured handball session.", 90, 8, 14, SkillLevel.Intermediate, SkillLevel.Advanced, 6m, MatchStatus.Open, null),
                new("Weekend Rugby Touch", "jamie.davis@example.com", "Non-contact touch rugby.", 90, 10, 30, SkillLevel.Beginner, SkillLevel.Intermediate, 8m, MatchStatus.Open, null),
                new("Hockey Training Game", "quinn.clark@example.com", "Small-sided field hockey.", 90, 6, 22, SkillLevel.Beginner, SkillLevel.Advanced, 7m, MatchStatus.Completed, null),
                new("Baseball Practice Game", "drew.hall@example.com", "Relaxed practice game for all levels.", 120, 6, 18, SkillLevel.Beginner, SkillLevel.Intermediate, 5m, MatchStatus.Cancelled, "Venue maintenance"),
                new("Saturday Morning Football", "alex.morgan@example.com", "Friendly kickabout, all levels welcome.", 90, 6, 22, SkillLevel.Beginner, SkillLevel.Advanced, 7m, MatchStatus.Open, null),
                new("Friday Night Basketball", "jordan.lee@example.com", "Full-court games, winners stay on.", 90, 4, 8, SkillLevel.Beginner, SkillLevel.Advanced, 5m, MatchStatus.Confirmed, null),
                new("Monday Volleyball Social", "casey.wilson@example.com", "Relaxed indoor volleyball with mixed teams.", 90, 6, 10, SkillLevel.Beginner, SkillLevel.Intermediate, 6m, MatchStatus.Open, null),
                new("Padel Doubles Evening", "morgan.kim@example.com", "Four players, rotating partners.", 90, 4, 4, SkillLevel.Beginner, SkillLevel.Advanced, 9m, MatchStatus.Open, null),
                new("Sunday Tennis Singles", "sam.taylor@example.com", "Friendly singles on clay, bring your racket.", 60, 2, 4, SkillLevel.Beginner, SkillLevel.Intermediate, 6m, MatchStatus.Confirmed, null)
            ],
            Events =
            [
                new("Tuesday Evening Football", "Football", "Main Football Pitch", 0),
                new("Downtown Basketball", "Basketball", "Arena Basketball 1", 1),
                new("Saturday Tennis Doubles", "Tennis", "Riverside Tennis 1", 2),
                new("Beach Volleyball Morning", "Volleyball", "South Volleyball Court", 3),
                new("Padel After Work", "Padel", "Mountain Padel 1", 4),
                new("Badminton Open Play", "Badminton", "West Badminton 1", 5),
                new("Handball Skills Match", "Handball", "Sea Handball Court", 6),
                new("Weekend Rugby Touch", "Rugby", "Maritsa Rugby Field", 7),
                new("Hockey Training Game", "Hockey", "Green Hockey Field", 8),
                new("Baseball Practice Game", "Baseball", "Danube Baseball Field", 9),
                new("Saturday Morning Football", "Football", "Main Football Pitch", 10),
                new("Friday Night Basketball", "Basketball", "Arena Basketball 1", 11),
                new("Monday Volleyball Social", "Volleyball", "Arena Basketball 1", 12),
                new("Padel Doubles Evening", "Padel", "Mountain Padel 1", 13),
                new("Sunday Tennis Singles", "Tennis", "Riverside Tennis 1", 14)
            ],
            MatchParticipants =
            [
                new("Tuesday Evening Football", "alex.morgan@example.com", ParticipantStatus.Attended, new DateTime(2026, 9, 29, 9, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "jordan.lee@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 4, 0, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "sam.taylor@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 4, 6, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "casey.wilson@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 4, 12, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "morgan.kim@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 4, 18, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "riley.adams@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 5, 0, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "taylor.brown@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 5, 6, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "quinn.clark@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 5, 12, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "drew.hall@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 5, 18, 0, 0), new DateTime(2026, 10, 13, 19, 30, 0), false),
                new("Tuesday Evening Football", "jamie.davis@example.com", ParticipantStatus.Cancelled, new DateTime(2026, 10, 3, 12, 0, 0), new DateTime(2026, 10, 13, 16, 50, 0), true),
                new("Downtown Basketball", "jordan.lee@example.com", ParticipantStatus.Attended, new DateTime(2026, 9, 30, 9, 0, 0), new DateTime(2026, 10, 14, 20, 30, 0), false),
                new("Downtown Basketball", "morgan.kim@example.com", ParticipantStatus.NoShow, new DateTime(2026, 10, 5, 1, 0, 0), new DateTime(2026, 10, 14, 20, 30, 0), false),
                new("Downtown Basketball", "alex.morgan@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 5, 7, 0, 0), new DateTime(2026, 10, 14, 20, 30, 0), false),
                new("Downtown Basketball", "casey.wilson@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 5, 13, 0, 0), new DateTime(2026, 10, 14, 20, 30, 0), false),
                new("Saturday Tennis Doubles", "sam.taylor@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 3, 9, 0, 0), new DateTime(2026, 10, 17, 12, 0, 0), false),
                new("Saturday Tennis Doubles", "quinn.clark@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 7, 16, 0, 0), new DateTime(2026, 10, 17, 12, 0, 0), false),
                new("Saturday Tennis Doubles", "drew.hall@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 7, 22, 0, 0), new DateTime(2026, 10, 17, 12, 0, 0), false),
                new("Saturday Tennis Doubles", "alex.morgan@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 8, 4, 0, 0), new DateTime(2026, 10, 17, 12, 0, 0), false),
                new("Beach Volleyball Morning", "casey.wilson@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 4, 9, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Beach Volleyball Morning", "jamie.davis@example.com", ParticipantStatus.NoShow, new DateTime(2026, 10, 8, 15, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Beach Volleyball Morning", "sam.taylor@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 8, 21, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Beach Volleyball Morning", "morgan.kim@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 9, 3, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Beach Volleyball Morning", "riley.adams@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 9, 9, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Beach Volleyball Morning", "taylor.brown@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 9, 15, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Beach Volleyball Morning", "quinn.clark@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 9, 21, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Beach Volleyball Morning", "drew.hall@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 10, 3, 0, 0), new DateTime(2026, 10, 18, 10, 30, 0), false),
                new("Padel After Work", "morgan.kim@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Padel After Work", "alex.morgan@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Padel After Work", "jordan.lee@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Padel After Work", "casey.wilson@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 9, 0, 0), new DateTime(2026, 10, 30, 9, 0, 0), false),
                new("Padel After Work", "quinn.clark@example.com", ParticipantStatus.Waitlisted, new DateTime(2026, 10, 30, 10, 0, 0), new DateTime(2026, 10, 30, 10, 0, 0), false),
                new("Badminton Open Play", "riley.adams@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Badminton Open Play", "morgan.kim@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Badminton Open Play", "taylor.brown@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Handball Skills Match", "taylor.brown@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Handball Skills Match", "quinn.clark@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Handball Skills Match", "drew.hall@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Weekend Rugby Touch", "jamie.davis@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Weekend Rugby Touch", "sam.taylor@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Weekend Rugby Touch", "casey.wilson@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Weekend Rugby Touch", "morgan.kim@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 9, 0, 0), new DateTime(2026, 10, 30, 9, 0, 0), false),
                new("Weekend Rugby Touch", "riley.adams@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 10, 0, 0), new DateTime(2026, 10, 30, 10, 0, 0), false),
                new("Hockey Training Game", "quinn.clark@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 11, 9, 0, 0), new DateTime(2026, 10, 25, 15, 30, 0), false),
                new("Hockey Training Game", "taylor.brown@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 15, 20, 0, 0), new DateTime(2026, 10, 25, 15, 30, 0), false),
                new("Hockey Training Game", "drew.hall@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 16, 2, 0, 0), new DateTime(2026, 10, 25, 15, 30, 0), false),
                new("Hockey Training Game", "alex.morgan@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 16, 8, 0, 0), new DateTime(2026, 10, 25, 15, 30, 0), false),
                new("Hockey Training Game", "jordan.lee@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 16, 14, 0, 0), new DateTime(2026, 10, 25, 15, 30, 0), false),
                new("Hockey Training Game", "sam.taylor@example.com", ParticipantStatus.Attended, new DateTime(2026, 10, 16, 20, 0, 0), new DateTime(2026, 10, 25, 15, 30, 0), false),
                new("Hockey Training Game", "jamie.davis@example.com", ParticipantStatus.Cancelled, new DateTime(2026, 10, 15, 8, 0, 0), new DateTime(2026, 10, 25, 12, 50, 0), true),
                new("Baseball Practice Game", "drew.hall@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Baseball Practice Game", "taylor.brown@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Baseball Practice Game", "quinn.clark@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Saturday Morning Football", "alex.morgan@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Saturday Morning Football", "jordan.lee@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 28, 16, 0, 0), new DateTime(2026, 10, 28, 16, 0, 0), false),
                new("Saturday Morning Football", "sam.taylor@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 28, 22, 0, 0), new DateTime(2026, 10, 28, 22, 0, 0), false),
                new("Saturday Morning Football", "casey.wilson@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 29, 4, 0, 0), new DateTime(2026, 10, 29, 4, 0, 0), false),
                new("Friday Night Basketball", "jordan.lee@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Friday Night Basketball", "morgan.kim@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Friday Night Basketball", "riley.adams@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Friday Night Basketball", "taylor.brown@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 9, 0, 0), new DateTime(2026, 10, 30, 9, 0, 0), false),
                new("Friday Night Basketball", "quinn.clark@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 10, 0, 0), new DateTime(2026, 10, 30, 10, 0, 0), false),
                new("Friday Night Basketball", "drew.hall@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 11, 0, 0), new DateTime(2026, 10, 30, 11, 0, 0), false),
                new("Friday Night Basketball", "alex.morgan@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 12, 0, 0), new DateTime(2026, 10, 30, 12, 0, 0), false),
                new("Friday Night Basketball", "sam.taylor@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 13, 0, 0), new DateTime(2026, 10, 30, 13, 0, 0), false),
                new("Friday Night Basketball", "casey.wilson@example.com", ParticipantStatus.Waitlisted, new DateTime(2026, 10, 30, 14, 0, 0), new DateTime(2026, 10, 30, 14, 0, 0), false),
                new("Monday Volleyball Social", "casey.wilson@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Monday Volleyball Social", "quinn.clark@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Monday Volleyball Social", "drew.hall@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Monday Volleyball Social", "alex.morgan@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 9, 0, 0), new DateTime(2026, 10, 30, 9, 0, 0), false),
                new("Monday Volleyball Social", "jordan.lee@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 10, 0, 0), new DateTime(2026, 10, 30, 10, 0, 0), false),
                new("Padel Doubles Evening", "morgan.kim@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Padel Doubles Evening", "sam.taylor@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false),
                new("Padel Doubles Evening", "casey.wilson@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 8, 0, 0), new DateTime(2026, 10, 30, 8, 0, 0), false),
                new("Sunday Tennis Singles", "sam.taylor@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 20, 9, 0, 0), new DateTime(2026, 10, 20, 9, 0, 0), false),
                new("Sunday Tennis Singles", "taylor.brown@example.com", ParticipantStatus.Joined, new DateTime(2026, 10, 30, 7, 0, 0), new DateTime(2026, 10, 30, 7, 0, 0), false)
            ],
            Notifications =
            [
                new("drew.hall@example.com", NotificationType.MatchCancelled, "Baseball Practice Game was cancelled: Venue maintenance.", "Baseball Practice Game", false, new DateTime(2026, 10, 30, 9, 0, 0)),
                new("taylor.brown@example.com", NotificationType.MatchCancelled, "Baseball Practice Game was cancelled: Venue maintenance.", "Baseball Practice Game", false, new DateTime(2026, 10, 30, 9, 0, 0)),
                new("quinn.clark@example.com", NotificationType.MatchCancelled, "Baseball Practice Game was cancelled: Venue maintenance.", "Baseball Practice Game", false, new DateTime(2026, 10, 30, 9, 0, 0)),
                new("morgan.kim@example.com", NotificationType.MatchConfirmed, "Padel After Work has enough players and will go ahead.", "Padel After Work", true, new DateTime(2026, 10, 30, 10, 0, 0)),
                new("alex.morgan@example.com", NotificationType.MatchConfirmed, "Padel After Work has enough players and will go ahead.", "Padel After Work", false, new DateTime(2026, 10, 30, 10, 0, 0)),
                new("casey.wilson@example.com", NotificationType.Promoted, "A spot opened in Padel After Work. You moved off the waitlist.", "Padel After Work", false, new DateTime(2026, 10, 30, 18, 30, 0)),
                new("jamie.davis@example.com", NotificationType.StrikeAdded, "You left Hockey Training Game less than 2 hours before the start. A strike was added.", "Hockey Training Game", true, new DateTime(2026, 10, 25, 12, 50, 0)),
                new("morgan.kim@example.com", NotificationType.StrikeAdded, "You were marked as a no-show for Downtown Basketball. A strike was added.", "Downtown Basketball", false, new DateTime(2026, 10, 14, 20, 40, 0)),
                new("jordan.lee@example.com", NotificationType.RatingReceived, "Alex Morgan rated you 5/5 after Tuesday Evening Football.", "Tuesday Evening Football", true, new DateTime(2026, 10, 14, 1, 30, 0)),
                new("alex.morgan@example.com", NotificationType.RatingReceived, "Jordan Lee rated you 3/5 after Downtown Basketball.", "Downtown Basketball", false, new DateTime(2026, 10, 15, 2, 30, 0))
            ],
            PlayerRatings =
            [
                new("Tuesday Evening Football", "alex.morgan@example.com", "jordan.lee@example.com", 5, "Superb teammate, great energy."),
                new("Tuesday Evening Football", "jordan.lee@example.com", "sam.taylor@example.com", 3, "Decent game, nothing special."),
                new("Tuesday Evening Football", "sam.taylor@example.com", "casey.wilson@example.com", 3, "Decent game, nothing special."),
                new("Downtown Basketball", "jordan.lee@example.com", "alex.morgan@example.com", 3, "Decent game, nothing special."),
                new("Downtown Basketball", "alex.morgan@example.com", "casey.wilson@example.com", 3, "Decent game, nothing special."),
                new("Saturday Tennis Doubles", "sam.taylor@example.com", "quinn.clark@example.com", 5, "Superb teammate, great energy."),
                new("Saturday Tennis Doubles", "quinn.clark@example.com", "drew.hall@example.com", 4, null),
                new("Beach Volleyball Morning", "casey.wilson@example.com", "sam.taylor@example.com", 4, null),
                new("Beach Volleyball Morning", "sam.taylor@example.com", "morgan.kim@example.com", 5, "Superb teammate, great energy."),
                new("Hockey Training Game", "quinn.clark@example.com", "taylor.brown@example.com", 5, "Superb teammate, great energy.")
            ]
        };

        private static async Task SeedRolesAsync(
            RoleManager<ApplicationRole> roleManager,
            IReadOnlyList<string> roles,
            CancellationToken cancellationToken)
        {
            foreach (var roleName in roles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationRole
                    {
                        Name = roleName,
                    };

                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Failed to seed role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
        private static async Task<Dictionary<string, ApplicationUser>> SeedUsersAndRolesAsync(
            UserManager<ApplicationUser> userManager,
            IReadOnlyList<SeedUserDto> seedUsers,
            CancellationToken cancellationToken)
        {
            var resultMap = new Dictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in seedUsers)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var user = await userManager.FindByEmailAsync(item.Email);
                if (user == null)
                {
                    if (string.IsNullOrWhiteSpace(item.Password))
                    {
                        continue;
                    }

                    user = new ApplicationUser
                    {
                        UserName = item.Email,
                        Email = item.Email,
                        FullName = item.FullName,
                        DisplayName = item.DisplayName,
                        Address = item.Address,
                        JoinDate = item.JoinDate,
                        City = item.City,
                        IsActive = item.IsActive,
                        ReliabilityScore = item.ReliabilityScore,
                        MatchesPlayed = item.MatchesPlayed,
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };

                    var createResult = await userManager.CreateAsync(user, item.Password);
                    if (!createResult.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Failed to seed user '{item.Email}': {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    }
                }

                if (!string.IsNullOrWhiteSpace(item.Role) && !await userManager.IsInRoleAsync(user, item.Role))
                {
                    var roleResult = await userManager.AddToRoleAsync(user, item.Role);
                    if (!roleResult.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Failed to assign role '{item.Role}' to '{item.Email}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }

                resultMap[item.Email] = user;
            }

            return resultMap;
        }

        private static async Task<Dictionary<string, Sport>> SeedSportsAsync(
            PlayOnDbContext context,
            IReadOnlyList<SeedSportDto> seedSports,
            CancellationToken cancellationToken)
        {
            var existing = await context.Set<Sport>().ToListAsync(cancellationToken);
            var map = existing.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            var toAdd = seedSports.Where(x => !map.ContainsKey(x.Name)).Select(x => new Sport
            {
                Name = x.Name,
                Icon = x.Icon,
                DefaultMinPlayers = x.DefaultMinPlayers,
                DefaultMaxPlayers = x.DefaultMaxPlayers
            }).ToList();
            if (toAdd.Count > 0)
            {
                await context.Set<Sport>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                foreach (var item in toAdd) map[item.Name] = item;
            }
            return map;
        }

        private static async Task<List<Venue>> SeedVenuesAsync(
            PlayOnDbContext context,
            IReadOnlyList<SeedVenueDto> seedVenues,
            CancellationToken cancellationToken)
        {
            var existing = await context.Set<Venue>().ToListAsync(cancellationToken);
            var map = existing.ToDictionary(
                x => (x.Name, x.Address, x.City, x.Date, x.StartsAt));
            var ordered = new List<Venue>(seedVenues.Count);
            var toAdd = new List<Venue>();
            foreach (var dto in seedVenues)
            {
                var key = (dto.Name, dto.Address, dto.City, dto.Date, dto.StartsAt);
                if (!map.TryGetValue(key, out var venue))
                {
                    venue = new Venue
                    {
                        Date = dto.Date,
                        StartsAt = dto.StartsAt,
                        Name = dto.Name,
                        Address = dto.Address,
                        City = dto.City,
                        Latitude = dto.Latitude,
                        Longitude = dto.Longitude,
                        PhotoUrl = dto.PhotoUrl,
                        OpensAt = dto.OpensAt,
                        ClosesAt = dto.ClosesAt,
                        IsDeleted = dto.IsDeleted
                    };
                    map[key] = venue;
                    toAdd.Add(venue);
                }
                ordered.Add(venue);
            }
            if (toAdd.Count > 0)
            {
                await context.Set<Venue>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
            return ordered;
        }

        private static async Task SeedSportPreferencesAsync(
            PlayOnDbContext context,
            Dictionary<string, ApplicationUser> usersByEmail,
            Dictionary<string, Sport> sportsByName,
            IReadOnlyList<SeedSportPreferenceDto> preferences,
            CancellationToken cancellationToken)
        {
            var existing = (await context.Set<SportPreference>()
                .Select(x => new { x.UserId, x.SportId })
                .ToListAsync(cancellationToken))
                .Select(x => (x.UserId, x.SportId)).ToHashSet();
            var toAdd = new List<SportPreference>();
            foreach (var item in preferences)
            {
                if (!usersByEmail.TryGetValue(item.UserEmail, out var user) ||
                    !sportsByName.TryGetValue(item.SportName, out var sport)) continue;
                if (existing.Add((user.Id, sport.Id)))
                    toAdd.Add(new SportPreference { UserId = user.Id, SportId = sport.Id, SkillLevel = item.SkillLevel });
            }
            if (toAdd.Count > 0)
            {
                await context.Set<SportPreference>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        private static async Task<Dictionary<string, Court>> SeedCourtsAsync(
            PlayOnDbContext context,
            IReadOnlyList<SeedCourtDto> seedCourts,
            CancellationToken cancellationToken)
        {
            var existingCourts = await context.Set<Court>().ToListAsync(cancellationToken);
            var map = existingCourts.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            var toAdd = new List<Court>();
            foreach (var dto in seedCourts)
            {
                if (!map.ContainsKey(dto.Name))
                {
                    toAdd.Add(new Court
                    {
                        Name = dto.Name,
                        Surface = dto.Surface,
                        Capacity = dto.Capacity,
                        HourlyPrice = dto.HourlyPrice
                    });
                }
            }

            if (toAdd.Count > 0)
            {
                await context.Set<Court>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                foreach (var added in toAdd)
                {
                    map[added.Name] = added;
                }
            }

            return map;
        }

        private static async Task SeedCourtSportsAsync(
            PlayOnDbContext context,
            Dictionary<string, Court> courtsByName,
            Dictionary<string, Sport> sportsByName,
            IReadOnlyList<SeedCourtSportDto> pairs,
            CancellationToken cancellationToken)
        {
            var existingKeys = (await context.Set<CourtSport>()
                .Select(x => new { x.CourtId, x.SportId })
                .ToListAsync(cancellationToken))
                .Select(x => (x.CourtId, x.SportId))
                .ToHashSet();

            var toAdd = new List<CourtSport>();
            foreach (var pair in pairs)
            {
                if (!courtsByName.TryGetValue(pair.CourtName, out var court) ||
                    !sportsByName.TryGetValue(pair.SportName, out var sport))
                {
                    continue;
                }

                if (!existingKeys.Contains((court.Id, sport.Id)))
                {
                    toAdd.Add(new CourtSport { CourtId = court.Id, SportId = sport.Id });
                    existingKeys.Add((court.Id, sport.Id));
                }
            }

            if (toAdd.Count > 0)
            {
                await context.Set<CourtSport>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        private static async Task<Dictionary<string, Match>> SeedMatchesAsync(
            PlayOnDbContext context,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedMatchDto> seedMatches,
            CancellationToken cancellationToken)
        {
            var existingMatches = await context.Set<Match>().ToListAsync(cancellationToken);
            var map = existingMatches.ToDictionary(m => m.Title, StringComparer.OrdinalIgnoreCase);

            var toAdd = new List<Match>();
            foreach (var item in seedMatches)
            {
                if (map.ContainsKey(item.Title) || !usersByEmail.TryGetValue(item.OrganiserEmail, out var organiser))
                {
                    continue;
                }

                toAdd.Add(new Match
                {
                    OrganiserId = organiser.Id,
                    Title = item.Title,
                    Description = item.Description,
                    DurationMinutes = item.DurationMinutes,
                    MinPlayers = item.MinPlayers,
                    MaxPlayers = item.MaxPlayers,
                    MinSkill = item.MinSkill,
                    MaxSkill = item.MaxSkill,
                    PricePerPlayer = item.PricePerPlayer,
                    Status = item.Status,
                    CancelReason = item.CancelReason,
                    IsHidden = false,
                    IsDeleted = false
                });
            }

            if (toAdd.Count > 0)
            {
                await context.Set<Match>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                foreach (var added in toAdd)
                {
                    map[added.Title] = added;
                }
            }

            return map;
        }

        private static async Task SeedEventsAsync(
            PlayOnDbContext context,
            Dictionary<string, Match> matchesByTitle,
            Dictionary<string, Court> courtsByName,
            Dictionary<string, Sport> sportsByName,
            IReadOnlyList<Venue> venues,
            IReadOnlyList<SeedEventDto> seedEvents,
            CancellationToken cancellationToken)
        {
            if (venues.Count == 0)
            {
                return;
            }

            var existingKeys = (await context.Set<Event>()
                .Select(x => new { x.MatchId, x.SportId, x.CourtId, x.VenueId })
                .ToListAsync(cancellationToken))
                .Select(x => (x.MatchId, x.SportId, x.CourtId, x.VenueId))
                .ToHashSet();

            var toAdd = new List<Event>();
            foreach (var item in seedEvents)
            {
                if (!matchesByTitle.TryGetValue(item.MatchTitle, out var match) ||
                    !sportsByName.TryGetValue(item.SportName, out var sport) ||
                    !courtsByName.TryGetValue(item.CourtName, out var court) ||
                    item.VenueIndex < 0 || item.VenueIndex >= venues.Count)
                {
                    continue;
                }

                var venue = venues[item.VenueIndex];
                if (!existingKeys.Contains((match.Id, sport.Id, court.Id, venue.Id)))
                {
                    toAdd.Add(new Event
                    {
                        MatchId = match.Id,
                        SportId = sport.Id,
                        CourtId = court.Id,
                        VenueId = venue.Id
                    });
                    existingKeys.Add((match.Id, sport.Id, court.Id, venue.Id));
                }
            }

            if (toAdd.Count > 0)
            {
                await context.Set<Event>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        private static async Task SeedMatchParticipantsAsync(
            PlayOnDbContext context,
            Dictionary<string, Match> matchesByTitle,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedMatchParticipantDto> seedParticipants,
            CancellationToken cancellationToken)
        {
            var existingPairs = (await context.Set<MatchParticipant>()
                .Select(x => new { x.MatchId, x.UserId })
                .ToListAsync(cancellationToken))
                .Select(x => (x.MatchId, x.UserId))
                .ToHashSet();

            var toAdd = new List<MatchParticipant>();
            foreach (var item in seedParticipants)
            {
                if (!matchesByTitle.TryGetValue(item.MatchTitle, out var match) ||
                    !usersByEmail.TryGetValue(item.UserEmail, out var user))
                {
                    continue;
                }

                if (!existingPairs.Contains((match.Id, user.Id)))
                {
                    toAdd.Add(new MatchParticipant
                    {
                        MatchId = match.Id,
                        UserId = user.Id,
                        Status = item.Status,
                        JoinedOn = item.JoinedOn,
                        StatusChangedOn = item.StatusChangedOn,
                        IsLateCancel = item.IsLateCancel
                    });
                    existingPairs.Add((match.Id, user.Id));
                }
            }

            if (toAdd.Count > 0)
            {
                await context.Set<MatchParticipant>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        private static async Task SeedNotificationsAsync(
            PlayOnDbContext context,
            Dictionary<string, Match> matchesByTitle,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedNotificationDto> seedNotifications,
            CancellationToken cancellationToken)
        {
            var existingKeys = (await context.Set<Notification>()
                .Select(x => new { x.UserId, x.MatchId, x.Message, x.CreatedOn })
                .ToListAsync(cancellationToken))
                .Select(x => (x.UserId, x.MatchId, x.Message, x.CreatedOn))
                .ToHashSet();

            var toAdd = new List<Notification>();
            foreach (var item in seedNotifications)
            {
                if (!usersByEmail.TryGetValue(item.UserEmail, out var user) ||
                    !matchesByTitle.TryGetValue(item.MatchTitle, out var match))
                {
                    continue;
                }

                var key = (user.Id, (Guid?)match.Id, item.Message, item.CreatedOn);
                if (!existingKeys.Contains(key))
                {
                    toAdd.Add(new Notification
                    {
                        UserId = user.Id,
                        Type = item.Type,
                        Message = item.Message,
                        MatchId = match.Id,
                        IsRead = item.IsRead,
                        CreatedOn = item.CreatedOn
                    });
                    existingKeys.Add(key);
                }
            }

            if (toAdd.Count > 0)
            {
                await context.Set<Notification>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        private static async Task SeedPlayerRatingsAsync(
            PlayOnDbContext context,
            Dictionary<string, Match> matchesByTitle,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedPlayerRatingDto> seedRatings,
            CancellationToken cancellationToken)
        {
            var existingKeys = (await context.Set<PlayerRating>()
                .Select(x => new { x.MatchId, x.RaterId, x.RatedUserId })
                .ToListAsync(cancellationToken))
                .Select(x => (x.MatchId, x.RaterId, x.RatedUserId))
                .ToHashSet();

            var toAdd = new List<PlayerRating>();
            foreach (var item in seedRatings)
            {
                if (!matchesByTitle.TryGetValue(item.MatchTitle, out var match) ||
                    !usersByEmail.TryGetValue(item.RaterEmail, out var rater) ||
                    !usersByEmail.TryGetValue(item.RatedEmail, out var ratedUser))
                {
                    continue;
                }

                if (!existingKeys.Contains((match.Id, rater.Id, ratedUser.Id)))
                {
                    toAdd.Add(new PlayerRating
                    {
                        MatchId = match.Id,
                        RaterId = rater.Id,
                        RatedUserId = ratedUser.Id,
                        Score = item.Score,
                        Comment = item.Comment,
                        IsHidden = false
                    });
                    existingKeys.Add((match.Id, rater.Id, ratedUser.Id));
                }
            }

            if (toAdd.Count > 0)
            {
                await context.Set<PlayerRating>().AddRangeAsync(toAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}