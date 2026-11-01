using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class Reinitialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    City = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ReliabilityScore = table.Column<int>(type: "int", nullable: true),
                    MatchesPlayed = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DefaultMinPlayers = table.Column<int>(type: "int", nullable: false),
                    DefaultMaxPlayers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    MatchStartsAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    OpensAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosesAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganiserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    MinPlayers = table.Column<int>(type: "int", nullable: false),
                    MaxPlayers = table.Column<int>(type: "int", nullable: false),
                    MinSkill = table.Column<int>(type: "int", nullable: false),
                    MaxSkill = table.Column<int>(type: "int", nullable: false),
                    PricePerPlayer = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CancelReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_AspNetUsers_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SportPreferences",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SportId = table.Column<int>(type: "int", nullable: false),
                    SkillLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportPreferences", x => new { x.UserId, x.SportId });
                    table.ForeignKey(
                        name: "FK_SportPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SportPreferences_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Surface = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    HourlyPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courts_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MatchParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    JoinedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLateCancel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchParticipants_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    RaterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RatedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerRatings_AspNetUsers_RatedUserId",
                        column: x => x.RatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerRatings_AspNetUsers_RaterId",
                        column: x => x.RaterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerRatings_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourtSports",
                columns: table => new
                {
                    CourtId = table.Column<int>(type: "int", nullable: false),
                    SportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtSports", x => new { x.CourtId, x.SportId });
                    table.ForeignKey(
                        name: "FK_CourtSports_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourtSports_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    SportId = table.Column<int>(type: "int", nullable: false),
                    CourtId = table.Column<int>(type: "int", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => new { x.MatchId, x.SportId, x.CourtId, x.VenueId });
                    table.ForeignKey(
                        name: "FK_Events_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "MatchesPlayed", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ReliabilityScore", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", 0, "Sofia", "0b8d5f31-1f24-4b9b-8c4a-0c1a8c21f001", "Alex Morgan", "alex.morgan@example.com", true, true, false, null, 4, "ALEX.MORGAN@EXAMPLE.COM", "ALEX.MORGAN", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "alex.morgan" },
                    { "22222222-2222-2222-2222-222222222222", 0, "Plovdiv", "3e9a1d72-5c66-4a80-9f31-52e2d4b8f002", "Jordan Lee", "jordan.lee@example.com", true, true, false, null, 3, "JORDAN.LEE@EXAMPLE.COM", "JORDAN.LEE", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "jordan.lee" },
                    { "33333333-3333-3333-3333-333333333333", 0, "Varna", "7a4c2e90-8b13-46fd-a275-91c3e6f4a003", "Sam Taylor", "sam.taylor@example.com", true, true, false, null, 4, "SAM.TAYLOR@EXAMPLE.COM", "SAM.TAYLOR", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "sam.taylor" },
                    { "44444444-4444-4444-4444-444444444444", 0, "Burgas", "d18f6b42-2a97-4e51-b803-6c7d9a5f4004", "Casey Wilson", "casey.wilson@example.com", true, true, false, null, 3, "CASEY.WILSON@EXAMPLE.COM", "CASEY.WILSON", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "casey.wilson" },
                    { "55555555-5555-5555-5555-555555555555", 0, "Sofia", "f5b27c68-9d40-43ae-8b16-2e7a4c9d5005", "Morgan Kim", "morgan.kim@example.com", true, true, false, null, 2, "MORGAN.KIM@EXAMPLE.COM", "MORGAN.KIM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 67, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "morgan.kim" },
                    { "66666666-6666-6666-6666-666666666666", 0, "Ruse", "6c1e8a53-74b9-4fd2-a605-3b9c7e2f6006", "Riley Adams", "riley.adams@example.com", true, true, false, null, 2, "RILEY.ADAMS@EXAMPLE.COM", "RILEY.ADAMS", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, null, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "riley.adams" },
                    { "77777777-7777-7777-7777-777777777777", 0, "Sofia", "a92d4f17-6e80-4b35-9c21-7d5a8e3f7007", "Taylor Brown", "taylor.brown@example.com", true, true, false, null, 3, "TAYLOR.BROWN@EXAMPLE.COM", "TAYLOR.BROWN", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "taylor.brown" },
                    { "88888888-8888-8888-8888-888888888888", 0, "Plovdiv", "4f7b2c19-a638-45de-8b04-6e9d1a5c8008", "Jamie Davis", "jamie.davis@example.com", true, true, false, null, 0, "JAMIE.DAVIS@EXAMPLE.COM", "JAMIE.DAVIS", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 0, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "jamie.davis" },
                    { "99999999-9999-9999-9999-999999999999", 0, "Varna", "8e3a6d25-1b74-4c90-a582-9f6b2d7e9009", "Quinn Clark", "quinn.clark@example.com", true, true, false, null, 4, "QUINN.CLARK@EXAMPLE.COM", "QUINN.CLARK", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "quinn.clark" },
                    { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 0, "Ruse", "b6c1f843-5a29-47de-9082-4e7d3a1f0010", "Drew Hall", "drew.hall@example.com", true, true, false, null, 4, "DREW.HALL@EXAMPLE.COM", "DREW.HALL", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "drew.hall" }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "Id", "Capacity", "HourlyPrice", "Name", "Surface", "VenueId" },
                values: new object[,]
                {
                    { 1, 22, 80m, "Main Football Pitch", 1, null },
                    { 2, 10, 45m, "Arena Basketball 1", 2, null },
                    { 3, 4, 25m, "Riverside Tennis 1", 3, null },
                    { 4, 12, 35m, "South Volleyball Court", 4, null },
                    { 5, 4, 30m, "Mountain Padel 1", 1, null },
                    { 6, 4, 20m, "West Badminton 1", 2, null },
                    { 7, 14, 50m, "Sea Handball Court", 5, null },
                    { 8, 30, 90m, "Maritsa Rugby Field", 0, null },
                    { 9, 22, 75m, "Green Hockey Field", 1, null },
                    { 10, 18, 65m, "Danube Baseball Field", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Sports",
                columns: new[] { "Id", "DefaultMaxPlayers", "DefaultMinPlayers", "IconClass", "Name" },
                values: new object[,]
                {
                    { 1, 22, 6, "fa-solid fa-futbol", "Football" },
                    { 2, 10, 4, "fa-solid fa-basketball", "Basketball" },
                    { 3, 4, 2, "fa-solid fa-table-tennis-paddle-ball", "Tennis" },
                    { 4, 12, 4, "fa-solid fa-volleyball", "Volleyball" },
                    { 5, 4, 2, "fa-solid fa-table-tennis-paddle-ball", "Padel" },
                    { 6, 4, 2, "fa-solid fa-feather", "Badminton" },
                    { 7, 14, 6, "fa-solid fa-hand", "Handball" },
                    { 8, 30, 10, "fa-solid fa-football", "Rugby" },
                    { 9, 22, 6, "fa-solid fa-hockey-puck", "Hockey" },
                    { 10, 18, 6, "fa-solid fa-baseball", "Baseball" }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "Id", "Address", "City", "ClosesAt", "Date", "IsDeleted", "Latitude", "Longitude", "MatchStartsAt", "Name", "OpensAt", "PhotoUrl" },
                values: new object[,]
                {
                    { 1, "1 Main Street", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(1, 1, 1), false, 42.697700m, 23.321900m, new TimeOnly(0, 0, 0), "Central Sports Park", new TimeOnly(7, 0, 0), null },
                    { 2, "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(1, 1, 1), false, 42.135400m, 24.745300m, new TimeOnly(0, 0, 0), "Eastside Arena", new TimeOnly(8, 0, 0), null },
                    { 3, "8 River Road", "Varna", new TimeOnly(22, 30, 0), new DateOnly(1, 1, 1), false, 43.214100m, 27.914700m, new TimeOnly(0, 0, 0), "Riverside Courts", new TimeOnly(6, 30, 0), null },
                    { 4, "24 South Park", "Burgas", new TimeOnly(21, 30, 0), new DateOnly(1, 1, 1), false, 42.504800m, 27.462600m, new TimeOnly(0, 0, 0), "South Field Complex", new TimeOnly(7, 30, 0), null },
                    { 5, "5 Vitosha Avenue", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(1, 1, 1), false, 42.650700m, 23.347000m, new TimeOnly(0, 0, 0), "Mountain Sports Hall", new TimeOnly(8, 0, 0), null },
                    { 6, "44 West Street", "Sofia", new TimeOnly(22, 0, 0), new DateOnly(1, 1, 1), false, 42.705000m, 23.270000m, new TimeOnly(0, 0, 0), "West End Club", new TimeOnly(9, 0, 0), null },
                    { 7, "19 Coast Lane", "Varna", new TimeOnly(21, 0, 0), new DateOnly(1, 1, 1), false, 43.207000m, 27.922000m, new TimeOnly(0, 0, 0), "Black Sea Sports", new TimeOnly(7, 0, 0), null },
                    { 8, "31 Maritsa Road", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(1, 1, 1), false, 42.145000m, 24.760000m, new TimeOnly(0, 0, 0), "Maritsa Activity Center", new TimeOnly(8, 0, 0), null },
                    { 9, "7 Garden Street", "Ruse", new TimeOnly(21, 30, 0), new DateOnly(1, 1, 1), false, 43.835000m, 25.965000m, new TimeOnly(0, 0, 0), "Green Court Club", new TimeOnly(7, 30, 0), null },
                    { 10, "16 Harbor Avenue", "Ruse", new TimeOnly(22, 30, 0), new DateOnly(1, 1, 1), false, 43.848000m, 25.960000m, new TimeOnly(0, 0, 0), "Danube Sports Hub", new TimeOnly(8, 30, 0), null }
                });

            migrationBuilder.InsertData(
                table: "CourtSports",
                columns: new[] { "CourtId", "SportId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 9 },
                    { 2, 2 },
                    { 2, 4 },
                    { 2, 7 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 2 },
                    { 7, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 10, 10 }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "CancelReason", "Description", "DurationMinutes", "IsDeleted", "IsHidden", "MaxPlayers", "MaxSkill", "MinPlayers", "MinSkill", "OrganiserId", "PricePerPlayer", "Status", "Title" },
                values: new object[,]
                {
                    { 1, null, "Casual five-a-side football.", 90, false, false, 22, 2, 6, 0, "11111111-1111-1111-1111-111111111111", 8m, 3, "Tuesday Evening Football" },
                    { 2, null, "Competitive five-on-five game.", 90, false, false, 10, 2, 4, 1, "22222222-2222-2222-2222-222222222222", 5m, 3, "Downtown Basketball" },
                    { 3, null, "Friendly doubles session.", 120, false, false, 4, 1, 2, 0, "33333333-3333-3333-3333-333333333333", 6m, 3, "Saturday Tennis Doubles" },
                    { 4, null, "Three-on-three beach volleyball.", 90, false, false, 12, 2, 6, 0, "44444444-4444-4444-4444-444444444444", 7m, 3, "Beach Volleyball Morning" },
                    { 5, null, "Fast-paced padel games.", 90, false, false, 4, 2, 2, 1, "55555555-5555-5555-5555-555555555555", 9m, 1, "Padel After Work" },
                    { 6, null, "Singles and doubles rotation.", 120, false, false, 4, 1, 2, 0, "66666666-6666-6666-6666-666666666666", 4m, 1, "Badminton Open Play" },
                    { 7, null, "A structured handball session.", 90, false, false, 14, 2, 8, 1, "77777777-7777-7777-7777-777777777777", 6m, 0, "Handball Skills Match" },
                    { 8, null, "Non-contact touch rugby.", 90, false, false, 30, 1, 10, 0, "88888888-8888-8888-8888-888888888888", 8m, 0, "Weekend Rugby Touch" },
                    { 9, null, "Small-sided field hockey.", 90, false, false, 22, 2, 6, 0, "99999999-9999-9999-9999-999999999999", 7m, 3, "Hockey Training Game" },
                    { 10, "Venue maintenance", "Relaxed practice game for all levels.", 120, false, false, 18, 1, 6, 0, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 5m, 2, "Baseball Practice Game" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedOn", "IsRead", "Link", "Message", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/10", "Baseball Practice Game was cancelled: Venue maintenance.", 2, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 2, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/10", "Baseball Practice Game was cancelled: Venue maintenance.", 2, "77777777-7777-7777-7777-777777777777" },
                    { 3, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/10", "Baseball Practice Game was cancelled: Venue maintenance.", 2, "99999999-9999-9999-9999-999999999999" },
                    { 4, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), true, "/Matches/Details/5", "Padel After Work has enough players and will go ahead.", 1, "55555555-5555-5555-5555-555555555555" },
                    { 5, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/5", "Padel After Work has enough players and will go ahead.", 1, "11111111-1111-1111-1111-111111111111" },
                    { 6, new DateTime(2026, 10, 30, 18, 30, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/5", "A spot opened in Padel After Work. You moved off the waitlist.", 0, "44444444-4444-4444-4444-444444444444" },
                    { 7, new DateTime(2026, 10, 25, 12, 50, 0, 0, DateTimeKind.Unspecified), true, "/Matches/Details/9", "You left Hockey Training Game less than 2 hours before the start. A strike was added.", 4, "88888888-8888-8888-8888-888888888888" },
                    { 8, new DateTime(2026, 10, 14, 20, 40, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/2", "You were marked as a no-show for Downtown Basketball. A strike was added.", 4, "55555555-5555-5555-5555-555555555555" },
                    { 9, new DateTime(2026, 10, 14, 1, 30, 0, 0, DateTimeKind.Unspecified), true, "/Matches/Details/1", "Alex Morgan rated you 5/5 after Tuesday Evening Football.", 3, "22222222-2222-2222-2222-222222222222" },
                    { 10, new DateTime(2026, 10, 15, 2, 30, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/2", "Jordan Lee rated you 3/5 after Downtown Basketball.", 3, "11111111-1111-1111-1111-111111111111" }
                });

            migrationBuilder.InsertData(
                table: "SportPreferences",
                columns: new[] { "SportId", "UserId", "SkillLevel" },
                values: new object[,]
                {
                    { 1, "11111111-1111-1111-1111-111111111111", 1 },
                    { 2, "11111111-1111-1111-1111-111111111111", 1 },
                    { 5, "11111111-1111-1111-1111-111111111111", 1 },
                    { 2, "22222222-2222-2222-2222-222222222222", 2 },
                    { 5, "22222222-2222-2222-2222-222222222222", 1 },
                    { 3, "33333333-3333-3333-3333-333333333333", 0 },
                    { 2, "44444444-4444-4444-4444-444444444444", 1 },
                    { 4, "44444444-4444-4444-4444-444444444444", 1 },
                    { 5, "44444444-4444-4444-4444-444444444444", 1 },
                    { 2, "55555555-5555-5555-5555-555555555555", 1 },
                    { 5, "55555555-5555-5555-5555-555555555555", 2 },
                    { 6, "66666666-6666-6666-6666-666666666666", 0 },
                    { 7, "77777777-7777-7777-7777-777777777777", 1 },
                    { 8, "88888888-8888-8888-8888-888888888888", 0 },
                    { 5, "99999999-9999-9999-9999-999999999999", 2 },
                    { 7, "99999999-9999-9999-9999-999999999999", 1 },
                    { 9, "99999999-9999-9999-9999-999999999999", 2 },
                    { 7, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 1 },
                    { 10, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 1 }
                });

            migrationBuilder.InsertData(
                table: "MatchParticipants",
                columns: new[] { "Id", "IsLateCancel", "JoinedOn", "MatchId", "Status", "StatusChangedOn", "UserId" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2026, 9, 29, 9, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 2, false, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "22222222-2222-2222-2222-222222222222" },
                    { 3, false, new DateTime(2026, 10, 4, 6, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 4, false, new DateTime(2026, 10, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 5, false, new DateTime(2026, 10, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 6, false, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "66666666-6666-6666-6666-666666666666" },
                    { 7, false, new DateTime(2026, 10, 5, 6, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" },
                    { 8, false, new DateTime(2026, 10, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 9, false, new DateTime(2026, 10, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 10, true, new DateTime(2026, 10, 3, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, new DateTime(2026, 10, 13, 16, 50, 0, 0, DateTimeKind.Unspecified), "88888888-8888-8888-8888-888888888888" },
                    { 11, false, new DateTime(2026, 9, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), "22222222-2222-2222-2222-222222222222" },
                    { 12, false, new DateTime(2026, 10, 5, 1, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 13, false, new DateTime(2026, 10, 5, 7, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 14, false, new DateTime(2026, 10, 5, 13, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 15, false, new DateTime(2026, 10, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 16, false, new DateTime(2026, 10, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 17, false, new DateTime(2026, 10, 7, 22, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 18, false, new DateTime(2026, 10, 8, 4, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 19, false, new DateTime(2026, 10, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 20, false, new DateTime(2026, 10, 8, 15, 0, 0, 0, DateTimeKind.Unspecified), 4, 4, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "88888888-8888-8888-8888-888888888888" },
                    { 21, false, new DateTime(2026, 10, 8, 21, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 22, false, new DateTime(2026, 10, 9, 3, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 23, false, new DateTime(2026, 10, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "66666666-6666-6666-6666-666666666666" },
                    { 24, false, new DateTime(2026, 10, 9, 15, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" },
                    { 25, false, new DateTime(2026, 10, 9, 21, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 26, false, new DateTime(2026, 10, 10, 3, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 27, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 5, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 28, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 5, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 29, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 5, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "22222222-2222-2222-2222-222222222222" },
                    { 30, false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), 5, 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 31, false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 32, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 6, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "66666666-6666-6666-6666-666666666666" },
                    { 33, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 6, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 34, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 6, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" },
                    { 35, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 7, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" },
                    { 36, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 7, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 37, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 7, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 38, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 8, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "88888888-8888-8888-8888-888888888888" },
                    { 39, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 8, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 40, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 8, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 41, false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), 8, 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 42, false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 8, 0, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "66666666-6666-6666-6666-666666666666" },
                    { 43, false, new DateTime(2026, 10, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), 9, 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 44, false, new DateTime(2026, 10, 15, 20, 0, 0, 0, DateTimeKind.Unspecified), 9, 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" },
                    { 45, false, new DateTime(2026, 10, 16, 2, 0, 0, 0, DateTimeKind.Unspecified), 9, 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 46, false, new DateTime(2026, 10, 16, 8, 0, 0, 0, DateTimeKind.Unspecified), 9, 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 47, false, new DateTime(2026, 10, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), 9, 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "22222222-2222-2222-2222-222222222222" },
                    { 48, false, new DateTime(2026, 10, 16, 20, 0, 0, 0, DateTimeKind.Unspecified), 9, 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 49, true, new DateTime(2026, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), 9, 2, new DateTime(2026, 10, 25, 12, 50, 0, 0, DateTimeKind.Unspecified), "88888888-8888-8888-8888-888888888888" },
                    { 50, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 10, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 51, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 10, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" },
                    { 52, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 10, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" }
                });

            migrationBuilder.InsertData(
                table: "PlayerRatings",
                columns: new[] { "Id", "Comment", "IsHidden", "MatchId", "RatedUserId", "RaterId", "Score" },
                values: new object[,]
                {
                    { 1, "Superb teammate, great energy.", false, 1, "22222222-2222-2222-2222-222222222222", "11111111-1111-1111-1111-111111111111", 5 },
                    { 2, "Decent game, nothing special.", false, 1, "33333333-3333-3333-3333-333333333333", "22222222-2222-2222-2222-222222222222", 3 },
                    { 3, "Decent game, nothing special.", false, 1, "44444444-4444-4444-4444-444444444444", "33333333-3333-3333-3333-333333333333", 3 },
                    { 4, "Decent game, nothing special.", false, 2, "11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222", 3 },
                    { 5, "Decent game, nothing special.", false, 2, "44444444-4444-4444-4444-444444444444", "11111111-1111-1111-1111-111111111111", 3 },
                    { 6, "Superb teammate, great energy.", false, 3, "99999999-9999-9999-9999-999999999999", "33333333-3333-3333-3333-333333333333", 5 },
                    { 7, null, false, 3, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "99999999-9999-9999-9999-999999999999", 4 },
                    { 8, null, false, 4, "33333333-3333-3333-3333-333333333333", "44444444-4444-4444-4444-444444444444", 4 },
                    { 9, "Superb teammate, great energy.", false, 4, "55555555-5555-5555-5555-555555555555", "33333333-3333-3333-3333-333333333333", 5 },
                    { 10, "Superb teammate, great energy.", false, 9, "77777777-7777-7777-7777-777777777777", "99999999-9999-9999-9999-999999999999", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Courts_VenueId",
                table: "Courts",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtSports_SportId",
                table: "CourtSports",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CourtId",
                table: "Events",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_MatchId",
                table: "Events",
                column: "MatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_SportId",
                table: "Events",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueId",
                table: "Events",
                column: "VenueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_OrganiserId",
                table: "Matches",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipants_MatchId",
                table: "MatchParticipants",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipants_UserId",
                table: "MatchParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRatings_MatchId",
                table: "PlayerRatings",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRatings_RatedUserId",
                table: "PlayerRatings",
                column: "RatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRatings_RaterId",
                table: "PlayerRatings",
                column: "RaterId");

            migrationBuilder.CreateIndex(
                name: "IX_SportPreferences_SportId",
                table: "SportPreferences",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_Sports_Name",
                table: "Sports",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CourtSports");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "MatchParticipants");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PlayerRatings");

            migrationBuilder.DropTable(
                name: "SportPreferences");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Courts");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
