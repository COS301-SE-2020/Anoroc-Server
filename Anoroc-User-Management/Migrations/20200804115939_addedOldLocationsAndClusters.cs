using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class addedOldLocationsAndClusters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OldClusters",
                columns: table => new
                {
                    Cluster_Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Center_LocationLocation_ID = table.Column<long>(nullable: true),
                    Cluster_Created = table.Column<DateTime>(nullable: false),
                    Cluster_Radius = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OldClusters", x => x.Cluster_Id);
                    table.ForeignKey(
                        name: "FK_OldClusters_Locations_Center_LocationLocation_ID",
                        column: x => x.Center_LocationLocation_ID,
                        principalTable: "Locations",
                        principalColumn: "Location_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OldLocations",
                columns: table => new
                {
                    Location_ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Carrier_Data_Point = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    AreaReferenceID = table.Column<long>(nullable: false),
                    RegionArea_ID = table.Column<long>(nullable: true),
                    ClusterReferenceID = table.Column<long>(nullable: true),
                    Cluster_Id = table.Column<long>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    UserAccessToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OldLocations", x => x.Location_ID);
                    table.ForeignKey(
                        name: "FK_OldLocations_Clusters_Cluster_Id",
                        column: x => x.Cluster_Id,
                        principalTable: "Clusters",
                        principalColumn: "Cluster_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OldLocations_Areas_RegionArea_ID",
                        column: x => x.RegionArea_ID,
                        principalTable: "Areas",
                        principalColumn: "Area_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OldClusters_Center_LocationLocation_ID",
                table: "OldClusters",
                column: "Center_LocationLocation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_Cluster_Id",
                table: "OldLocations",
                column: "Cluster_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_RegionArea_ID",
                table: "OldLocations",
                column: "RegionArea_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_OldClusters_ClusterReferenceID",
                table: "Locations",
                column: "ClusterReferenceID",
                principalTable: "OldClusters",
                principalColumn: "Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_OldClusters_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "OldClusters");

            migrationBuilder.DropTable(
                name: "OldLocations");
        }
    }
}
