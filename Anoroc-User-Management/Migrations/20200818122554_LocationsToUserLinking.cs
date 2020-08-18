using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class LocationsToUserLinking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAccessToken",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "UserAccessToken",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "OldLocations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Locations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_AccessToken",
                table: "OldLocations",
                column: "AccessToken",
                unique: true,
                filter: "[AccessToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AccessToken",
                table: "Locations",
                column: "AccessToken",
                unique: true,
                filter: "[AccessToken] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_AccessToken",
                table: "Locations",
                column: "AccessToken",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldLocations_Users_AccessToken",
                table: "OldLocations",
                column: "AccessToken",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_AccessToken",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_Users_AccessToken",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_AccessToken",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_AccessToken",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "UserAccessToken",
                table: "OldLocations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccessToken",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
