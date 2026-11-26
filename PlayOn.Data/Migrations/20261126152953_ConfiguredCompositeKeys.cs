using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayOn.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguredCompositeKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerRatings",
                table: "PlayerRatings");

            migrationBuilder.DropIndex(
                name: "IX_PlayerRatings_MatchId",
                table: "PlayerRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchParticipants",
                table: "MatchParticipants");

            migrationBuilder.DropIndex(
                name: "IX_MatchParticipants_MatchId",
                table: "MatchParticipants");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlayerRatings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MatchParticipants");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Matches");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerRatings",
                table: "PlayerRatings",
                columns: new[] { "MatchId", "RaterId", "RatedUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchParticipants",
                table: "MatchParticipants",
                columns: new[] { "MatchId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerRatings",
                table: "PlayerRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchParticipants",
                table: "MatchParticipants");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Venues",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PlayerRatings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MatchParticipants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerRatings",
                table: "PlayerRatings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchParticipants",
                table: "MatchParticipants",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRatings_MatchId",
                table: "PlayerRatings",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipants_MatchId",
                table: "MatchParticipants",
                column: "MatchId");
        }
    }
}
