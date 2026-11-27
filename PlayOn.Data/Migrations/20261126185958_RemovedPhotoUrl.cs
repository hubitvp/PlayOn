using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPhotoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Venues");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Venues",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);
        }
    }
}
