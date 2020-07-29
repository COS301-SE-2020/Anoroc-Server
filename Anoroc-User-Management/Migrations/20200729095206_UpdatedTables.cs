using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class UpdatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoCoordinate");

            migrationBuilder.AddColumn<long>(
                name: "Center_LocationLocation_ID",
                table: "Clusters",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Location_ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Carrier_Data_Point = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    RegionArea_ID = table.Column<long>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    UserAccessToken = table.Column<string>(nullable: true),
                    Cluster_ID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Location_ID);
                    table.ForeignKey(
                        name: "FK_Locations_Clusters_Cluster_ID",
                        column: x => x.Cluster_ID,
                        principalTable: "Clusters",
                        principalColumn: "Cluster_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Areas_RegionArea_ID",
                        column: x => x.RegionArea_ID,
                        principalTable: "Areas",
                        principalColumn: "Area_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_Center_LocationLocation_ID",
                table: "Clusters",
                column: "Center_LocationLocation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Cluster_ID",
                table: "Locations",
                column: "Cluster_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_RegionArea_ID",
                table: "Locations",
                column: "RegionArea_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters",
                column: "Center_LocationLocation_ID",
                principalTable: "Locations",
                principalColumn: "Location_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Clusters_Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.DropColumn(
                name: "Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.CreateTable(
                name: "GeoCoordinate",
                columns: table => new
                {
                    Altitude = table.Column<double>(type: "float", nullable: false),
                    Course = table.Column<double>(type: "float", nullable: false),
                    HorizontalAccuracy = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Speed = table.Column<double>(type: "float", nullable: false),
                    VerticalAccuracy = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                });
        }
    }
}
