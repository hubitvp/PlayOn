using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Data.RepositoryImplementation;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlayOn.Data.Seeding
{
    /// <summary>
    /// Seeds the database with roles, demo users and sample data (sports, venues, courts, matches, events, participants, notifications and ratings).
    /// </summary>
    /// <remarks>
    /// The data comes from a JSON seed file (<c>playon-seed.json</c>) that is created from built-in defaults on the first run and kept between runs.
    /// The file contains the randomly generated passwords of the demo accounts in plain text, so it is intended for development and demo environments only.
    /// </remarks>
    public static class PlayOnDataSeeder
    {
        /// <summary>
        /// Characters used when generating random passwords: Latin letters (upper and lower case) and digits.
        /// </summary>
        private const string AlphanumericChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// JSON options for reading and writing the seed file: indented output, case-insensitive property names, relaxed escaping (so non-Latin text such as Cyrillic stays readable) and enums written as strings.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// Generates a random alphanumeric password using a cryptographically secure random number generator. It always contains at least one uppercase letter, one lowercase letter and one digit, and its characters are shuffled.
        /// </summary>
        /// <param name="length">Total length of the password; at least 3 characters are produced. Defaults to 14.</param>
        /// <returns>The generated password.</returns>
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

        /// <summary>
        /// Seeds the database using repositories created over the given context. Convenience overload that creates a repository for every entity and delegates to the repository-based overload.
        /// </summary>
        /// <param name="context">The database context used to create the repositories.</param>
        /// <param name="userManager">Manager used to create users and assign roles.</param>
        /// <param name="roleManager">Manager used to create roles.</param>
        /// <param name="hostEnvironment">The hosting environment, used to locate the content root for the default seed file.</param>
        /// <param name="jsonFilePath">Optional path of the seed JSON file; defaults to <c>SeedData/playon-seed.json</c> under the content root.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <c>null</c>.</exception>
        public static async Task SeedAsync(
            PlayOnDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IHostEnvironment hostEnvironment,
            string? jsonFilePath = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sportRepo = new Repository<Sport>(context);
            var venueRepo = new Repository<Venue>(context);
            var courtRepo = new Repository<Court>(context);
            var sportPreferenceRepo = new Repository<SportPreference>(context);
            var courtSportRepo = new Repository<CourtSport>(context);
            var matchRepo = new Repository<Match>(context);
            var eventRepo = new Repository<Event>(context);
            var matchParticipantRepo = new Repository<MatchParticipant>(context);
            var notificationRepo = new Repository<Notification>(context);
            var playerRatingRepo = new Repository<PlayerRating>(context);

            await SeedAsync(
                sportRepo,
                venueRepo,
                courtRepo,
                sportPreferenceRepo,
                courtSportRepo,
                matchRepo,
                eventRepo,
                matchParticipantRepo,
                notificationRepo,
                playerRatingRepo,
                userManager,
                roleManager,
                hostEnvironment,
                jsonFilePath,
                cancellationToken);
        }

        /// <summary>
        /// Seeds the database with roles, users and sample data. It loads (or creates) the JSON seed bundle and then seeds roles, users, sports, venues, courts, sport preferences, court-sport mappings, matches, events, participants, notifications and player ratings, in dependency order. Seeding can be run repeatedly: entries that already exist are skipped.
        /// </summary>
        /// <param name="sportRepository">Repository used to access sports.</param>
        /// <param name="venueRepository">Repository used to access venues.</param>
        /// <param name="courtRepository">Repository used to access courts.</param>
        /// <param name="sportPreferenceRepository">Repository used to access sport preferences.</param>
        /// <param name="courtSportRepository">Repository used to access court-sport mappings.</param>
        /// <param name="matchRepository">Repository used to access matches.</param>
        /// <param name="eventRepository">Repository used to access events.</param>
        /// <param name="matchParticipantRepository">Repository used to access match participants.</param>
        /// <param name="notificationRepository">Repository used to access notifications.</param>
        /// <param name="playerRatingRepository">Repository used to access player ratings.</param>
        /// <param name="userManager">Manager used to create users and assign roles.</param>
        /// <param name="roleManager">Manager used to create roles.</param>
        /// <param name="hostEnvironment">The hosting environment, used to locate the content root for the default seed file.</param>
        /// <param name="jsonFilePath">Optional path of the seed JSON file; defaults to <c>SeedData/playon-seed.json</c> under the content root.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <exception cref="ArgumentNullException">Thrown when a required argument is <c>null</c>.</exception>
        public static async Task SeedAsync(
            IRepository<Sport> sportRepository,
            IRepository<Venue> venueRepository,
            IRepository<Court> courtRepository,
            IRepository<SportPreference> sportPreferenceRepository,
            IRepository<CourtSport> courtSportRepository,
            IRepository<Match> matchRepository,
            IRepository<Event> eventRepository,
            IRepository<MatchParticipant> matchParticipantRepository,
            IRepository<Notification> notificationRepository,
            IRepository<PlayerRating> playerRatingRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IHostEnvironment hostEnvironment,
            string? jsonFilePath = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(sportRepository);
            ArgumentNullException.ThrowIfNull(venueRepository);
            ArgumentNullException.ThrowIfNull(courtRepository);
            ArgumentNullException.ThrowIfNull(sportPreferenceRepository);
            ArgumentNullException.ThrowIfNull(courtSportRepository);
            ArgumentNullException.ThrowIfNull(matchRepository);
            ArgumentNullException.ThrowIfNull(eventRepository);
            ArgumentNullException.ThrowIfNull(matchParticipantRepository);
            ArgumentNullException.ThrowIfNull(notificationRepository);
            ArgumentNullException.ThrowIfNull(playerRatingRepository);
            ArgumentNullException.ThrowIfNull(userManager);
            ArgumentNullException.ThrowIfNull(roleManager);
            ArgumentNullException.ThrowIfNull(hostEnvironment);

            var resolvedPath = jsonFilePath
                ?? Path.Combine(hostEnvironment.ContentRootPath, "SeedData", "playon-seed.json");

            var bundle = await LoadOrCreateSeedBundleAsync(resolvedPath, cancellationToken);

            await SeedRolesAsync(roleManager, bundle.Roles, cancellationToken);

            var usersByEmail = await SeedUsersAndRolesAsync(userManager, bundle.Users, cancellationToken);
            var sportsByName = await SeedSportsAsync(sportRepository, bundle.Sports, cancellationToken);
            var venues = await SeedVenuesAsync(venueRepository, bundle.Venues, cancellationToken);
            var courtsByName = await SeedCourtsAsync(courtRepository, bundle.Courts, cancellationToken);

            await SeedSportPreferencesAsync(sportPreferenceRepository, usersByEmail, sportsByName, bundle.SportPreferences, cancellationToken);
            await SeedCourtSportsAsync(courtSportRepository, courtsByName, sportsByName, bundle.CourtSports, cancellationToken);

            var matchesByTitle = await SeedMatchesAsync(matchRepository, usersByEmail, bundle.Matches, cancellationToken);

            await SeedEventsAsync(eventRepository, matchesByTitle, courtsByName, sportsByName, venues, bundle.Events, cancellationToken);
            await SeedMatchParticipantsAsync(matchParticipantRepository, matchesByTitle, usersByEmail, bundle.MatchParticipants, cancellationToken);
            await SeedNotificationsAsync(notificationRepository, matchesByTitle, usersByEmail, bundle.Notifications, cancellationToken);
            await SeedPlayerRatingsAsync(playerRatingRepository, matchesByTitle, usersByEmail, bundle.PlayerRatings, cancellationToken);
        }

        /// <summary>
        /// Registers a (match, user) key in the set of known keys.
        /// </summary>
        /// <param name="matchId">Identifier of the match.</param>
        /// <param name="userId">Identifier of the user.</param>
        /// <param name="knownIds">The set of (match, user) keys already registered; the new key is added to it.</param>
        /// <returns><c>true</c> if the key was added; <c>false</c> if an identifier is empty or the key is already registered.</returns>
        private static bool TryRegisterGuid(Guid matchId, Guid userId, HashSet<(Guid, Guid)> knownIds)
        {
            if (matchId == Guid.Empty || userId == Guid.Empty)
            {
                return false;
            }

            return knownIds.Add((matchId, userId));
        }

        /// <summary>
        /// Registers an identifier in the set of known identifiers.
        /// </summary>
        /// <param name="id">The identifier to register.</param>
        /// <param name="knownIds">The set of identifiers already registered; the new identifier is added to it.</param>
        /// <returns><c>true</c> if the identifier was added; <c>false</c> if it is empty or already registered.</returns>
        private static bool TryRegisterGuid(Guid id, HashSet<Guid> knownIds)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            return knownIds.Add(id);
        }


        /// <summary>
        /// Registers a (match, rater, rated user) key in the set of known keys.
        /// </summary>
        /// <param name="matchId">Identifier of the match.</param>
        /// <param name="raterId">Identifier of the user giving the rating.</param>
        /// <param name="ratedUserId">Identifier of the user being rated.</param>
        /// <param name="knownIds">The set of (match, rater, rated user) keys already registered; the new key is added to it.</param>
        /// <returns><c>true</c> if the key was added; <c>false</c> if an identifier is empty or the key is already registered.</returns>
        private static bool TryRegisterGuid(Guid matchId, Guid raterId, Guid ratedUserId, HashSet<(Guid, Guid, Guid)> knownIds)
        {
            if (matchId == Guid.Empty || raterId == Guid.Empty || ratedUserId == Guid.Empty)
            {
                return false;
            }

            return knownIds.Add((matchId, raterId, ratedUserId));
        }



        /// <summary>
        /// Loads the seed bundle from the JSON file or creates it from the built-in defaults. When the file exists and contains data, the defaults are merged in (new demo users, missing sections and the generated six-month demo data) and the file is written back. Otherwise, including when the file cannot be parsed, the default bundle is created and written to the file.
        /// </summary>
        /// <param name="filePath">Path of the seed JSON file.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>The seed bundle to use.</returns>
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

                var hadMatches = loaded.Matches.Count > 0;
                var hadVenues = loaded.Venues.Count > 0;

                // Merge newly introduced demo accounts into an existing JSON bundle.
                // This matters because playon-seed.json is persisted after its first run.
                var knownUserEmails = loaded.Users
                    .Select(x => x.Email)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                loaded.Users.AddRange(defaults.Users.Where(x =>
                    !string.IsNullOrWhiteSpace(x.Email) && knownUserEmails.Add(x.Email)));
                if (loaded.Sports.Count == 0) loaded.Sports = defaults.Sports;
                if (loaded.Venues.Count == 0)
                {
                    // Copy only the original venue catalogue. The default bundle also
                    // contains generated PlayOn venues, which are added below.
                    loaded.Venues = defaults.Venues
                        .Where(x => !x.Name.StartsWith("PlayOn ", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                if (loaded.Courts.Count == 0) loaded.Courts = defaults.Courts;
                if (loaded.SportPreferences.Count == 0) loaded.SportPreferences = defaults.SportPreferences;
                if (loaded.CourtSports.Count == 0) loaded.CourtSports = defaults.CourtSports;

                if (!hadMatches)
                {
                    loaded.Matches = defaults.Matches
                        .Where(x => !x.Title.StartsWith("PlayOn ", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    if (loaded.Events.Count == 0 && !hadVenues)
                    {
                        // These event indexes correspond to the original venue catalogue.
                        loaded.Events = defaults.Events;
                    }
                }

                // Do not copy defaults.Events into an existing venue list: VenueIndex is
                // positional, so those indexes would point at unrelated venues.
                AddSixMonthDemoData(loaded);

                await WriteSeedBundleAsync(filePath, loaded, cancellationToken);
                return loaded;
            }

            var defaultBundle = CreateDefaultSeedBundle();
            await WriteSeedBundleAsync(filePath, defaultBundle, cancellationToken);
            return defaultBundle;
        }

        /// <summary>
        /// Writes the seed bundle to the given JSON file, creating its directory if needed. An existing file is overwritten.
        /// </summary>
        /// <param name="filePath">Path of the seed JSON file.</param>
        /// <param name="bundle">The seed data bundle.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
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

        /// <summary>
        /// Creates the built-in seed bundle: roles, demo users (with randomly generated passwords), sports, venues, sport preferences, courts, court-sport mappings, matches, events, participants, notifications and ratings, plus the generated six-month demo data.
        /// </summary>
        /// <returns>The default seed bundle.</returns>
        private static SeedDataBundle CreateDefaultSeedBundle()
        {
            var bundle = new SeedDataBundle
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
                new("olivia.ross@example.com", "olivia.ross@example.com", "Olivia Ross", "Olivia Ross", "ул. Шипка 18", new DateOnly(2026, 1, 1), "Sofia", true, 100, 2, RoleNames.User, GenerateAlphanumericPassword()),
                new("noah.wright@example.com", "noah.wright@example.com", "Noah Wright", "Noah Wright", "ул. Капитан Райчо 22", new DateOnly(2026, 1, 1), "Plovdiv", true, 100, 3, RoleNames.User, GenerateAlphanumericPassword()),
                new("ava.mitchell@example.com", "ava.mitchell@example.com", "Ava Mitchell", "Ava Mitchell", "ул. Генерал Колев 31", new DateOnly(2026, 1, 1), "Varna", true, 100, 2, RoleNames.User, GenerateAlphanumericPassword()),
                new("ethan.scott@example.com", "ethan.scott@example.com", "Ethan Scott", "Ethan Scott", "ул. Александровска 41", new DateOnly(2026, 1, 1), "Burgas", true, 100, 4, RoleNames.User, GenerateAlphanumericPassword()),
                new("sophia.green@example.com", "sophia.green@example.com", "Sophia Green", "Sophia Green", "ул. Оборище 9", new DateOnly(2026, 1, 1), "Sofia", true, 100, 3, RoleNames.User, GenerateAlphanumericPassword()),
                new("lucas.baker@example.com", "lucas.baker@example.com", "Lucas Baker", "Lucas Baker", "ул. Борисова 67", new DateOnly(2026, 1, 1), "Ruse", true, 100, 2, RoleNames.User, GenerateAlphanumericPassword()),
                new("mia.turner@example.com", "mia.turner@example.com", "Mia Turner", "Mia Turner", "ул. Преслав 28", new DateOnly(2026, 1, 1), "Varna", true, 100, 3, RoleNames.User, GenerateAlphanumericPassword()),
                new("mason.carter@example.com", "mason.carter@example.com", "Mason Carter", "Mason Carter", "ул. Христо Г. Данов 14", new DateOnly(2026, 1, 1), "Plovdiv", true, 100, 2, RoleNames.User, GenerateAlphanumericPassword()),
                new("isabella.phillips@example.com", "isabella.phillips@example.com", "Isabella Phillips", "Isabella Phillips", "ул. Цар Асен 33", new DateOnly(2026, 1, 1), "Sofia", true, 100, 3, RoleNames.User, GenerateAlphanumericPassword()),
                new("logan.parker@example.com", "logan.parker@example.com", "Logan Parker", "Logan Parker", "ул. Борисова 19", new DateOnly(2026, 1, 1), "Ruse", true, 100, 2, RoleNames.User, GenerateAlphanumericPassword()),
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
                new(new DateOnly(2026, 10, 13), new TimeOnly(18, 0), "Central Sports Park", "1 Main Street", "Sofia", 42.697700m, 23.321900m,  new TimeOnly(7, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 10, 14), new TimeOnly(19, 0), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m,  new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 10, 17), new TimeOnly(10, 0), "Riverside Courts", "8 River Road", "Varna", 43.214100m, 27.914700m,  new TimeOnly(6, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 10, 18), new TimeOnly(9, 0), "South Field Complex", "24 South Park", "Burgas", 42.504800m, 27.462600m, new TimeOnly(7, 30), new TimeOnly(21, 30), false),
                new(new DateOnly(2026, 11, 9), new TimeOnly(18, 30), "Mountain Sports Hall", "5 Vitosha Avenue", "Sofia", 42.650700m, 23.347000m,  new TimeOnly(8, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 10), new TimeOnly(17, 0), "West End Club", "44 West Street", "Sofia", 42.705000m, 23.270000m,  new TimeOnly(9, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 11), new TimeOnly(19, 30), "Black Sea Sports", "19 Coast Lane", "Varna", 43.207000m, 27.922000m,  new TimeOnly(7, 0), new TimeOnly(21, 0), false),
                new(new DateOnly(2026, 11, 14), new TimeOnly(11, 0), "Maritsa Activity Center", "31 Maritsa Road", "Plovdiv", 42.145000m, 24.760000m, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 10, 25), new TimeOnly(14, 0), "Green Court Club", "7 Garden Street", "Ruse", 43.835000m, 25.965000m, new TimeOnly(7, 30), new TimeOnly(21, 30), false),
                new(new DateOnly(2026, 11, 15), new TimeOnly(15, 0), "Danube Sports Hub", "16 Harbor Avenue", "Ruse", 43.848000m, 25.960000m, new TimeOnly(8, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 11, 7), new TimeOnly(10, 0), "Central Sports Park", "1 Main Street", "Sofia", 42.697700m, 23.321900m, new TimeOnly(7, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 13), new TimeOnly(20, 0), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 16), new TimeOnly(18, 30), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 18), new TimeOnly(19, 0), "Mountain Sports Hall", "5 Vitosha Avenue", "Sofia", 42.650700m, 23.347000m, new TimeOnly(8, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 22), new TimeOnly(9, 30), "Riverside Courts", "8 River Road", "Varna", 43.214100m, 27.914700m, new TimeOnly(6, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 11, 20), new TimeOnly(18, 0), "Central Sports Park", "1 Main Street", "Sofia", 42.697700m, 23.321900m, new TimeOnly(7, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 11, 21), new TimeOnly(10, 0), "Eastside Arena", "12 East Boulevard", "Plovdiv", 42.135400m, 24.745300m, new TimeOnly(8, 0), new TimeOnly(22, 0), false),
                new(new DateOnly(2026, 11, 24), new TimeOnly(19, 0), "Riverside Courts", "8 River Road", "Varna", 43.214100m, 27.914700m, new TimeOnly(6, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 11, 27), new TimeOnly(17, 30), "South Field Complex", "24 South Park", "Burgas", 42.504800m, 27.462600m, new TimeOnly(7, 30), new TimeOnly(21, 30), false),
                new(new DateOnly(2026, 11, 28), new TimeOnly(11, 0), "Mountain Sports Hall", "5 Vitosha Avenue", "Sofia", 42.650700m, 23.347000m, new TimeOnly(8, 0), new TimeOnly(23, 0), false),
                new(new DateOnly(2026, 12, 3), new TimeOnly(18, 0), "Danube Sports Hub", "16 Harbor Avenue", "Ruse", 43.848000m, 25.960000m, new TimeOnly(8, 30), new TimeOnly(22, 30), false),
                new(new DateOnly(2026, 12, 5), new TimeOnly(15, 0), "West End Club", "44 West Street", "Sofia", 42.705000m, 23.270000m, new TimeOnly(9, 0), new TimeOnly(22, 0), false)
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

            AddSixMonthDemoData(bundle);

            return bundle;
        }

        /// <summary>
        /// Extends the bundle with generated demo data so that every account has player-facing sample data: sport preferences, upcoming matches scheduled twice a week for the next six months (each with its own venue and event), shared community, waitlist and cancelled matches, completed history matches with attendance and ratings, and notifications. It can be run repeatedly: existing titles are preserved and no new future schedule is generated once enough demo matches exist.
        /// </summary>
        /// <param name="bundle">The bundle to extend with demo data.</param>
        private static void AddSixMonthDemoData(SeedDataBundle bundle)
        {
            // Every account in the seed bundle receives player-facing sample data,
            // including administrator accounts, so every seeded profile can be tested.
            var users = bundle.Users
                .Where(user => !string.IsNullOrWhiteSpace(user.Email))
                .ToArray();

            var sports = bundle.Sports.ToArray();
            if (users.Length == 0 || sports.Length == 0 || bundle.Courts.Count == 0)
            {
                return;
            }

            // Give each account several sport preferences.
            for (var userIndex = 0; userIndex < users.Length; userIndex++)
            {
                for (var offset = 0; offset < Math.Min(3, sports.Length); offset++)
                {
                    var sport = sports[(userIndex + offset) % sports.Length];
                    if (!bundle.SportPreferences.Any(x =>
                        string.Equals(x.UserEmail, users[userIndex].Email, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(x.SportName, sport.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        bundle.SportPreferences.Add(new SeedSportPreferenceDto(
                            users[userIndex].Email,
                            sport.Name,
                            (SkillLevel)((userIndex + offset) % Enum.GetValues<SkillLevel>().Length)));
                    }
                }
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            var endDate = today.AddMonths(6);
            var futureSlots = new List<(DateOnly Date, TimeOnly Time)>();

            // Two events per week, up to (and never beyond) six months from today.
            for (var week = 0; ; week++)
            {
                var firstDate = today.AddDays(1 + (week * 7));
                if (firstDate > endDate)
                {
                    break;
                }

                futureSlots.Add((firstDate, new TimeOnly(18, 0)));

                var secondDate = firstDate.AddDays(3);
                if (secondDate <= endDate)
                {
                    futureSlots.Add((secondDate, new TimeOnly(10, 0)));
                }
            }

            // The first thirteen future matches are organised by the thirteen
            // seeded accounts. The next three provide shared upcoming, waitlist,
            // and cancelled examples. Remaining slots broaden the match catalogue.
            // The JSON bundle is persisted between application starts. Avoid
            // appending another six-month schedule on every subsequent day.
            var existingFutureDemoMatchCount = bundle.Matches.Count(x =>
                x.Title.StartsWith("PlayOn ", StringComparison.OrdinalIgnoreCase) &&
                !x.Title.StartsWith("PlayOn History ", StringComparison.OrdinalIgnoreCase));
            var futureMatchCount = existingFutureDemoMatchCount >= 52
                ? 0
                : Math.Min(52, futureSlots.Count);
            var football = sports.FirstOrDefault(x => x.Name.Equals("Football", StringComparison.OrdinalIgnoreCase)) ?? sports[0];
            var tennis = sports.FirstOrDefault(x => x.Name.Equals("Tennis", StringComparison.OrdinalIgnoreCase)) ?? sports[0];

            var communityTitle = "PlayOn Community Football Meetup";
            var waitlistTitleA = "PlayOn Popular Tennis Session A";
            var waitlistTitleB = "PlayOn Popular Tennis Session B";
            var cancelledTitle = "PlayOn Cancelled Community Session";

            for (var i = 0; i < futureMatchCount; i++)
            {
                var slot = futureSlots[i];
                var city = users[i % users.Length].City;
                var sport = i switch
                {
                    13 => football,
                    14 or 15 => tennis,
                    _ => sports[i % sports.Length]
                };

                var courtSport = bundle.CourtSports.FirstOrDefault(x =>
                    x.SportName.Equals(sport.Name, StringComparison.OrdinalIgnoreCase));
                if (courtSport is null)
                {
                    // A missing court/sport mapping should not produce an Event
                    // that violates the Event -> CourtSport relationship.
                    continue;
                }

                var organiser = users[i % users.Length];
                var title = i switch
                {
                    13 => communityTitle,
                    14 => waitlistTitleA,
                    15 => waitlistTitleB,
                    16 => cancelledTitle,
                    _ when i < users.Length => $"PlayOn {organiser.DisplayName} Match {slot.Date:yyyyMMdd}",
                    _ => $"PlayOn {sport.Name} Session {slot.Date:yyyyMMdd} {i + 1}"
                };

                // Preserve an existing title if this method is run again against
                // a previously generated JSON bundle.
                if (bundle.Matches.Any(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var isCancelled = title == cancelledTitle;
                var isWaitlistMatch = title == waitlistTitleA || title == waitlistTitleB;
                var maxPlayers = isWaitlistMatch
                    ? 4
                    : title == communityTitle || isCancelled
                        ? Math.Max(22, sport.DefaultMaxPlayers)
                        : Math.Max(sport.DefaultMinPlayers, sport.DefaultMaxPlayers);
                var minPlayers = isWaitlistMatch
                    ? 2
                    : Math.Min(sport.DefaultMinPlayers, maxPlayers);
                var status = isCancelled
                    ? MatchStatus.Cancelled
                    : (i % 3 == 0 ? MatchStatus.Confirmed : MatchStatus.Open);

                bundle.Matches.Add(new SeedMatchDto(
                    title,
                    organiser.Email,
                    isCancelled
                        ? "This demo match was cancelled so the cancelled-match views can be tested."
                        : $"Friendly {sport.Name.ToLowerInvariant()} session. All skill levels welcome.",
                    90,
                    minPlayers,
                    maxPlayers,
                    SkillLevel.Beginner,
                    SkillLevel.Advanced,
                    sport.Name.Equals("Football", StringComparison.OrdinalIgnoreCase) ? 8m : 6m,
                    status,
                    isCancelled ? "Organiser unavailable" : null));

                var venueIndex = bundle.Venues.Count;
                var (latitude, longitude) = GetCityCoordinates(city);
                bundle.Venues.Add(new SeedVenueDto(
                    slot.Date,
                    slot.Time,
                    $"PlayOn {city} Sports Centre {i + 1}",
                    $"Sports Complex {i + 1}, {city}",
                    city,
                    latitude,
                    longitude,
                    new TimeOnly(7, 0),
                    new TimeOnly(23, 0),
                    false));

                bundle.Events.Add(new SeedEventDto(
                    title,
                    sport.Name,
                    courtSport.CourtName,
                    venueIndex));
            }

            if (users.Length == 0)
            {
                return;
            }

            EnsureEachUserHasCreatedMatch(bundle, users, sports, futureSlots);

            var now = DateTime.Now;

            // All users join one upcoming match, so each account has an upcoming
            // participation in addition to the match they organise themselves.
            AddParticipants(
                bundle,
                communityTitle,
                users,
                ParticipantStatus.Joined,
                now.AddDays(-2),
                now.AddDays(-2));

            // Two full tennis matches distribute waitlist membership across all
            // accounts while keeping the four confirmed places within capacity.
            AddWaitlistParticipants(bundle, waitlistTitleA, users, joinedIndexes: [0, 1, 2, 3], now);
            AddWaitlistParticipants(bundle, waitlistTitleB, users, joinedIndexes: [4, 5, 6, 7], now);

            // Every account appears on a cancelled match as a cancelled participant.
            AddParticipants(
                bundle,
                cancelledTitle,
                users,
                ParticipantStatus.Cancelled,
                now.AddDays(-5),
                now.AddDays(-1));

            // Add two completed matches with all seeded accounts in attendance.
            var historyTitles = new[]
            {
                "PlayOn History Football",
                "PlayOn History Rugby"
            };
            var historySports = new[] { football, sports.FirstOrDefault(x => x.Name.Equals("Rugby", StringComparison.OrdinalIgnoreCase)) ?? football };

            for (var historyIndex = 0; historyIndex < historyTitles.Length; historyIndex++)
            {
                var title = historyTitles[historyIndex];
                if (!bundle.Matches.Any(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
                {
                    var sport = historySports[historyIndex];
                    var courtSport = bundle.CourtSports.FirstOrDefault(x =>
                        x.SportName.Equals(sport.Name, StringComparison.OrdinalIgnoreCase));

                    if (courtSport is not null)
                    {
                        var organiser = users[historyIndex % users.Length];
                        bundle.Matches.Add(new SeedMatchDto(
                            title,
                            organiser.Email,
                            "Completed demo match used for player history and rating examples.",
                            90,
                            Math.Min(sport.DefaultMinPlayers, sport.DefaultMaxPlayers),
                            Math.Max(22, sport.DefaultMaxPlayers),
                            SkillLevel.Beginner,
                            SkillLevel.Advanced,
                            7m,
                            MatchStatus.Completed,
                            null));

                        var date = today.AddDays(historyIndex == 0 ? -14 : -35);
                        var venueIndex = bundle.Venues.Count;
                        var (latitude, longitude) = GetCityCoordinates(organiser.City);
                        bundle.Venues.Add(new SeedVenueDto(
                            date,
                            new TimeOnly(18, 0),
                            $"PlayOn History Venue {historyIndex + 1}",
                            $"History Sports Ground {historyIndex + 1}, {organiser.City}",
                            organiser.City,
                            latitude,
                            longitude,
                            new TimeOnly(7, 0),
                            new TimeOnly(23, 0),
                            false));
                        bundle.Events.Add(new SeedEventDto(title, sport.Name, courtSport.CourtName, venueIndex));
                    }
                }

                AddParticipants(
                    bundle,
                    title,
                    users,
                    ParticipantStatus.Attended,
                    now.AddDays(historyIndex == 0 ? -16 : -37),
                    now.AddDays(historyIndex == 0 ? -14 : -35));

                AddCyclicRatings(bundle, title, users);
            }

            // Give each account notifications for confirmation, cancellation,
            // and a rating received. Existing notifications are retained.
            foreach (var user in users)
            {
                AddNotificationIfMissing(
                    bundle,
                    user.Email,
                    NotificationType.MatchConfirmed,
                    "Your PlayOn Community Football Meetup is confirmed.",
                    communityTitle,
                    now.AddMinutes(-user.Email.Length));
                AddNotificationIfMissing(
                    bundle,
                    user.Email,
                    NotificationType.MatchCancelled,
                    "PlayOn Cancelled Community Session was cancelled: Organiser unavailable.",
                    cancelledTitle,
                    now.AddMinutes(-30 - user.Email.Length));
                AddNotificationIfMissing(
                    bundle,
                    user.Email,
                    NotificationType.RatingReceived,
                    "You received a player rating after a completed PlayOn match.",
                    historyTitles[0],
                    now.AddMinutes(-60 - user.Email.Length));
            }
        }

        /// <summary>
        /// Makes sure every user organises at least one match by generating an upcoming match, with its venue and event, for each user who has none.
        /// </summary>
        /// <param name="bundle">The seed data bundle.</param>
        /// <param name="users">The seed users.</param>
        /// <param name="sports">The seed sports.</param>
        /// <param name="futureSlots">Available future date and time slots.</param>
        private static void EnsureEachUserHasCreatedMatch(
            SeedDataBundle bundle,
            IReadOnlyList<SeedUserDto> users,
            IReadOnlyList<SeedSportDto> sports,
            IReadOnlyList<(DateOnly Date, TimeOnly Time)> futureSlots)
        {
            if (futureSlots.Count == 0 || sports.Count == 0)
            {
                return;
            }

            var slotIndex = 0;

            foreach (var user in users)
            {
                if (bundle.Matches.Any(x =>
                    x.OrganiserEmail.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var sport = sports[slotIndex % sports.Count];
                var courtSport = bundle.CourtSports.FirstOrDefault(x =>
                    x.SportName.Equals(sport.Name, StringComparison.OrdinalIgnoreCase));

                if (courtSport is null)
                {
                    slotIndex++;
                    continue;
                }

                var slot = futureSlots[slotIndex % futureSlots.Count];
                var title = $"PlayOn {user.DisplayName} Match {slot.Date:yyyyMMdd}";
                var suffix = 2;
                while (bundle.Matches.Any(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
                {
                    title = $"PlayOn {user.DisplayName} Match {slot.Date:yyyyMMdd} ({suffix++})";
                }

                bundle.Matches.Add(new SeedMatchDto(
                    title,
                    user.Email,
                    $"Friendly {sport.Name.ToLowerInvariant()} session organised by {user.DisplayName}.",
                    90,
                    sport.DefaultMinPlayers,
                    sport.DefaultMaxPlayers,
                    SkillLevel.Beginner,
                    SkillLevel.Advanced,
                    sport.Name.Equals("Football", StringComparison.OrdinalIgnoreCase) ? 8m : 6m,
                    MatchStatus.Open,
                    null));

                var (latitude, longitude) = GetCityCoordinates(user.City);
                var venueIndex = bundle.Venues.Count;
                bundle.Venues.Add(new SeedVenueDto(
                    slot.Date,
                    slot.Time,
                    $"PlayOn {user.City} Community Venue {user.Email.Split('@')[0]}",
                    $"Community Sports Ground, {user.City}",
                    user.City,
                    latitude,
                    longitude,
                    new TimeOnly(7, 0),
                    new TimeOnly(23, 0),
                    false));

                bundle.Events.Add(new SeedEventDto(
                    title,
                    sport.Name,
                    courtSport.CourtName,
                    venueIndex));

                slotIndex++;
            }
        }

        /// <summary>
        /// Gets the approximate coordinates of a known city; unknown cities default to the coordinates of Sofia.
        /// </summary>
        /// <param name="city">Name of the city.</param>
        /// <returns>The latitude and longitude of the city.</returns>
        private static (decimal Latitude, decimal Longitude) GetCityCoordinates(string city)
        {
            return city.ToLowerInvariant() switch
            {
                "sofia" => (42.6977m, 23.3219m),
                "plovdiv" => (42.1354m, 24.7453m),
                "varna" => (43.2141m, 27.9147m),
                "burgas" => (42.5048m, 27.4626m),
                "ruse" => (43.8350m, 25.9650m),
                _ => (42.6977m, 23.3219m)
            };
        }

        /// <summary>
        /// Adds every user to the match with the given status, skipping users who are already participants.
        /// </summary>
        /// <param name="bundle">The seed data bundle.</param>
        /// <param name="matchTitle">Title of the match.</param>
        /// <param name="users">The users to add as participants.</param>
        /// <param name="status">Participation status to assign.</param>
        /// <param name="joinedOn">Date and time the users joined the match.</param>
        /// <param name="statusChangedOn">Date and time the status last changed.</param>
        private static void AddParticipants(
            SeedDataBundle bundle,
            string matchTitle,
            IReadOnlyList<SeedUserDto> users,
            ParticipantStatus status,
            DateTime joinedOn,
            DateTime statusChangedOn)
        {
            foreach (var user in users)
            {
                if (!bundle.MatchParticipants.Any(x =>
                    x.MatchTitle.Equals(matchTitle, StringComparison.OrdinalIgnoreCase) &&
                    x.UserEmail.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    bundle.MatchParticipants.Add(new SeedMatchParticipantDto(
                        matchTitle,
                        user.Email,
                        status,
                        joinedOn,
                        statusChangedOn,
                        false));
                }
            }
        }

        /// <summary>
        /// Adds every user to the match: the users at the given indexes as joined and all others as waitlisted. Users who are already participants are skipped.
        /// </summary>
        /// <param name="bundle">The seed data bundle.</param>
        /// <param name="matchTitle">Title of the match.</param>
        /// <param name="users">The users to add as participants.</param>
        /// <param name="joinedIndexes">Indexes (in the users list) of the users who get a place in the match; everyone else is waitlisted.</param>
        /// <param name="now">The reference date and time used to compute the participation dates.</param>
        private static void AddWaitlistParticipants(
            SeedDataBundle bundle,
            string matchTitle,
            IReadOnlyList<SeedUserDto> users,
            IReadOnlyCollection<int> joinedIndexes,
            DateTime now)
        {
            for (var i = 0; i < users.Count; i++)
            {
                var status = joinedIndexes.Contains(i)
                    ? ParticipantStatus.Joined
                    : ParticipantStatus.Waitlisted;

                if (!bundle.MatchParticipants.Any(x =>
                    x.MatchTitle.Equals(matchTitle, StringComparison.OrdinalIgnoreCase) &&
                    x.UserEmail.Equals(users[i].Email, StringComparison.OrdinalIgnoreCase)))
                {
                    bundle.MatchParticipants.Add(new SeedMatchParticipantDto(
                        matchTitle,
                        users[i].Email,
                        status,
                        now.AddDays(-3),
                        now.AddDays(-3),
                        false));
                }
            }
        }

        /// <summary>
        /// Adds ratings between users arranged in a circle (each user rates the next one) with scores that vary by position. Does nothing for fewer than two users.
        /// </summary>
        /// <param name="bundle">The seed data bundle.</param>
        /// <param name="matchTitle">Title of the match the ratings are for.</param>
        /// <param name="users">The users arranged in a circle.</param>
        private static void AddCyclicRatings(
            SeedDataBundle bundle,
            string matchTitle,
            IReadOnlyList<SeedUserDto> users)
        {
            if (users.Count < 2)
            {
                return;
            }

            for (var i = 0; i < users.Count; i++)
            {
                var next = users[(i + 1) % users.Count];
                var previous = users[(i + users.Count - 1) % users.Count];
                AddRatingIfMissing(bundle, matchTitle, users[i].Email, next.Email, 4 + (i % 2));
                AddRatingIfMissing(bundle, matchTitle, previous.Email, users[i].Email, 3 + (i % 3));
            }
        }

        /// <summary>
        /// Adds a rating unless one already exists for the same match, rater and rated user.
        /// </summary>
        /// <param name="bundle">The seed data bundle.</param>
        /// <param name="matchTitle">Title of the match the rating is for.</param>
        /// <param name="raterEmail">Email address of the user giving the rating.</param>
        /// <param name="ratedEmail">Email address of the user being rated.</param>
        /// <param name="score">Score of the rating.</param>
        private static void AddRatingIfMissing(
            SeedDataBundle bundle,
            string matchTitle,
            string raterEmail,
            string ratedEmail,
            int score)
        {
            if (!bundle.PlayerRatings.Any(x =>
                x.MatchTitle.Equals(matchTitle, StringComparison.OrdinalIgnoreCase) &&
                x.RaterEmail.Equals(raterEmail, StringComparison.OrdinalIgnoreCase) &&
                x.RatedEmail.Equals(ratedEmail, StringComparison.OrdinalIgnoreCase)))
            {
                bundle.PlayerRatings.Add(new SeedPlayerRatingDto(
                    matchTitle,
                    raterEmail,
                    ratedEmail,
                    score,
                    "Good teammate. Enjoyed playing together."));
            }
        }

        /// <summary>
        /// Adds an unread notification unless one of the same type already exists for the user and match.
        /// </summary>
        /// <param name="bundle">The seed data bundle.</param>
        /// <param name="userEmail">Email address of the user.</param>
        /// <param name="type">Type of the notification.</param>
        /// <param name="message">Text of the notification.</param>
        /// <param name="matchTitle">Title of the match the notification relates to.</param>
        /// <param name="createdOn">Date and time the notification was created.</param>
        private static void AddNotificationIfMissing(
            SeedDataBundle bundle,
            string userEmail,
            NotificationType type,
            string message,
            string matchTitle,
            DateTime createdOn)
        {
            if (!bundle.Notifications.Any(x =>
                x.UserEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase) &&
                x.Type == type &&
                x.MatchTitle.Equals(matchTitle, StringComparison.OrdinalIgnoreCase)))
            {
                bundle.Notifications.Add(new SeedNotificationDto(
                    userEmail,
                    type,
                    message,
                    matchTitle,
                    false,
                    createdOn));
            }
        }

        /// <summary>
        /// Creates the given roles that do not exist yet.
        /// </summary>
        /// <param name="roleManager">Manager used to create roles.</param>
        /// <param name="roles">Names of the roles to create.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <exception cref="InvalidOperationException">Thrown when a role cannot be created.</exception>
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

        /// <summary>
        /// Creates the seed users that do not exist yet (with a confirmed email and lockout disabled) and assigns their roles. A user that does not exist and has no password is skipped.
        /// </summary>
        /// <param name="userManager">Manager used to create users and assign roles.</param>
        /// <param name="seedUsers">The users to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>The users keyed by email address (case-insensitive).</returns>
        /// <exception cref="InvalidOperationException">Thrown when a user cannot be created or a role cannot be assigned.</exception>
        private static async Task<Dictionary<string, ApplicationUser>> SeedUsersAndRolesAsync(
            UserManager<ApplicationUser> userManager,
            IReadOnlyList<SeedUserDto> seedUsers,
            CancellationToken cancellationToken)
        {
            var resultMap = new Dictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase);
            var seenUserIds = new HashSet<Guid>();

            foreach (var item in seedUsers)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var user = await userManager.FindByEmailAsync(item.Email);
                if (user != null)
                {
                    if (!TryRegisterGuid(user.Id, seenUserIds))
                    {
                        continue;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(item.Password))
                    {
                        continue;
                    }

                    user = new ApplicationUser
                    {
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

                    if (!TryRegisterGuid(user.Id, seenUserIds))
                    {
                        continue;
                    }

                    await userManager.SetUserNameAsync(user, item.Email);
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

        /// <summary>
        /// Adds the sports that do not exist yet (matched by name).
        /// </summary>
        /// <param name="sportRepository">Repository used to access sports.</param>
        /// <param name="seedSports">The sports to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>All sports keyed by name (case-insensitive).</returns>
        private static async Task<Dictionary<string, Sport>> SeedSportsAsync(
            IRepository<Sport> sportRepository,
            IReadOnlyList<SeedSportDto> seedSports,
            CancellationToken cancellationToken)
        {
            var existing = await sportRepository.Query().ToListAsync(cancellationToken);
            var knownIds = existing.Select(x => x.Id).Where(id => id != Guid.Empty).ToHashSet();
            var map = existing.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            var toAdd = new List<Sport>();
            foreach (var dto in seedSports)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (map.ContainsKey(dto.Name))
                {
                    continue;
                }

                var entity = new Sport
                {
                    Name = dto.Name,
                    Icon = dto.Icon,
                    DefaultMinPlayers = dto.DefaultMinPlayers,
                    DefaultMaxPlayers = dto.DefaultMaxPlayers
                };

                if (!TryRegisterGuid(entity.Id, knownIds))
                {
                    continue;
                }

                map[entity.Name] = entity;
                toAdd.Add(entity);
            }

            if (toAdd.Count > 0)
            {
                await sportRepository.AddRangeAsync(toAdd, cancellationToken);
                await sportRepository.SaveChangesAsync(cancellationToken);
            }

            return map;
        }

        /// <summary>
        /// Adds the venues that do not exist yet (matched by name, address, city, date and start time).
        /// </summary>
        /// <param name="venueRepository">Repository used to access venues.</param>
        /// <param name="seedVenues">The venues to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>The venues in the order of the seed venue list, so that events can refer to them by position.</returns>
        private static async Task<List<Venue>> SeedVenuesAsync(
            IRepository<Venue> venueRepository,
            IReadOnlyList<SeedVenueDto> seedVenues,
            CancellationToken cancellationToken)
        {
            var existing = await venueRepository.Query().ToListAsync(cancellationToken);
            var knownIds = existing.Select(x => x.Id).Where(id => id != Guid.Empty).ToHashSet();
            var map = existing.ToDictionary(
                x => (x.Name, x.Address, x.City, x.Date, x.StartsAt));

            var ordered = new List<Venue>(seedVenues.Count);
            var toAdd = new List<Venue>();

            foreach (var dto in seedVenues)
            {
                cancellationToken.ThrowIfCancellationRequested();

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
                        OpensAt = dto.OpensAt,
                        ClosesAt = dto.ClosesAt,
                        IsDeleted = dto.IsDeleted
                    };

                    if (!TryRegisterGuid(venue.Id, knownIds))
                    {
                        continue;
                    }

                    map[key] = venue;
                    toAdd.Add(venue);
                }

                ordered.Add(venue);
            }

            if (toAdd.Count > 0)
            {
                await venueRepository.AddRangeAsync(toAdd, cancellationToken);
                await venueRepository.SaveChangesAsync(cancellationToken);
            }

            return ordered;
        }

        /// <summary>
        /// Adds the user sport preferences that do not exist yet. Entries whose user or sport cannot be resolved are skipped.
        /// </summary>
        /// <param name="sportPreferenceRepository">Repository used to access sport preferences.</param>
        /// <param name="usersByEmail">Users keyed by email address.</param>
        /// <param name="sportsByName">Sports keyed by name.</param>
        /// <param name="preferences">The sport preferences to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        private static async Task SeedSportPreferencesAsync(
            IRepository<SportPreference> sportPreferenceRepository,
            Dictionary<string, ApplicationUser> usersByEmail,
            Dictionary<string, Sport> sportsByName,
            IReadOnlyList<SeedSportPreferenceDto> preferences,
            CancellationToken cancellationToken)
        {
            var existingPairs = (await sportPreferenceRepository.Query()
                .Select(x => new { x.UserId, x.SportId })
                .ToListAsync(cancellationToken))
                .Where(x => x.UserId != Guid.Empty && x.SportId != Guid.Empty)
                .Select(x => (x.UserId, x.SportId))
                .ToHashSet();

            var toAdd = new List<SportPreference>();
            foreach (var item in preferences)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!usersByEmail.TryGetValue(item.UserEmail, out var user) ||
                    !sportsByName.TryGetValue(item.SportName, out var sport))
                {
                    continue;
                }

                if (user.Id == Guid.Empty || sport.Id == Guid.Empty)
                {
                    continue;
                }

                var pair = (user.Id, sport.Id);
                if (!existingPairs.Add(pair))
                {
                    continue;
                }

                toAdd.Add(new SportPreference
                {
                    UserId = user.Id,
                    SportId = sport.Id,
                    SkillLevel = item.SkillLevel
                });
            }

            if (toAdd.Count > 0)
            {
                await sportPreferenceRepository.AddRangeAsync(toAdd, cancellationToken);
                await sportPreferenceRepository.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Adds the courts that do not exist yet (matched by name).
        /// </summary>
        /// <param name="courtRepository">Repository used to access courts.</param>
        /// <param name="seedCourts">The courts to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>All courts keyed by name (case-insensitive).</returns>
        private static async Task<Dictionary<string, Court>> SeedCourtsAsync(
            IRepository<Court> courtRepository,
            IReadOnlyList<SeedCourtDto> seedCourts,
            CancellationToken cancellationToken)
        {
            var existingCourts = await courtRepository.Query().ToListAsync(cancellationToken);
            var knownIds = existingCourts.Select(x => x.Id).Where(id => id != Guid.Empty).ToHashSet();
            var map = existingCourts.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            var toAdd = new List<Court>();
            foreach (var dto in seedCourts)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (map.ContainsKey(dto.Name))
                {
                    continue;
                }

                var entity = new Court
                {
                    Name = dto.Name,
                    Surface = dto.Surface,
                    Capacity = dto.Capacity,
                    HourlyPrice = dto.HourlyPrice
                };

                if (!TryRegisterGuid(entity.Id, knownIds))
                {
                    continue;
                }

                map[entity.Name] = entity;
                toAdd.Add(entity);
            }

            if (toAdd.Count > 0)
            {
                await courtRepository.AddRangeAsync(toAdd, cancellationToken);
                await courtRepository.SaveChangesAsync(cancellationToken);
            }

            return map;
        }

        /// <summary>
        /// Adds the court-sport mappings that do not exist yet. Entries whose court or sport cannot be resolved are skipped.
        /// </summary>
        /// <param name="courtSportRepository">Repository used to access court-sport mappings.</param>
        /// <param name="courtsByName">Courts keyed by name.</param>
        /// <param name="sportsByName">Sports keyed by name.</param>
        /// <param name="pairs">The court-sport mappings to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        private static async Task SeedCourtSportsAsync(
            IRepository<CourtSport> courtSportRepository,
            Dictionary<string, Court> courtsByName,
            Dictionary<string, Sport> sportsByName,
            IReadOnlyList<SeedCourtSportDto> pairs,
            CancellationToken cancellationToken)
        {
            var existingKeys = (await courtSportRepository.Query()
                .Select(x => new { x.CourtId, x.SportId })
                .ToListAsync(cancellationToken))
                .Where(x => x.CourtId != Guid.Empty && x.SportId != Guid.Empty)
                .Select(x => (x.CourtId, x.SportId))
                .ToHashSet();

            var toAdd = new List<CourtSport>();
            foreach (var pair in pairs)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!courtsByName.TryGetValue(pair.CourtName, out var court) ||
                    !sportsByName.TryGetValue(pair.SportName, out var sport))
                {
                    continue;
                }

                if (court.Id == Guid.Empty || sport.Id == Guid.Empty)
                {
                    continue;
                }

                var key = (court.Id, sport.Id);
                if (!existingKeys.Add(key))
                {
                    continue;
                }

                toAdd.Add(new CourtSport
                {
                    CourtId = court.Id,
                    SportId = sport.Id
                });
            }

            if (toAdd.Count > 0)
            {
                await courtSportRepository.AddRangeAsync(toAdd, cancellationToken);
                await courtSportRepository.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Adds the matches that do not exist yet (matched by title). Matches whose organiser cannot be resolved are skipped.
        /// </summary>
        /// <param name="matchRepository">Repository used to access matches.</param>
        /// <param name="usersByEmail">Users keyed by email address.</param>
        /// <param name="seedMatches">The matches to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        /// <returns>All matches keyed by title (case-insensitive).</returns>
        private static async Task<Dictionary<string, Match>> SeedMatchesAsync(
            IRepository<Match> matchRepository,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedMatchDto> seedMatches,
            CancellationToken cancellationToken)
        {
            var existingMatches = await matchRepository.Query().ToListAsync(cancellationToken);
            var knownIds = existingMatches.Select(m => m.Id).Where(id => id != Guid.Empty).ToHashSet();
            var map = existingMatches.ToDictionary(m => m.Title, StringComparer.OrdinalIgnoreCase);

            var toAdd = new List<Match>();
            foreach (var item in seedMatches)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (map.ContainsKey(item.Title) || !usersByEmail.TryGetValue(item.OrganiserEmail, out var organiser))
                {
                    continue;
                }

                var entity = new Match
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
                    IsDeleted = false
                };

                if (!TryRegisterGuid(entity.Id, knownIds))
                {
                    continue;
                }

                map[entity.Title] = entity;
                toAdd.Add(entity);
            }

            if (toAdd.Count > 0)
            {
                await matchRepository.AddRangeAsync(toAdd, cancellationToken);
                await matchRepository.SaveChangesAsync(cancellationToken);
            }

            return map;
        }

        /// <summary>
        /// Adds the events that link matches to a sport, a court and a venue (the venue is picked by its position in the venue list). Events that cannot be resolved, that already exist or whose venue is already used by another event are skipped.
        /// </summary>
        /// <param name="eventRepository">Repository used to access events.</param>
        /// <param name="matchesByTitle">Matches keyed by title.</param>
        /// <param name="courtsByName">Courts keyed by name.</param>
        /// <param name="sportsByName">Sports keyed by name.</param>
        /// <param name="venues">The persisted venues, in the order of the seed venue list; events refer to them by position.</param>
        /// <param name="seedEvents">The events to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        private static async Task SeedEventsAsync(
            IRepository<Event> eventRepository,
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

            var existingRows = await eventRepository.Query()
                .Select(x => new { x.MatchId, x.SportId, x.CourtId, x.VenueId })
                .ToListAsync(cancellationToken);

            var existingKeys = existingRows
                .Where(x => x.MatchId != Guid.Empty
                    && x.SportId != Guid.Empty
                    && x.CourtId != Guid.Empty
                    && x.VenueId != Guid.Empty)
                .Select(x => (x.MatchId, x.SportId, x.CourtId, x.VenueId))
                .ToHashSet();

            var usedVenueIds = existingRows
                .Select(x => x.VenueId)
                .Where(id => id != Guid.Empty)
                .ToHashSet();

            var toAdd = new List<Event>();
            foreach (var item in seedEvents)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!matchesByTitle.TryGetValue(item.MatchTitle, out var match) ||
                    !sportsByName.TryGetValue(item.SportName, out var sport) ||
                    !courtsByName.TryGetValue(item.CourtName, out var court) ||
                    item.VenueIndex < 0 || item.VenueIndex >= venues.Count)
                {
                    continue;
                }

                var venue = venues[item.VenueIndex];

                if (match.Id == Guid.Empty
                    || sport.Id == Guid.Empty
                    || court.Id == Guid.Empty
                    || venue.Id == Guid.Empty)
                {
                    continue;
                }

                var key = (match.Id, sport.Id, court.Id, venue.Id);
                if (!existingKeys.Add(key))
                {
                    continue;
                }

                if (!usedVenueIds.Add(venue.Id))
                {
                    existingKeys.Remove(key);
                    continue;
                }

                toAdd.Add(new Event
                {
                    MatchId = match.Id,
                    SportId = sport.Id,
                    CourtId = court.Id,
                    VenueId = venue.Id
                });
            }

            if (toAdd.Count > 0)
            {
                await eventRepository.AddRangeAsync(toAdd, cancellationToken);
                await eventRepository.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Adds the match participants that do not exist yet. Entries whose match or user cannot be resolved are skipped.
        /// </summary>
        /// <param name="matchParticipantRepository">Repository used to access match participants.</param>
        /// <param name="matchesByTitle">Matches keyed by title.</param>
        /// <param name="usersByEmail">Users keyed by email address.</param>
        /// <param name="seedParticipants">The participants to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        private static async Task SeedMatchParticipantsAsync(
            IRepository<MatchParticipant> matchParticipantRepository,
            Dictionary<string, Match> matchesByTitle,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedMatchParticipantDto> seedParticipants,
            CancellationToken cancellationToken)
        {
            var existingRows = await matchParticipantRepository.Query()
                .Select(x => new { x.MatchId, x.UserId })
                .ToListAsync(cancellationToken);

            var knownIds = existingRows
                .Where(x => x.MatchId != Guid.Empty && x.UserId != Guid.Empty)
                .Select(x => (x.MatchId, x.UserId))
                .ToHashSet();
            var existingPairs = existingRows.Select(x => (x.MatchId, x.UserId)).ToHashSet();

            var toAdd = new List<MatchParticipant>();
            foreach (var item in seedParticipants)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!matchesByTitle.TryGetValue(item.MatchTitle, out var match) ||
                    !usersByEmail.TryGetValue(item.UserEmail, out var user))
                {
                    continue;
                }

                var pair = (match.Id, user.Id);
                if (!existingPairs.Add(pair))
                {
                    continue;
                }

                var entity = new MatchParticipant
                {
                    MatchId = match.Id,
                    UserId = user.Id,
                    Status = item.Status,
                    JoinedOn = item.JoinedOn,
                    StatusChangedOn = item.StatusChangedOn,
                    IsLateCancel = item.IsLateCancel
                };

                if (!TryRegisterGuid(matchId: entity.MatchId, userId: entity.UserId, knownIds))
                {
                    existingPairs.Remove(pair);
                    continue;
                }

                toAdd.Add(entity);
            }

            if (toAdd.Count > 0)
            {
                await matchParticipantRepository.AddRangeAsync(toAdd, cancellationToken);
                await matchParticipantRepository.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Adds the notifications that do not exist yet. Entries whose user or match cannot be resolved are skipped.
        /// </summary>
        /// <param name="notificationRepository">Repository used to access notifications.</param>
        /// <param name="matchesByTitle">Matches keyed by title.</param>
        /// <param name="usersByEmail">Users keyed by email address.</param>
        /// <param name="seedNotifications">The notifications to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        private static async Task SeedNotificationsAsync(
            IRepository<Notification> notificationRepository,
            Dictionary<string, Match> matchesByTitle,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedNotificationDto> seedNotifications,
            CancellationToken cancellationToken)
        {
            var existingRows = await notificationRepository.Query()
                .Select(x => new { x.Id, x.UserId, x.MatchId, x.Message, x.CreatedOn })
                .ToListAsync(cancellationToken);

            var knownIds = existingRows.Select(x => x.Id).Where(id => id != Guid.Empty).ToHashSet();
            var existingKeys = existingRows
                .Select(x => (x.UserId, x.MatchId, x.Message, x.CreatedOn))
                .ToHashSet();

            var toAdd = new List<Notification>();
            foreach (var item in seedNotifications)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!usersByEmail.TryGetValue(item.UserEmail, out var user) ||
                    !matchesByTitle.TryGetValue(item.MatchTitle, out var match))
                {
                    continue;
                }

                var key = (user.Id, (Guid?)match.Id, item.Message, item.CreatedOn);
                if (!existingKeys.Add(key))
                {
                    continue;
                }

                var entity = new Notification
                {
                    UserId = user.Id,
                    Type = item.Type,
                    Message = item.Message,
                    MatchId = match.Id,
                    IsRead = item.IsRead,
                    CreatedOn = item.CreatedOn
                };

                if (!TryRegisterGuid(entity.Id, knownIds))
                {
                    existingKeys.Remove(key);
                    continue;
                }

                toAdd.Add(entity);
            }

            if (toAdd.Count > 0)
            {
                await notificationRepository.AddRangeAsync(toAdd, cancellationToken);
                await notificationRepository.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Adds the player ratings that do not exist yet. Entries whose match, rater or rated user cannot be resolved are skipped.
        /// </summary>
        /// <param name="playerRatingRepository">Repository used to access player ratings.</param>
        /// <param name="matchesByTitle">Matches keyed by title.</param>
        /// <param name="usersByEmail">Users keyed by email address.</param>
        /// <param name="seedRatings">The ratings to seed.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        private static async Task SeedPlayerRatingsAsync(
            IRepository<PlayerRating> playerRatingRepository,
            Dictionary<string, Match> matchesByTitle,
            Dictionary<string, ApplicationUser> usersByEmail,
            IReadOnlyList<SeedPlayerRatingDto> seedRatings,
            CancellationToken cancellationToken)
        {
            var existingRows = await playerRatingRepository.Query()
                .Select(x => new { x.MatchId, x.RaterId, x.RatedUserId })
                .ToListAsync(cancellationToken);

            var knownIds = existingRows
                .Where(x => x.MatchId != Guid.Empty && x.RaterId != Guid.Empty && x.RatedUserId != Guid.Empty)
                .Select(x => (x.MatchId, x.RaterId, x.RatedUserId))
                .ToHashSet();
            var existingKeys = existingRows
                .Select(x => (x.MatchId, x.RaterId, x.RatedUserId))
                .ToHashSet();

            var toAdd = new List<PlayerRating>();
            foreach (var item in seedRatings)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!matchesByTitle.TryGetValue(item.MatchTitle, out var match) ||
                    !usersByEmail.TryGetValue(item.RaterEmail, out var rater) ||
                    !usersByEmail.TryGetValue(item.RatedEmail, out var ratedUser))
                {
                    continue;
                }

                var key = (match.Id, rater.Id, ratedUser.Id);
                if (!existingKeys.Add(key))
                {
                    continue;
                }

                var entity = new PlayerRating
                {
                    MatchId = match.Id,
                    RaterId = rater.Id,
                    RatedUserId = ratedUser.Id,
                    Score = item.Score,
                    Comment = item.Comment,
                    IsHidden = false
                };

                if (!TryRegisterGuid(
                    matchId: entity.MatchId,
                    raterId: entity.RaterId,
                    ratedUserId: entity.RatedUserId,
                    knownIds))
                {
                    existingKeys.Remove(key);
                    continue;
                }

                toAdd.Add(entity);
            }

            if (toAdd.Count > 0)
            {
                await playerRatingRepository.AddRangeAsync(toAdd, cancellationToken);
                await playerRatingRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
