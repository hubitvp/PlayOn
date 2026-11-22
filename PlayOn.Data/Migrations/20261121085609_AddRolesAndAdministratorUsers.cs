using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndAdministratorUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"), "22fd9079-6192-4b3b-a553-98a4812f7869", "Administrator", "ADMINISTRATOR" },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), "d6ecc739-d33f-4f53-8dfc-feac22c08b9d", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "FullName", "IsActive", "JoinDate", "LockoutEnabled", "LockoutEnd", "MatchesPlayed", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ReliabilityScore", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("1de24f29-29ee-40c5-b575-b6b85f925ef9"), 0, "бул. България 12", "Sofia", "b8844aae-c63e-4267-bfdf-231affb29c99", "Ivan Petrov", "ivan.petrov@example.com", true, "Ivan Petrov", true, new DateOnly(2025, 1, 1), false, null, 0, "IVAN.PETROV@EXAMPLE.COM", "IVAN.PETROV@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "IVAN.PETROV@EXAMPLE.COM" },
                    { new Guid("6e3ed491-c85e-4b32-b600-96aa78c2251d"), 0, "ул. Граф Игнатиев 28", "Plovdiv", "01c5b515-8d35-4036-9b47-07f413b7dd58", "Maria Ivanova", "maria.ivanova@example.com", true, "Maria Ivanova", true, new DateOnly(2025, 1, 1), false, null, 0, "MARIA.IVANOVA@EXAMPLE.COM", "MARIA.IVANOVA@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "MARIA.IVANOVA@EXAMPLE.COM" },
                    { new Guid("f754f39b-4ebe-4cbd-a705-51394372b3d3"), 0, "ул. Александровска 15", "Varna", "a2eb819e-0a44-4075-98a0-27a7e06e998c", "Georgi Dimitrov", "georgi.dimitrov@example.com", true, "Georgi Dimitrov", true, new DateOnly(2025, 1, 1), false, null, 0, "GEORGI.DIMITROV@EXAMPLE.COM", "GEORGI.DIMITROV@EXAMPLE.COM", "AQAAAAIAAYagAAAAEF86nB57LUhgocPl9wkStNa2Svp+fHGNi/3hE6or/QJMzteRck+ThqQsgLDjNlHc8A==", null, false, 100, "364894f5-ab00-5eb4-a4a7-40ac99d7f27f", false, "GEORGI.DIMITROV@EXAMPLE.COM" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"), new Guid("1de24f29-29ee-40c5-b575-b6b85f925ef9") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") },
                    { new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"), new Guid("6e3ed491-c85e-4b32-b600-96aa78c2251d") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") },
                    { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") },
                    { new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"), new Guid("f754f39b-4ebe-4cbd-a705-51394372b3d3") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"), new Guid("1de24f29-29ee-40c5-b575-b6b85f925ef9") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("35954915-3dee-4abb-9284-4fea99eb740b") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("4d530b4b-71d2-4039-8383-85d3a954c699") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"), new Guid("6e3ed491-c85e-4b32-b600-96aa78c2251d") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("9f86e28d-346a-4d51-b237-d0b67620398e") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"), new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"), new Guid("f754f39b-4ebe-4cbd-a705-51394372b3d3") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("66e04508-4a18-4c79-8bdb-24b4dc4dece0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cd705d13-51b2-4f16-bdfc-533fdc14e7c3"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1de24f29-29ee-40c5-b575-b6b85f925ef9"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e3ed491-c85e-4b32-b600-96aa78c2251d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f754f39b-4ebe-4cbd-a705-51394372b3d3"));
        }
    }
}
