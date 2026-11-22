using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "Optional home address of the member");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Full name of the member");

            migrationBuilder.AddColumn<DateOnly>(
                name: "JoinDate",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                comment: "Date the member joined");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("35954915-3dee-4abb-9284-4fea99eb740b"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Борисова 54", "Drew Hall", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("39ec5484-8f11-4c75-aa5f-1938bce519e7"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Борисова 32", "Riley Adams", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3ecf3b04-7a41-406b-adaf-d1d34a4067c4"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Преслав 16", "Quinn Clark", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4d530b4b-71d2-4039-8383-85d3a954c699"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Граф Игнатиев 18", "Morgan Kim", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4efb518d-d5b9-401b-893b-95fd7d67582d"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Цар Асен 15", "Taylor Brown", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5fcf6180-cd78-4b96-a2cf-4b75c63a9673"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Иван Вазов 12", "Jordan Lee", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9f33f886-e2ce-4fcc-9aa9-ca83ceb984ba"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Христо Г. Данов 27", "Jamie Davis", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9f86e28d-346a-4d51-b237-d0b67620398e"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "бул. Княз Борис I 88", "Sam Taylor", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ab4028fd-29b6-4b7b-8600-cdf4c9197729"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "бул. Витоша 45", "Alex Morgan", new DateOnly(2025, 1, 1) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d3f14be3-367f-48a6-8c34-9cc38d5875a5"),
                columns: new[] { "Address", "FullName", "JoinDate" },
                values: new object[] { "ул. Александровска 24", "Casey Wilson", new DateOnly(2025, 1, 1) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "AspNetUsers");
        }
    }
}
