using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class AddedTotalsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalsID",
                table: "Areas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Totals",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Suburb = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totals", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Carriers",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCarriers = table.Column<int>(nullable: false),
                    TotalsID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carriers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Carriers_Totals_TotalsID",
                        column: x => x.TotalsID,
                        principalTable: "Totals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dates",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomDate = table.Column<DateTime>(nullable: false),
                    TotalsID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dates_Totals_TotalsID",
                        column: x => x.TotalsID,
                        principalTable: "Totals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_TotalsID",
                table: "Areas",
                column: "TotalsID");

            migrationBuilder.CreateIndex(
                name: "IX_Carriers_TotalsID",
                table: "Carriers",
                column: "TotalsID");

            migrationBuilder.CreateIndex(
                name: "IX_Dates_TotalsID",
                table: "Dates",
                column: "TotalsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Totals_TotalsID",
                table: "Areas",
                column: "TotalsID",
                principalTable: "Totals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Totals_TotalsID",
                table: "Areas");

            migrationBuilder.DropTable(
                name: "Carriers");

            migrationBuilder.DropTable(
                name: "Dates");

            migrationBuilder.DropTable(
                name: "Totals");

            migrationBuilder.DropIndex(
                name: "IX_Areas_TotalsID",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "TotalsID",
                table: "Areas");
        }
    }
}
