using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class TotalCarriersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Totals",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    TotalCarriers = table.Column<int>(nullable: false),
                    RegionArea_ID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Totals_Areas_RegionArea_ID",
                        column: x => x.RegionArea_ID,
                        principalTable: "Areas",
                        principalColumn: "Area_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Totals_RegionArea_ID",
                table: "Totals",
                column: "RegionArea_ID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Totals");
        }
    }
}
