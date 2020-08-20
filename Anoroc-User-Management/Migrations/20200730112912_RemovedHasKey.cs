using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class RemovedHasKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Area_ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    Suburb = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Area_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(nullable: true),
                    Last_Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Access_Token = table.Column<string>(nullable: true),
                    Firebase_Token = table.Column<string>(nullable: true),
                    Facebook_Log_In = table.Column<bool>(nullable: false),
                    Google_Log_In = table.Column<bool>(nullable: false),
                    Anoroc_Log_In = table.Column<bool>(nullable: false),
                    Carrier_Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
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
                    Token = table.Column<string>(nullable: true),
                    UserAccessToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Location_ID);
                    table.ForeignKey(
                        name: "FK_Locations_Areas_RegionArea_ID",
                        column: x => x.RegionArea_ID,
                        principalTable: "Areas",
                        principalColumn: "Area_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clusters",
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
                    table.PrimaryKey("PK_Clusters", x => x.Cluster_Id);
                    table.ForeignKey(
                        name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                        column: x => x.Center_LocationLocation_ID,
                        principalTable: "Locations",
                        principalColumn: "Location_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_Center_LocationLocation_ID",
                table: "Clusters",
                column: "Center_LocationLocation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ClusterReferenceID",
                table: "Locations",
                column: "ClusterReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_RegionArea_ID",
                table: "Locations",
                column: "RegionArea_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Clusters_ClusterReferenceID",
                table: "Locations",
                column: "ClusterReferenceID",
                principalTable: "Clusters",
                principalColumn: "Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Clusters");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
