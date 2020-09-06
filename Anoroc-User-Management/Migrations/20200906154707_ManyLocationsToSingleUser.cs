using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class ManyLocationsToSingleUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_AccessToken",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_AccessToken",
                table: "Locations");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "Locations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccessToken",
                table: "Locations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UserAccessToken",
                table: "Locations",
                column: "UserAccessToken");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_UserAccessToken",
                table: "Locations",
                column: "UserAccessToken",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_UserAccessToken",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_UserAccessToken",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UserAccessToken",
                table: "Locations");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
        }
    }
}
