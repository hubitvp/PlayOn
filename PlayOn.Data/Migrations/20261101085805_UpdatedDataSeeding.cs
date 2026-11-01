using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courts_Venues_VenueId",
                table: "Courts");

            migrationBuilder.DropIndex(
                name: "IX_Events_CourtId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Courts_VenueId",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "Courts");

            migrationBuilder.RenameColumn(
                name: "MatchStartsAt",
                table: "Venues",
                newName: "StartsAt");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 2, 2, 2 },
                    { 3, 3, 3, 3 },
                    { 4, 4, 4, 4 },
                    { 5, 5, 5, 5 },
                    { 6, 6, 6, 6 },
                    { 7, 7, 7, 7 },
                    { 8, 8, 8, 8 },
                    { 9, 9, 9, 9 },
                    { 10, 10, 10, 10 }
                });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 10, 13), new TimeOnly(18, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 10, 14), new TimeOnly(19, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 10, 17), new TimeOnly(10, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 10, 18), new TimeOnly(9, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 11, 9), new TimeOnly(18, 30, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 11, 10), new TimeOnly(17, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 11, 11), new TimeOnly(19, 30, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 11, 14), new TimeOnly(11, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 10, 25), new TimeOnly(14, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Date", "StartsAt" },
                values: new object[] { new DateOnly(2026, 11, 15), new TimeOnly(15, 0, 0) });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CourtId_SportId",
                table: "Events",
                columns: new[] { "CourtId", "SportId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_CourtSports_CourtId_SportId",
                table: "Events",
                columns: new[] { "CourtId", "SportId" },
                principalTable: "CourtSports",
                principalColumns: new[] { "CourtId", "SportId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_CourtSports_CourtId_SportId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CourtId_SportId",
                table: "Events");

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 1, 1, 1, 1 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 2, 2, 2, 2 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 3, 3, 3, 3 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 4, 4, 4, 4 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 5, 5, 5, 5 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 6, 6, 6, 6 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 7, 7, 7, 7 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 8, 8, 8, 8 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 9, 9, 9, 9 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumns: new[] { "CourtId", "MatchId", "SportId", "VenueId" },
                keyValues: new object[] { 10, 10, 10, 10 });

            migrationBuilder.RenameColumn(
                name: "StartsAt",
                table: "Venues",
                newName: "MatchStartsAt");

            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "Courts",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 1,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 2,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 3,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 4,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 5,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 6,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 7,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 8,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 9,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 10,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Date", "MatchStartsAt" },
                values: new object[] { new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0) });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CourtId",
                table: "Events",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_Courts_VenueId",
                table: "Courts",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courts_Venues_VenueId",
                table: "Courts",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }
    }
}
