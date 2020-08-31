using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class onetomanyUserItinerary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryRisks_Users_AccessToken",
                table: "ItineraryRisks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryRisks",
                table: "ItineraryRisks");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "ItineraryRisks",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ItineraryRisks",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItineraryRisks",
                table: "ItineraryRisks",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryRisks_AccessToken",
                table: "ItineraryRisks",
                column: "AccessToken");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryRisks_Users_AccessToken",
                table: "ItineraryRisks",
                column: "AccessToken",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryRisks_Users_AccessToken",
                table: "ItineraryRisks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItineraryRisks",
                table: "ItineraryRisks");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryRisks_AccessToken",
                table: "ItineraryRisks");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ItineraryRisks");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "ItineraryRisks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
