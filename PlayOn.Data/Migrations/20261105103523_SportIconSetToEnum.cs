using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class SportIconSetToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconClass",
                table: "Sports");

            migrationBuilder.AddColumn<int>(
                name: "Icon",
                table: "Sports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 1,
                column: "Icon",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2,
                column: "Icon",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3,
                column: "Icon",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icon",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icon",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 6,
                column: "Icon",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 7,
                column: "Icon",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 8,
                column: "Icon",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 9,
                column: "Icon",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 10,
                column: "Icon",
                value: 9);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Sports");

            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                table: "Sports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 1,
                column: "IconClass",
                value: "fa-solid fa-futbol");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2,
                column: "IconClass",
                value: "fa-solid fa-basketball");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3,
                column: "IconClass",
                value: "fa-solid fa-table-tennis-paddle-ball");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 4,
                column: "IconClass",
                value: "fa-solid fa-volleyball");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 5,
                column: "IconClass",
                value: "fa-solid fa-table-tennis-paddle-ball");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 6,
                column: "IconClass",
                value: "fa-solid fa-feather");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 7,
                column: "IconClass",
                value: "fa-solid fa-hand");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 8,
                column: "IconClass",
                value: "fa-solid fa-football");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 9,
                column: "IconClass",
                value: "fa-solid fa-hockey-puck");

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 10,
                column: "IconClass",
                value: "fa-solid fa-baseball");
        }
    }
}
