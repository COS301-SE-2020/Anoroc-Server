using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class DatabaseGeneratedID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID",
                table: "ItineraryRisks");

            migrationBuilder.AddColumn<int>(
                name: "Itinerary_ID",
                table: "ItineraryRisks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Itinerary_ID",
                table: "ItineraryRisks");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ItineraryRisks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
