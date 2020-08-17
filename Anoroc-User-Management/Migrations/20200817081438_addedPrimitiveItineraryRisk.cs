using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class addedPrimitiveItineraryRisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItineraryRisks",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    UserEmail = table.Column<string>(nullable: true),
                    TotalItineraryRisk = table.Column<int>(nullable: false),
                    LocationItineraryRisks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItineraryRisks");
        }
    }
}
