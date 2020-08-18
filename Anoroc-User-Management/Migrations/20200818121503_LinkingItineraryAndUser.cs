using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class LinkingItineraryAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAccessToken",
                table: "ItineraryRisks");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "ItineraryRisks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryRisks",
                table: "ItineraryRisks",
                column: "AccessToken");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryRisks_Users_AccessToken",
                table: "ItineraryRisks",
                column: "AccessToken",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryRisks_Users_AccessToken",
                table: "ItineraryRisks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryRisks",
                table: "ItineraryRisks");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "ItineraryRisks");

            migrationBuilder.AddColumn<string>(
                name: "UserAccessToken",
                table: "ItineraryRisks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
