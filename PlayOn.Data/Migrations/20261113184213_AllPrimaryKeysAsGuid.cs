using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class AllPrimaryKeysAsGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "Courts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Surface = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    HourlyPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Icon = table.Column<int>(type: "int", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartsAt = table.Column<TimeOnly>(type: "time", nullable: false),
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
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganiserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "CourtSports",
                columns: table => new
                {
                    CourtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "SportPreferences",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "MatchParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "Events",
                columns: table => new
                {
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => new { x.MatchId, x.SportId, x.CourtId, x.VenueId });
                    table.ForeignKey(
                        name: "FK_Events_CourtSports_CourtId_SportId",
                        columns: x => new { x.CourtId, x.SportId },
                        principalTable: "CourtSports",
                        principalColumns: new[] { "CourtId", "SportId" },
                        onDelete: ReferentialAction.Restrict);
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
                    { new Guid("35954915-3dee-4abb-9284-4fea99eb740b"), 0, "Ruse", "b6c1f843-5a29-47de-9082-4e7d3a1f0010", "Drew Hall", "drew.hall@example.com", true, true, false, null, 4, "DREW.HALL@EXAMPLE.COM", "DREW.HALL@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "DREW.HALL@EXAMPLE.COM" },
                    { new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7"), 0, "Ruse", "6c1e8a53-74b9-4fd2-a605-3b9c7e2f6006", "Riley Adams", "riley.adams@example.com", true, true, false, null, 2, "RILEY.ADAMS@EXAMPLE.COM", "RILEY.ADAMS@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, null, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "RILEY.ADAMS@EXAMPLE.COM" },
                    { new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), 0, "Varna", "8e3a6d25-1b74-4c90-a582-9f6b2d7e9009", "Quinn Clark", "quinn.clark@example.com", true, true, false, null, 4, "QUINN.CLARK@EXAMPLE.COM", "QUINN.CLARK@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "QUINN.CLARK@EXAMPLE.COM" },
                    { new Guid("4d530b4b-71d2-4039-8383-85d3a954c699"), 0, "Sofia", "f5b27c68-9d40-43ae-8b16-2e7a4c9d5005", "Morgan Kim", "morgan.kim@example.com", true, true, false, null, 2, "MORGAN.KIM@EXAMPLE.COM", "MORGAN.KIM@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 67, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "MORGAN.KIM@EXAMPLE.COM" },
                    { new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d"), 0, "Sofia", "a92d4f17-6e80-4b35-9c21-7d5a8e3f7007", "Taylor Brown", "taylor.brown@example.com", true, true, false, null, 3, "TAYLOR.BROWN@EXAMPLE.COM", "TAYLOR.BROWN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "TAYLOR.BROWN@EXAMPLE.COM" },
                    { new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), 0, "Plovdiv", "3e9a1d72-5c66-4a80-9f31-52e2d4b8f002", "Jordan Lee", "jordan.lee@example.com", true, true, false, null, 3, "JORDAN.LEE@EXAMPLE.COM", "JORDAN.LEE@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "JORDAN.LEE@EXAMPLE.COM" },
                    { new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba"), 0, "Plovdiv", "4f7b2c19-a638-45de-8b04-6e9d1a5c8008", "Jamie Davis", "jamie.davis@example.com", true, true, false, null, 0, "JAMIE.DAVIS@EXAMPLE.COM", "JAMIE.DAVIS@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 0, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "JAMIE.DAVIS@EXAMPLE.COM" },
                    { new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), 0, "Varna", "7a4c2e90-8b13-46fd-a275-91c3e6f4a003", "Sam Taylor", "sam.taylor@example.com", true, true, false, null, 4, "SAM.TAYLOR@EXAMPLE.COM", "SAM.TAYLOR@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "SAM.TAYLOR@EXAMPLE.COM" },
                    { new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 0, "Sofia", "0b8d5f31-1f24-4b9b-8c4a-0c1a8c21f001", "Alex Morgan", "alex.morgan@example.com", true, true, false, null, 4, "ALEX.MORGAN@EXAMPLE.COM", "ALEX.MORGAN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "ALEX.MORGAN@EXAMPLE.COM" },
                    { new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), 0, "Burgas", "d18f6b42-2a97-4e51-b803-6c7d9a5f4004", "Casey Wilson", "casey.wilson@example.com", true, true, false, null, 3, "CASEY.WILSON@EXAMPLE.COM", "CASEY.WILSON@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "CASEY.WILSON@EXAMPLE.COM" }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "Id", "Capacity", "HourlyPrice", "Name", "Surface" },
                values: new object[,]
                {
                    { new Guid("20724adf-d7d2-49fd-a65d-9c26c7a8868a"), 18, 65m, "Danube Baseball Field", 0 },
                    { new Guid("548be3fe-4372-4b76-8ab1-2d8617cced5c"), 22, 75m, "Green Hockey Field", 1 },
                    { new Guid("55841a09-24aa-4435-83a9-a1bd4eb4c5d2"), 30, 90m, "Maritsa Rugby Field", 0 },
                    { new Guid("9babf7c4-3162-4f42-bcc1-96c011545ce7"), 12, 35m, "South Volleyball Court", 4 },
                    { new Guid("b0c59eb2-a0f4-44f0-9fd0-5a8f4a61c503"), 14, 50m, "Sea Handball Court", 5 },
                    { new Guid("c0f2379b-7c7a-4e8b-ac8f-c35053a95f8b"), 4, 30m, "Mountain Padel 1", 1 },
                    { new Guid("e39325e8-9d1e-4e1f-873b-fe962d894458"), 4, 20m, "West Badminton 1", 2 },
                    { new Guid("f24d7b8e-4a96-44fc-bbb5-01f63af0edd3"), 22, 80m, "Main Football Pitch", 1 },
                    { new Guid("f77a1433-4b7f-4671-95b1-25ccecd025a1"), 4, 25m, "Riverside Tennis 1", 3 },
                    { new Guid("f9699c2a-c342-4138-b059-c8a938e9f403"), 10, 45m, "Arena Basketball 1", 2 }
                });

            migrationBuilder.InsertData(
                table: "Sports",
                columns: new[] { "Id", "DefaultMaxPlayers", "DefaultMinPlayers", "Icon", "Name" },
                values: new object[,]
                {
                    { new Guid("091e7187-ce1d-4601-a741-48293f2a76ee"), 22, 6, 1, "Football" },
                    { new Guid("412442c4-3295-4dda-9724-30000b9e6df7"), 4, 2, 5, "Badminton" },
                    { new Guid("484957b7-c520-4895-ae26-d4016c99ea25"), 22, 6, 8, "Hockey" },
                    { new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d"), 10, 4, 2, "Basketball" },
                    { new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), 4, 2, 3, "Padel" },
                    { new Guid("7696f11a-271f-47ce-aca4-9d79f43897cf"), 18, 6, 9, "Baseball" },
                    { new Guid("9684b9ad-6ffc-4d87-9060-7db5b8d54b1d"), 4, 2, 3, "Tennis" },
                    { new Guid("a82c7be9-f68d-43c2-8633-93f287cdfa3e"), 14, 6, 6, "Handball" },
                    { new Guid("b01275f2-2b7d-4044-8aa6-a79d8bdb4e73"), 12, 4, 4, "Volleyball" },
                    { new Guid("cbb963c5-6c74-4e5d-9cfe-6a0dd6087f9c"), 30, 10, 7, "Rugby" }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "Id", "Address", "City", "ClosesAt", "Date", "IsDeleted", "Latitude", "Longitude", "Name", "OpensAt", "PhotoUrl", "StartsAt" },
                values: new object[,]
                {
                    { new Guid("211201a6-3210-4571-ac5d-886b469a831c"), "5 Vitosha Avenue", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 9), false, 42.650700m, 23.347000m, "Mountain Sports Hall", new TimeOnly(8, 0, 0), null, new TimeOnly(18, 30, 0) },
                    { new Guid("26b106ce-c7f2-451f-99ba-801a8e94a5de"), "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 10, 14), false, 42.135400m, 24.745300m, "Eastside Arena", new TimeOnly(8, 0, 0), null, new TimeOnly(19, 0, 0) },
                    { new Guid("379bbff3-90ec-48e9-ae30-cb5e524d3400"), "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 13), false, 42.135400m, 24.745300m, "Eastside Arena", new TimeOnly(8, 0, 0), null, new TimeOnly(20, 0, 0) },
                    { new Guid("401ac881-bfa4-495e-be4a-882bcd37f54a"), "44 West Street", "Sofia", new TimeOnly(22, 0, 0), new DateOnly(2026, 12, 5), false, 42.705000m, 23.270000m, "West End Club", new TimeOnly(9, 0, 0), null, new TimeOnly(15, 0, 0) },
                    { new Guid("49ae060a-fe45-4a7a-be45-d2339a1afd26"), "1 Main Street", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 20), false, 42.697700m, 23.321900m, "Central Sports Park", new TimeOnly(7, 0, 0), null, new TimeOnly(18, 0, 0) },
                    { new Guid("531ad908-27da-499b-98b6-15ad6007411f"), "8 River Road", "Varna", new TimeOnly(22, 30, 0), new DateOnly(2026, 11, 22), false, 43.214100m, 27.914700m, "Riverside Courts", new TimeOnly(6, 30, 0), null, new TimeOnly(9, 30, 0) },
                    { new Guid("5b9f95eb-59ea-4655-ad1a-1a3bec7b8284"), "5 Vitosha Avenue", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 28), false, 42.650700m, 23.347000m, "Mountain Sports Hall", new TimeOnly(8, 0, 0), null, new TimeOnly(11, 0, 0) },
                    { new Guid("6c32c044-01a6-4ab7-ab20-6e0470698886"), "31 Maritsa Road", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 14), false, 42.145000m, 24.760000m, "Maritsa Activity Center", new TimeOnly(8, 0, 0), null, new TimeOnly(11, 0, 0) },
                    { new Guid("6fe7a206-f64f-4d54-99a4-c11a57355478"), "1 Main Street", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 10, 13), false, 42.697700m, 23.321900m, "Central Sports Park", new TimeOnly(7, 0, 0), null, new TimeOnly(18, 0, 0) },
                    { new Guid("707ad79f-2193-4af5-99ed-1aa5f5b51a8e"), "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 16), false, 42.135400m, 24.745300m, "Eastside Arena", new TimeOnly(8, 0, 0), null, new TimeOnly(18, 30, 0) },
                    { new Guid("72f87db4-8510-4240-a687-5ebace67a598"), "44 West Street", "Sofia", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 10), false, 42.705000m, 23.270000m, "West End Club", new TimeOnly(9, 0, 0), null, new TimeOnly(17, 0, 0) },
                    { new Guid("8151ed92-2588-47ec-b48a-3a8301c730cd"), "16 Harbor Avenue", "Ruse", new TimeOnly(22, 30, 0), new DateOnly(2026, 11, 15), false, 43.848000m, 25.960000m, "Danube Sports Hub", new TimeOnly(8, 30, 0), null, new TimeOnly(15, 0, 0) },
                    { new Guid("92177a2a-94d7-4bc3-b040-b130a877ebfa"), "8 River Road", "Varna", new TimeOnly(22, 30, 0), new DateOnly(2026, 11, 24), false, 43.214100m, 27.914700m, "Riverside Courts", new TimeOnly(6, 30, 0), null, new TimeOnly(19, 0, 0) },
                    { new Guid("9392c41a-981f-4124-b426-e14ed9462f52"), "1 Main Street", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 7), false, 42.697700m, 23.321900m, "Central Sports Park", new TimeOnly(7, 0, 0), null, new TimeOnly(10, 0, 0) },
                    { new Guid("9a851b96-b0f3-458d-95dc-3c9488d88cea"), "19 Coast Lane", "Varna", new TimeOnly(21, 0, 0), new DateOnly(2026, 11, 11), false, 43.207000m, 27.922000m, "Black Sea Sports", new TimeOnly(7, 0, 0), null, new TimeOnly(19, 30, 0) },
                    { new Guid("9e98c7a5-578c-4316-bb7d-ebc336c09209"), "8 River Road", "Varna", new TimeOnly(22, 30, 0), new DateOnly(2026, 10, 17), false, 43.214100m, 27.914700m, "Riverside Courts", new TimeOnly(6, 30, 0), null, new TimeOnly(10, 0, 0) },
                    { new Guid("a526c468-5091-41d8-815e-f0e06fd6b11e"), "5 Vitosha Avenue", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 18), false, 42.650700m, 23.347000m, "Mountain Sports Hall", new TimeOnly(8, 0, 0), null, new TimeOnly(19, 0, 0) },
                    { new Guid("aa96a310-ef4f-4fba-98ab-107ebe9ed12e"), "16 Harbor Avenue", "Ruse", new TimeOnly(22, 30, 0), new DateOnly(2026, 12, 3), false, 43.848000m, 25.960000m, "Danube Sports Hub", new TimeOnly(8, 30, 0), null, new TimeOnly(18, 0, 0) },
                    { new Guid("ad106f42-f3b4-4f29-aae7-9e0dc3297deb"), "24 South Park", "Burgas", new TimeOnly(21, 30, 0), new DateOnly(2026, 10, 18), false, 42.504800m, 27.462600m, "South Field Complex", new TimeOnly(7, 30, 0), null, new TimeOnly(9, 0, 0) },
                    { new Guid("becb3da1-7bf6-4509-a719-970dc38840e7"), "7 Garden Street", "Ruse", new TimeOnly(21, 30, 0), new DateOnly(2026, 10, 25), false, 43.835000m, 25.965000m, "Green Court Club", new TimeOnly(7, 30, 0), null, new TimeOnly(14, 0, 0) },
                    { new Guid("bf7d44ea-791b-454f-af1a-d22d72f5ff84"), "24 South Park", "Burgas", new TimeOnly(21, 30, 0), new DateOnly(2026, 11, 27), false, 42.504800m, 27.462600m, "South Field Complex", new TimeOnly(7, 30, 0), null, new TimeOnly(17, 30, 0) },
                    { new Guid("e62b242a-bd6c-491e-8be3-a6dc78c433d0"), "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 21), false, 42.135400m, 24.745300m, "Eastside Arena", new TimeOnly(8, 0, 0), null, new TimeOnly(10, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "CourtSports",
                columns: new[] { "CourtId", "SportId" },
                values: new object[,]
                {
                    { new Guid("20724adf-d7d2-49fd-a65d-9c26c7a8868a"), new Guid("7696f11a-271f-47ce-aca4-9d79f43897cf") },
                    { new Guid("548be3fe-4372-4b76-8ab1-2d8617cced5c"), new Guid("484957b7-c520-4895-ae26-d4016c99ea25") },
                    { new Guid("55841a09-24aa-4435-83a9-a1bd4eb4c5d2"), new Guid("cbb963c5-6c74-4e5d-9cfe-6a0dd6087f9c") },
                    { new Guid("9babf7c4-3162-4f42-bcc1-96c011545ce7"), new Guid("b01275f2-2b7d-4044-8aa6-a79d8bdb4e73") },
                    { new Guid("b0c59eb2-a0f4-44f0-9fd0-5a8f4a61c503"), new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d") },
                    { new Guid("b0c59eb2-a0f4-44f0-9fd0-5a8f4a61c503"), new Guid("a82c7be9-f68d-43c2-8633-93f287cdfa3e") },
                    { new Guid("c0f2379b-7c7a-4e8b-ac8f-c35053a95f8b"), new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8") },
                    { new Guid("e39325e8-9d1e-4e1f-873b-fe962d894458"), new Guid("412442c4-3295-4dda-9724-30000b9e6df7") },
                    { new Guid("f24d7b8e-4a96-44fc-bbb5-01f63af0edd3"), new Guid("091e7187-ce1d-4601-a741-48293f2a76ee") },
                    { new Guid("f24d7b8e-4a96-44fc-bbb5-01f63af0edd3"), new Guid("484957b7-c520-4895-ae26-d4016c99ea25") },
                    { new Guid("f77a1433-4b7f-4671-95b1-25ccecd025a1"), new Guid("9684b9ad-6ffc-4d87-9060-7db5b8d54b1d") },
                    { new Guid("f9699c2a-c342-4138-b059-c8a938e9f403"), new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d") },
                    { new Guid("f9699c2a-c342-4138-b059-c8a938e9f403"), new Guid("a82c7be9-f68d-43c2-8633-93f287cdfa3e") },
                    { new Guid("f9699c2a-c342-4138-b059-c8a938e9f403"), new Guid("b01275f2-2b7d-4044-8aa6-a79d8bdb4e73") }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "CancelReason", "Description", "DurationMinutes", "IsDeleted", "IsHidden", "MaxPlayers", "MaxSkill", "MinPlayers", "MinSkill", "OrganiserId", "PricePerPlayer", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), null, "Competitive five-on-five game.", 90, false, false, 10, 2, 4, 1, new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), 5m, 3, "Downtown Basketball" },
                    { new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"), "Venue maintenance", "Relaxed practice game for all levels.", 120, false, false, 18, 1, 6, 0, new Guid("35954915-3dee-4abb-9284-4fea99eb740b"), 5m, 2, "Baseball Practice Game" },
                    { new Guid("35e11244-2323-43ca-ab84-065d2382d1e9"), null, "A structured handball session.", 90, false, false, 14, 2, 8, 1, new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d"), 6m, 0, "Handball Skills Match" },
                    { new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), null, "Friendly doubles session.", 120, false, false, 4, 1, 2, 0, new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), 6m, 3, "Saturday Tennis Doubles" },
                    { new Guid("53f227cc-0884-4f7b-a429-ea359b2e38d1"), null, "Friendly kickabout, all levels welcome.", 90, false, false, 22, 2, 6, 0, new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 7m, 0, "Saturday Morning Football" },
                    { new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), null, "Casual five-a-side football.", 90, false, false, 22, 2, 6, 0, new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 8m, 3, "Tuesday Evening Football" },
                    { new Guid("5e5d66f4-810c-4bff-8929-be3afeebc0b1"), null, "Four players, rotating partners.", 90, false, false, 4, 2, 4, 0, new Guid("4d530b4b-71d2-4039-8383-85d3a954c699"), 9m, 0, "Padel Doubles Evening" },
                    { new Guid("5ec715e5-d2c2-484f-ae3c-8246979ab6be"), null, "Relaxed indoor volleyball with mixed teams.", 90, false, false, 10, 1, 6, 0, new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), 6m, 0, "Monday Volleyball Social" },
                    { new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), null, "Full-court games, winners stay on.", 90, false, false, 8, 2, 4, 0, new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), 5m, 1, "Friday Night Basketball" },
                    { new Guid("6a1a9532-0549-4866-9d9b-0cccc7095136"), null, "Singles and doubles rotation.", 120, false, false, 4, 1, 2, 0, new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7"), 4m, 1, "Badminton Open Play" },
                    { new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), null, "Three-on-three beach volleyball.", 90, false, false, 12, 2, 6, 0, new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), 7m, 3, "Beach Volleyball Morning" },
                    { new Guid("adcf6aa6-9d54-4db9-b13e-55014d5599df"), null, "Friendly singles on clay, bring your racket.", 60, false, false, 4, 1, 2, 0, new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), 6m, 1, "Sunday Tennis Singles" },
                    { new Guid("af0b763b-6efe-420a-be74-84426caed591"), null, "Small-sided field hockey.", 90, false, false, 22, 2, 6, 0, new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), 7m, 3, "Hockey Training Game" },
                    { new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"), null, "Fast-paced padel games.", 90, false, false, 4, 2, 2, 1, new Guid("4d530b4b-71d2-4039-8383-85d3a954c699"), 9m, 1, "Padel After Work" },
                    { new Guid("db55404d-e1e5-4891-bbb7-b36044df3615"), null, "Non-contact touch rugby.", 90, false, false, 30, 1, 10, 0, new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba"), 8m, 0, "Weekend Rugby Touch" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedOn", "IsRead", "Link", "Message", "Type", "UserId" },
                values: new object[,]
                {
                    { new Guid("04a104d8-7e99-4a6d-88b7-95a0b8de2139"), new DateTime(2026, 10, 14, 20, 40, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/04c0e2e2-35fc-4b8e-8706-6e133c403a8e", "You were marked as a no-show for Downtown Basketball. A strike was added.", 4, new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("2b525941-0519-49dd-b776-88a9310c0dc2"), new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/1cf5ea94-367c-4999-b001-a0fb68bc4d0e", "Baseball Practice Game was cancelled: Venue maintenance.", 2, new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("3fc5b7ee-04ac-4673-bdd9-c63ea5332aa9"), new DateTime(2026, 10, 14, 1, 30, 0, 0, DateTimeKind.Unspecified), true, "/Matches/Details/5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9", "Alex Morgan rated you 5/5 after Tuesday Evening Football.", 3, new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("406cddbb-56f7-4a87-a5ec-2bc1a6051df3"), new DateTime(2026, 10, 15, 2, 30, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/04c0e2e2-35fc-4b8e-8706-6e133c403a8e", "Jordan Lee rated you 3/5 after Downtown Basketball.", 3, new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("409f623e-0c1f-4119-a0c6-8c3dbd785cbb"), new DateTime(2026, 10, 30, 18, 30, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/cad33e31-98f1-4fd8-9503-2c7f873f0725", "A spot opened in Padel After Work. You moved off the waitlist.", 0, new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("5f244450-0dac-44a0-acd2-861088a9359f"), new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/1cf5ea94-367c-4999-b001-a0fb68bc4d0e", "Baseball Practice Game was cancelled: Venue maintenance.", 2, new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("61c7bc9d-299b-402c-be8f-f03af433092c"), new DateTime(2026, 10, 25, 12, 50, 0, 0, DateTimeKind.Unspecified), true, "/Matches/Details/af0b763b-6efe-420a-be74-84426caed591", "You left Hockey Training Game less than 2 hours before the start. A strike was added.", 4, new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba") },
                    { new Guid("8b4cb258-9e97-45eb-b568-af8d77f21d16"), new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/cad33e31-98f1-4fd8-9503-2c7f873f0725", "Padel After Work has enough players and will go ahead.", 1, new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("a87d8d14-f08a-4b5c-b27b-aa83863ef9d9"), new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "/Matches/Details/1cf5ea94-367c-4999-b001-a0fb68bc4d0e", "Baseball Practice Game was cancelled: Venue maintenance.", 2, new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("fa37ca9b-2bc8-43cb-a47d-60f3cf2386fd"), new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), true, "/Matches/Details/cad33e31-98f1-4fd8-9503-2c7f873f0725", "Padel After Work has enough players and will go ahead.", 1, new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") }
                });

            migrationBuilder.InsertData(
                table: "SportPreferences",
                columns: new[] { "SportId", "UserId", "SkillLevel" },
                values: new object[,]
                {
                    { new Guid("7696f11a-271f-47ce-aca4-9d79f43897cf"), new Guid("35954915-3dee-4abb-9284-4fea99eb740b"), 1 },
                    { new Guid("a82c7be9-f68d-43c2-8633-93f287cdfa3e"), new Guid("35954915-3dee-4abb-9284-4fea99eb740b"), 1 },
                    { new Guid("412442c4-3295-4dda-9724-30000b9e6df7"), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7"), 0 },
                    { new Guid("484957b7-c520-4895-ae26-d4016c99ea25"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), 2 },
                    { new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), 2 },
                    { new Guid("a82c7be9-f68d-43c2-8633-93f287cdfa3e"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), 1 },
                    { new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d"), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699"), 1 },
                    { new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699"), 2 },
                    { new Guid("a82c7be9-f68d-43c2-8633-93f287cdfa3e"), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d"), 1 },
                    { new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d"), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), 2 },
                    { new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), 1 },
                    { new Guid("cbb963c5-6c74-4e5d-9cfe-6a0dd6087f9c"), new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba"), 0 },
                    { new Guid("9684b9ad-6ffc-4d87-9060-7db5b8d54b1d"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), 0 },
                    { new Guid("091e7187-ce1d-4601-a741-48293f2a76ee"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 1 },
                    { new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 1 },
                    { new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 1 },
                    { new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), 1 },
                    { new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), 1 },
                    { new Guid("b01275f2-2b7d-4044-8aa6-a79d8bdb4e73"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), 1 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                values: new object[,]
                {
                    { new Guid("f9699c2a-c342-4138-b059-c8a938e9f403"), new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d"), new Guid("26b106ce-c7f2-451f-99ba-801a8e94a5de") },
                    { new Guid("20724adf-d7d2-49fd-a65d-9c26c7a8868a"), new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"), new Guid("7696f11a-271f-47ce-aca4-9d79f43897cf"), new Guid("8151ed92-2588-47ec-b48a-3a8301c730cd") },
                    { new Guid("b0c59eb2-a0f4-44f0-9fd0-5a8f4a61c503"), new Guid("35e11244-2323-43ca-ab84-065d2382d1e9"), new Guid("a82c7be9-f68d-43c2-8633-93f287cdfa3e"), new Guid("9a851b96-b0f3-458d-95dc-3c9488d88cea") },
                    { new Guid("f77a1433-4b7f-4671-95b1-25ccecd025a1"), new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), new Guid("9684b9ad-6ffc-4d87-9060-7db5b8d54b1d"), new Guid("9e98c7a5-578c-4316-bb7d-ebc336c09209") },
                    { new Guid("f24d7b8e-4a96-44fc-bbb5-01f63af0edd3"), new Guid("53f227cc-0884-4f7b-a429-ea359b2e38d1"), new Guid("091e7187-ce1d-4601-a741-48293f2a76ee"), new Guid("9392c41a-981f-4124-b426-e14ed9462f52") },
                    { new Guid("f24d7b8e-4a96-44fc-bbb5-01f63af0edd3"), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), new Guid("091e7187-ce1d-4601-a741-48293f2a76ee"), new Guid("6fe7a206-f64f-4d54-99a4-c11a57355478") },
                    { new Guid("c0f2379b-7c7a-4e8b-ac8f-c35053a95f8b"), new Guid("5e5d66f4-810c-4bff-8929-be3afeebc0b1"), new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), new Guid("a526c468-5091-41d8-815e-f0e06fd6b11e") },
                    { new Guid("f9699c2a-c342-4138-b059-c8a938e9f403"), new Guid("5ec715e5-d2c2-484f-ae3c-8246979ab6be"), new Guid("b01275f2-2b7d-4044-8aa6-a79d8bdb4e73"), new Guid("707ad79f-2193-4af5-99ed-1aa5f5b51a8e") },
                    { new Guid("f9699c2a-c342-4138-b059-c8a938e9f403"), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), new Guid("523cd7cb-46ed-404a-8c26-c9d464ae631d"), new Guid("379bbff3-90ec-48e9-ae30-cb5e524d3400") },
                    { new Guid("e39325e8-9d1e-4e1f-873b-fe962d894458"), new Guid("6a1a9532-0549-4866-9d9b-0cccc7095136"), new Guid("412442c4-3295-4dda-9724-30000b9e6df7"), new Guid("72f87db4-8510-4240-a687-5ebace67a598") },
                    { new Guid("9babf7c4-3162-4f42-bcc1-96c011545ce7"), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), new Guid("b01275f2-2b7d-4044-8aa6-a79d8bdb4e73"), new Guid("ad106f42-f3b4-4f29-aae7-9e0dc3297deb") },
                    { new Guid("f77a1433-4b7f-4671-95b1-25ccecd025a1"), new Guid("adcf6aa6-9d54-4db9-b13e-55014d5599df"), new Guid("9684b9ad-6ffc-4d87-9060-7db5b8d54b1d"), new Guid("531ad908-27da-499b-98b6-15ad6007411f") },
                    { new Guid("548be3fe-4372-4b76-8ab1-2d8617cced5c"), new Guid("af0b763b-6efe-420a-be74-84426caed591"), new Guid("484957b7-c520-4895-ae26-d4016c99ea25"), new Guid("becb3da1-7bf6-4509-a719-970dc38840e7") },
                    { new Guid("c0f2379b-7c7a-4e8b-ac8f-c35053a95f8b"), new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"), new Guid("6102816c-f9d8-448b-a876-4bdd73fe55e8"), new Guid("211201a6-3210-4571-ac5d-886b469a831c") },
                    { new Guid("55841a09-24aa-4435-83a9-a1bd4eb4c5d2"), new Guid("db55404d-e1e5-4891-bbb7-b36044df3615"), new Guid("cbb963c5-6c74-4e5d-9cfe-6a0dd6087f9c"), new Guid("6c32c044-01a6-4ab7-ab20-6e0470698886") }
                });

            migrationBuilder.InsertData(
                table: "MatchParticipants",
                columns: new[] { "Id", "IsLateCancel", "JoinedOn", "MatchId", "Status", "StatusChangedOn", "UserId" },
                values: new object[,]
                {
                    { new Guid("0a789038-d99e-4831-a4a0-580f909a96bc"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("0b1bf04d-4bc6-443d-a992-d96bdd603f79"), false, new DateTime(2026, 10, 30, 11, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 30, 11, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("0cb24542-307f-4c36-9137-fe2ebdb44c71"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5e5d66f4-810c-4bff-8929-be3afeebc0b1"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("14692a0f-5b51-46c6-b81a-1c825390cfce"), false, new DateTime(2026, 10, 4, 6, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("14f5a6bc-b79b-4027-9f7a-f4323ce21eba"), false, new DateTime(2026, 10, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("1549487c-7f97-440e-a664-9b4e8dcd1cf1"), false, new DateTime(2026, 10, 7, 22, 0, 0, 0, DateTimeKind.Unspecified), new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("221b18f3-a78d-49b3-9098-33142ac7e81d"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6a1a9532-0549-4866-9d9b-0cccc7095136"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("234ab00b-18e0-40be-8d21-c19422d04a7d"), false, new DateTime(2026, 9, 29, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("288c0a7f-1eac-404d-b63f-fefcd9c52a88"), false, new DateTime(2026, 10, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("2cc5a29c-dece-4a86-84c8-54aab908bccd"), false, new DateTime(2026, 10, 30, 14, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 1, new DateTime(2026, 10, 30, 14, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("2cfe2c94-dc37-499f-8011-9f93dcb0e430"), false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("db55404d-e1e5-4891-bbb7-b36044df3615"), 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("2d09687b-2161-4024-b363-ef84ec2c5bf7"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5e5d66f4-810c-4bff-8929-be3afeebc0b1"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("37f93b85-1420-4e28-a98d-fe9f2bc7e90c"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("3b33c26a-6e18-4254-aaba-48d4f1fbf0c0"), false, new DateTime(2026, 10, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("3df7e1c1-3e89-4f7a-8d0d-a31f711af853"), false, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7") },
                    { new Guid("43430db4-7fe8-40e1-854f-d039a533edde"), false, new DateTime(2026, 10, 8, 15, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 4, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba") },
                    { new Guid("466e8892-4735-4e87-a006-91a0afcf3c60"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("db55404d-e1e5-4891-bbb7-b36044df3615"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba") },
                    { new Guid("4dc1ebb5-8a42-45c3-b353-1d3fa93bf9ce"), false, new DateTime(2026, 10, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("53c77a94-94b7-4fd3-9148-b39f30315a71"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35e11244-2323-43ca-ab84-065d2382d1e9"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("5b5c0b31-9013-45b3-875d-a42ccc208911"), false, new DateTime(2026, 10, 5, 6, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("6192d8fb-d207-42f9-97fe-0b652971aab9"), false, new DateTime(2026, 10, 5, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), 3, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("647eeca2-9493-4e3a-98b6-9450d34b6a54"), false, new DateTime(2026, 10, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("64cdc438-c01b-457c-ad2b-e46084c13f30"), false, new DateTime(2026, 10, 30, 13, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 30, 13, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("6d9f4606-128c-4814-b530-6e35476361cc"), false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"), 1, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("6f1ad99d-87bf-4371-9652-cc2bcc7ec5cf"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5ec715e5-d2c2-484f-ae3c-8246979ab6be"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("6f1b8007-fd81-4f51-b9c2-43b80141edea"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("71840065-f854-4594-8394-d53b73933779"), false, new DateTime(2026, 9, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), 3, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("71ab904d-4d2c-430a-9c18-51ac93972a08"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5ec715e5-d2c2-484f-ae3c-8246979ab6be"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("7462ebba-fb9e-49a3-b386-f7429e0ea905"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("db55404d-e1e5-4891-bbb7-b36044df3615"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("77a8a212-ee26-4d90-a878-386aed8820e0"), false, new DateTime(2026, 10, 15, 20, 0, 0, 0, DateTimeKind.Unspecified), new Guid("af0b763b-6efe-420a-be74-84426caed591"), 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("78f3bfeb-7e5e-4797-a046-98b6b0d1c745"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("7d6fad4f-f72f-4f44-81cc-9e1b02e51353"), false, new DateTime(2026, 10, 9, 15, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("8038cd27-7a7a-4048-9593-c758c3e3fe21"), false, new DateTime(2026, 10, 9, 3, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("849ba91e-505d-4303-9b7b-394717472cc0"), false, new DateTime(2026, 10, 5, 1, 0, 0, 0, DateTimeKind.Unspecified), new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), 4, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("86aa8f37-74e6-476e-815e-8a570e45878f"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5e5d66f4-810c-4bff-8929-be3afeebc0b1"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("87b64897-73b1-420a-ae5a-3caf7441172a"), true, new DateTime(2026, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("af0b763b-6efe-420a-be74-84426caed591"), 2, new DateTime(2026, 10, 25, 12, 50, 0, 0, DateTimeKind.Unspecified), new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba") },
                    { new Guid("886a21e4-cf83-44c0-acc9-ecb6dafe249c"), false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("8b6ac961-7d26-4b67-b94e-b2c00e5d64ab"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("8e495406-1b6c-45bd-a141-5c29cd279275"), false, new DateTime(2026, 10, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("94564a88-b529-4508-bc1a-301aa0eba043"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35e11244-2323-43ca-ab84-065d2382d1e9"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("9b3ac065-4ab4-47b9-8bdb-e901ccd67344"), false, new DateTime(2026, 10, 8, 4, 0, 0, 0, DateTimeKind.Unspecified), new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("9bc8d3bc-edd3-4daf-9fc7-98099b30c568"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("9f30b96e-a59e-49d1-a254-e4fe3f4f1ba9"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6a1a9532-0549-4866-9d9b-0cccc7095136"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7") },
                    { new Guid("a3d1864a-2200-4128-a61e-48ac8d13c692"), false, new DateTime(2026, 10, 16, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("af0b763b-6efe-420a-be74-84426caed591"), 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("a6585a40-f9a7-4b4d-b70e-6e9c89c5f9a0"), false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"), 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("a7b13eda-e315-4820-b433-2c5b769f0016"), false, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 3, new DateTime(2026, 10, 13, 19, 30, 0, 0, DateTimeKind.Unspecified), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("acdcf9e4-3337-4bb2-96d3-660e23e010f4"), false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("af1e4899-8754-4e42-8230-a6e832b7cb2c"), false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5ec715e5-d2c2-484f-ae3c-8246979ab6be"), 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("b22529be-37d6-41a9-ba0a-ced81de3c3f0"), false, new DateTime(2026, 10, 29, 4, 0, 0, 0, DateTimeKind.Unspecified), new Guid("53f227cc-0884-4f7b-a429-ea359b2e38d1"), 0, new DateTime(2026, 10, 29, 4, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("b2e5f4bf-24b6-40f8-828d-97e1af2f3352"), false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("db55404d-e1e5-4891-bbb7-b36044df3615"), 0, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7") },
                    { new Guid("b9b41c8e-0040-4fb9-bf24-f80911ff1818"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("baec8f0f-c738-4d7b-a265-1dad36d260ba"), true, new DateTime(2026, 10, 3, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), 2, new DateTime(2026, 10, 13, 16, 50, 0, 0, DateTimeKind.Unspecified), new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba") },
                    { new Guid("bcc0e810-fbe8-4560-825b-3683b4df10d7"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35e11244-2323-43ca-ab84-065d2382d1e9"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("c47d2b3c-3336-4da4-aa42-7dac07fb7068"), false, new DateTime(2026, 10, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("c48a456e-3289-49e2-b409-40285bfb9a78"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("db55404d-e1e5-4891-bbb7-b36044df3615"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("c5de84b1-3790-4b0b-afcc-8a706ed399ab"), false, new DateTime(2026, 10, 10, 3, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("c61d0c4f-0ce2-4a14-8fe6-1a07192611e0"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("53f227cc-0884-4f7b-a429-ea359b2e38d1"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("c6b8a0cd-645c-4486-90a4-c82227bf797f"), false, new DateTime(2026, 10, 28, 16, 0, 0, 0, DateTimeKind.Unspecified), new Guid("53f227cc-0884-4f7b-a429-ea359b2e38d1"), 0, new DateTime(2026, 10, 28, 16, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("c7b304ba-c4ff-4349-beea-c4133eeee8a0"), false, new DateTime(2026, 10, 16, 20, 0, 0, 0, DateTimeKind.Unspecified), new Guid("af0b763b-6efe-420a-be74-84426caed591"), 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("cc5411a3-9fca-4b87-8a9b-7b2dc0bee4d5"), false, new DateTime(2026, 10, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), new Guid("af0b763b-6efe-420a-be74-84426caed591"), 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("cc6f4740-4012-49f9-8315-afcc0d5690c0"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("adcf6aa6-9d54-4db9-b13e-55014d5599df"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("cd266c52-eba5-48d9-8b7b-e0905cdf0b18"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("697d5462-e28f-4633-b6f8-e8ca5ec3c55a"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7") },
                    { new Guid("d0c96487-f401-4eb5-8f9b-fa51841cdfcc"), false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"), 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("d6e79808-61fa-4d5e-bf7d-134a92980494"), false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5ec715e5-d2c2-484f-ae3c-8246979ab6be"), 0, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("d7d1a57e-cff6-433a-8750-31539dea9b37"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5ec715e5-d2c2-484f-ae3c-8246979ab6be"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("d84da7ed-d75f-4120-932f-d65871b83c4a"), false, new DateTime(2026, 10, 16, 2, 0, 0, 0, DateTimeKind.Unspecified), new Guid("af0b763b-6efe-420a-be74-84426caed591"), 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("d9c36a61-6695-4cb2-a9ce-44c12479cdd6"), false, new DateTime(2026, 10, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7") },
                    { new Guid("dff93c78-7f1c-4f36-ad12-ec6e1bd82992"), false, new DateTime(2026, 10, 5, 13, 0, 0, 0, DateTimeKind.Unspecified), new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), 3, new DateTime(2026, 10, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("e03eb696-1807-441f-a95c-4d7c2f3599f4"), false, new DateTime(2026, 10, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("af0b763b-6efe-420a-be74-84426caed591"), 3, new DateTime(2026, 10, 25, 15, 30, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("e5c640b7-d946-48f8-bb3a-163f0e07618f"), false, new DateTime(2026, 10, 28, 22, 0, 0, 0, DateTimeKind.Unspecified), new Guid("53f227cc-0884-4f7b-a429-ea359b2e38d1"), 0, new DateTime(2026, 10, 28, 22, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("eecc764c-4304-4797-8826-957c91bb0bc9"), false, new DateTime(2026, 10, 9, 21, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("f6e5b2b9-8042-4cd5-a8cc-d14df5bdcbfa"), false, new DateTime(2026, 10, 8, 21, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), 3, new DateTime(2026, 10, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("fb7e3f75-3980-4185-8e79-c09673bda7aa"), false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("adcf6aa6-9d54-4db9-b13e-55014d5599df"), 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("fb88d41e-0b07-481a-aab7-dcb84b9425de"), false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6a1a9532-0549-4866-9d9b-0cccc7095136"), 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("fecefe9b-62e1-485d-ab29-4e2842e33705"), false, new DateTime(2026, 10, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), 3, new DateTime(2026, 10, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") }
                });

            migrationBuilder.InsertData(
                table: "PlayerRatings",
                columns: new[] { "Id", "Comment", "IsHidden", "MatchId", "RatedUserId", "RaterId", "Score" },
                values: new object[,]
                {
                    { new Guid("06a6de2e-e6f7-4113-b2d7-1161c51d7373"), null, false, new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), 4 },
                    { new Guid("674f2f9d-01d0-429d-82ef-421d66fee921"), "Decent game, nothing special.", false, new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 3 },
                    { new Guid("7b05d663-2c1d-4faa-b5f9-19c56b33e1af"), "Superb teammate, great energy.", false, new Guid("7da65cf5-3955-47a7-a484-7cb9448de52d"), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), 5 },
                    { new Guid("833f6abd-7c06-4108-92aa-1649784f951f"), null, false, new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), new Guid("35954915-3dee-4abb-9284-4fea99eb740b"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), 4 },
                    { new Guid("8378cb18-df92-47e5-b3ff-d60da9c59a40"), "Superb teammate, great energy.", false, new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), 5 },
                    { new Guid("b14f368e-19be-473c-b7e1-25d5fc7028de"), "Superb teammate, great energy.", false, new Guid("496125fb-69d6-4ae2-9068-095d059f98a2"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), 5 },
                    { new Guid("bc7930d2-4704-4e40-ba19-14e36545c53e"), "Decent game, nothing special.", false, new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), 3 },
                    { new Guid("c51986da-052f-4672-aab2-55f5a09ad3da"), "Decent game, nothing special.", false, new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"), 3 },
                    { new Guid("c59140ee-5ba7-44ad-a92e-b67b9872b1d5"), "Superb teammate, great energy.", false, new Guid("af0b763b-6efe-420a-be74-84426caed591"), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"), 5 },
                    { new Guid("ebc623fc-08a6-4976-aa75-9d9574392243"), "Decent game, nothing special.", false, new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"), 3 }
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
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CourtSports_SportId",
                table: "CourtSports",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CourtId_SportId",
                table: "Events",
                columns: new[] { "CourtId", "SportId" });

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
                name: "CourtSports");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Courts");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
