using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "CancelReason", "Description", "DurationMinutes", "IsDeleted", "IsHidden", "MaxPlayers", "MaxSkill", "MinPlayers", "MinSkill", "OrganiserId", "PricePerPlayer", "Status", "Title" },
                values: new object[,]
                {
                    { 11, null, "Friendly kickabout, all levels welcome.", 90, false, false, 22, 2, 6, 0, "11111111-1111-1111-1111-111111111111", 7m, 0, "Saturday Morning Football" },
                    { 12, null, "Full-court games, winners stay on.", 90, false, false, 8, 2, 4, 0, "22222222-2222-2222-2222-222222222222", 5m, 1, "Friday Night Basketball" },
                    { 13, null, "Relaxed indoor volleyball with mixed teams.", 90, false, false, 10, 1, 6, 0, "44444444-4444-4444-4444-444444444444", 6m, 0, "Monday Volleyball Social" },
                    { 14, null, "Four players, rotating partners.", 90, false, false, 4, 2, 4, 0, "55555555-5555-5555-5555-555555555555", 9m, 0, "Padel Doubles Evening" },
                    { 15, null, "Friendly singles on clay, bring your racket.", 60, false, false, 4, 1, 2, 0, "33333333-3333-3333-3333-333333333333", 6m, 1, "Sunday Tennis Singles" }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "Id", "Address", "City", "ClosesAt", "Date", "IsDeleted", "Latitude", "Longitude", "Name", "OpensAt", "PhotoUrl", "StartsAt" },
                values: new object[,]
                {
                    { 11, "1 Main Street", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 7), false, 42.697700m, 23.321900m, "Central Sports Park", new TimeOnly(7, 0, 0), null, new TimeOnly(10, 0, 0) },
                    { 12, "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 13), false, 42.135400m, 24.745300m, "Eastside Arena", new TimeOnly(8, 0, 0), null, new TimeOnly(20, 0, 0) },
                    { 13, "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 16), false, 42.135400m, 24.745300m, "Eastside Arena", new TimeOnly(8, 0, 0), null, new TimeOnly(18, 30, 0) },
                    { 14, "5 Vitosha Avenue", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 18), false, 42.650700m, 23.347000m, "Mountain Sports Hall", new TimeOnly(8, 0, 0), null, new TimeOnly(19, 0, 0) },
                    { 15, "8 River Road", "Varna", new TimeOnly(22, 30, 0), new DateOnly(2026, 11, 22), false, 43.214100m, 27.914700m, "Riverside Courts", new TimeOnly(6, 30, 0), null, new TimeOnly(9, 30, 0) }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                values: new object[,]
                {
                    { 1, 11, 1, 11 },
                    { 2, 12, 2, 12 },
                    { 2, 13, 4, 13 },
                    { 5, 14, 5, 14 },
                    { 3, 15, 3, 15 }
                });

            migrationBuilder.InsertData(
                table: "MatchParticipants",
                columns: new[] { "Id", "IsLateCancel", "JoinedOn", "MatchId", "Status", "StatusChangedOn", "UserId" },
                values: new object[,]
                {
                    { 53, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 11, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 54, false, new DateTime(2026, 10, 28, 16, 0, 0, 0, DateTimeKind.Unspecified), 11, 0, new DateTime(2026, 10, 28, 16, 0, 0, 0, DateTimeKind.Unspecified), "22222222-2222-2222-2222-222222222222" },
                    { 55, false, new DateTime(2026, 10, 28, 22, 0, 0, 0, DateTimeKind.Unspecified), 11, 0, new DateTime(2026, 10, 28, 22, 0, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 56, false, new DateTime(2026, 10, 29, 4, 0, 0, 0, DateTimeKind.Unspecified), 11, 0, new DateTime(2026, 10, 29, 4, 0, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 57, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "22222222-2222-2222-2222-222222222222" },
                    { 58, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 59, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "66666666-6666-6666-6666-666666666666" },
                    { 60, false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" },
                    { 61, false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 62, false, new DateTime(2026, 10, 30, 11, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 30, 11, 0, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 63, false, new DateTime(2026, 10, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 64, false, new DateTime(2026, 10, 30, 13, 0, 0, 0, DateTimeKind.Unspecified), 12, 0, new DateTime(2026, 10, 30, 13, 0, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 65, false, new DateTime(2026, 10, 30, 14, 0, 0, 0, DateTimeKind.Unspecified), 12, 1, new DateTime(2026, 10, 30, 14, 0, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 66, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 13, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 67, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 13, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "99999999-9999-9999-9999-999999999999" },
                    { 68, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 13, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { 69, false, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), 13, 0, new DateTime(2026, 10, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), "11111111-1111-1111-1111-111111111111" },
                    { 70, false, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 13, 0, new DateTime(2026, 10, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "22222222-2222-2222-2222-222222222222" },
                    { 71, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 14, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "55555555-5555-5555-5555-555555555555" },
                    { 72, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 14, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 73, false, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), 14, 0, new DateTime(2026, 10, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), "44444444-4444-4444-4444-444444444444" },
                    { 74, false, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 15, 0, new DateTime(2026, 10, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "33333333-3333-3333-3333-333333333333" },
                    { 75, false, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), 15, 0, new DateTime(2026, 10, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), "77777777-7777-7777-7777-777777777777" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 1, 11, 1, 11 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 2, 12, 2, 12 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 2, 13, 4, 13 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 5, 14, 5, 14 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 3, 15, 3, 15 });

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "MatchParticipants",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
