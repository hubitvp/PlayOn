using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "MatchId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("04a104d8-7e99-4a6d-88b7-95a0b8de2139"),
                column: "MatchId",
                value: new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("2b525941-0519-49dd-b776-88a9310c0dc2"),
                column: "MatchId",
                value: new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("3fc5b7ee-04ac-4673-bdd9-c63ea5332aa9"),
                column: "MatchId",
                value: new Guid("5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("406cddbb-56f7-4a87-a5ec-2bc1a6051df3"),
                column: "MatchId",
                value: new Guid("04c0e2e2-35fc-4b8e-8706-6e133c403a8e"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("409f623e-0c1f-4119-a0c6-8c3dbd785cbb"),
                column: "MatchId",
                value: new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("5f244450-0dac-44a0-acd2-861088a9359f"),
                column: "MatchId",
                value: new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("61c7bc9d-299b-402c-be8f-f03af433092c"),
                column: "MatchId",
                value: new Guid("af0b763b-6efe-420a-be74-84426caed591"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("8b4cb258-9e97-45eb-b568-af8d77f21d16"),
                column: "MatchId",
                value: new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("a87d8d14-f08a-4b5c-b27b-aa83863ef9d9"),
                column: "MatchId",
                value: new Guid("1cf5ea94-367c-4999-b001-a0fb68bc4d0e"));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("fa37ca9b-2bc8-43cb-a47d-60f3cf2386fd"),
                column: "MatchId",
                value: new Guid("cad33e31-98f1-4fd8-9503-2c7f873f0725"));

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_MatchId",
                table: "Notifications",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Matches_MatchId",
                table: "Notifications",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Matches_MatchId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_MatchId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Notifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("04a104d8-7e99-4a6d-88b7-95a0b8de2139"),
                column: "Link",
                value: "/Matches/Details/04c0e2e2-35fc-4b8e-8706-6e133c403a8e");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("2b525941-0519-49dd-b776-88a9310c0dc2"),
                column: "Link",
                value: "/Matches/Details/1cf5ea94-367c-4999-b001-a0fb68bc4d0e");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("3fc5b7ee-04ac-4673-bdd9-c63ea5332aa9"),
                column: "Link",
                value: "/Matches/Details/5be5d6b6-e292-4b3f-b05d-db9ee49dc3c9");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("406cddbb-56f7-4a87-a5ec-2bc1a6051df3"),
                column: "Link",
                value: "/Matches/Details/04c0e2e2-35fc-4b8e-8706-6e133c403a8e");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("409f623e-0c1f-4119-a0c6-8c3dbd785cbb"),
                column: "Link",
                value: "/Matches/Details/cad33e31-98f1-4fd8-9503-2c7f873f0725");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("5f244450-0dac-44a0-acd2-861088a9359f"),
                column: "Link",
                value: "/Matches/Details/1cf5ea94-367c-4999-b001-a0fb68bc4d0e");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("61c7bc9d-299b-402c-be8f-f03af433092c"),
                column: "Link",
                value: "/Matches/Details/af0b763b-6efe-420a-be74-84426caed591");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("8b4cb258-9e97-45eb-b568-af8d77f21d16"),
                column: "Link",
                value: "/Matches/Details/cad33e31-98f1-4fd8-9503-2c7f873f0725");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("a87d8d14-f08a-4b5c-b27b-aa83863ef9d9"),
                column: "Link",
                value: "/Matches/Details/1cf5ea94-367c-4999-b001-a0fb68bc4d0e");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: new Guid("fa37ca9b-2bc8-43cb-a47d-60f3cf2386fd"),
                column: "Link",
                value: "/Matches/Details/cad33e31-98f1-4fd8-9503-2c7f873f0725");
        }
    }
}
