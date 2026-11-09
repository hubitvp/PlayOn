using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedApplicationUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "ALEX.MORGAN@EXAMPLE.COM", "ALEX.MORGAN@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JORDAN.LEE@EXAMPLE.COM", "JORDAN.LEE@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "SAM.TAYLOR@EXAMPLE.COM", "SAM.TAYLOR@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "CASEY.WILSON@EXAMPLE.COM", "CASEY.WILSON@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "55555555-5555-5555-5555-555555555555",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "MORGAN.KIM@EXAMPLE.COM", "MORGAN.KIM@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66666666-6666-6666-6666-666666666666",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "RILEY.ADAMS@EXAMPLE.COM", "RILEY.ADAMS@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "77777777-7777-7777-7777-777777777777",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "TAYLOR.BROWN@EXAMPLE.COM", "TAYLOR.BROWN@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88888888-8888-8888-8888-888888888888",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JAMIE.DAVIS@EXAMPLE.COM", "JAMIE.DAVIS@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99999999-9999-9999-9999-999999999999",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "QUINN.CLARK@EXAMPLE.COM", "QUINN.CLARK@EXAMPLE.COM" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "DREW.HALL@EXAMPLE.COM", "DREW.HALL@EXAMPLE.COM" });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "Id", "Address", "City", "ClosesAt", "Date", "IsDeleted", "Latitude", "Longitude", "Name", "OpensAt", "PhotoUrl", "StartsAt" },
                values: new object[,]
                {
                    { 16, "1 Main Street", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 20), false, 42.697700m, 23.321900m, "Central Sports Park", new TimeOnly(7, 0, 0), null, new TimeOnly(18, 0, 0) },
                    { 17, "12 East Boulevard", "Plovdiv", new TimeOnly(22, 0, 0), new DateOnly(2026, 11, 21), false, 42.135400m, 24.745300m, "Eastside Arena", new TimeOnly(8, 0, 0), null, new TimeOnly(10, 0, 0) },
                    { 18, "8 River Road", "Varna", new TimeOnly(22, 30, 0), new DateOnly(2026, 11, 24), false, 43.214100m, 27.914700m, "Riverside Courts", new TimeOnly(6, 30, 0), null, new TimeOnly(19, 0, 0) },
                    { 19, "24 South Park", "Burgas", new TimeOnly(21, 30, 0), new DateOnly(2026, 11, 27), false, 42.504800m, 27.462600m, "South Field Complex", new TimeOnly(7, 30, 0), null, new TimeOnly(17, 30, 0) },
                    { 20, "5 Vitosha Avenue", "Sofia", new TimeOnly(23, 0, 0), new DateOnly(2026, 11, 28), false, 42.650700m, 23.347000m, "Mountain Sports Hall", new TimeOnly(8, 0, 0), null, new TimeOnly(11, 0, 0) },
                    { 21, "16 Harbor Avenue", "Ruse", new TimeOnly(22, 30, 0), new DateOnly(2026, 12, 3), false, 43.848000m, 25.960000m, "Danube Sports Hub", new TimeOnly(8, 30, 0), null, new TimeOnly(18, 0, 0) },
                    { 22, "44 West Street", "Sofia", new TimeOnly(22, 0, 0), new DateOnly(2026, 12, 5), false, 42.705000m, 23.270000m, "West End Club", new TimeOnly(9, 0, 0), null, new TimeOnly(15, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "ALEX.MORGAN", "alex.morgan" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JORDAN.LEE", "jordan.lee" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "SAM.TAYLOR", "sam.taylor" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "CASEY.WILSON", "casey.wilson" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "55555555-5555-5555-5555-555555555555",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "MORGAN.KIM", "morgan.kim" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66666666-6666-6666-6666-666666666666",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "RILEY.ADAMS", "riley.adams" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "77777777-7777-7777-7777-777777777777",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "TAYLOR.BROWN", "taylor.brown" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88888888-8888-8888-8888-888888888888",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "JAMIE.DAVIS", "jamie.davis" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99999999-9999-9999-9999-999999999999",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "QUINN.CLARK", "quinn.clark" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "DREW.HALL", "drew.hall" });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");
        }
    }
}
