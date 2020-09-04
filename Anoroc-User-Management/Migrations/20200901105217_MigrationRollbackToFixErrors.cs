using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class MigrationRollbackToFixErrors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_OldClusters_Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_OldClusters_Locations_Center_LocationLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_Clusters_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldClusters_Center_LocationLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Access_Token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Anoroc_Log_In",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Carrier_Status",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Facebook_Log_In",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "First_Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Google_Log_In",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Last_Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OldLocation_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "AreaReferenceID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Old_ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "UserAccessToken",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Center_LocationLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UserAccessToken",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserSurname",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "carrierStatus",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "currentlyLoggedIn",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "loggedInAnoroc",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "loggedInFacebook",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "loggedInGoogle",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "Old_Location_ID",
                table: "OldLocations",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Access_Token",
                table: "OldLocations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Area_Reference_ID",
                table: "OldLocations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ClusterOld_Cluster_Id",
                table: "OldLocations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Old_Cluster_Reference_ID",
                table: "OldLocations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Reference_ID",
                table: "OldLocations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Center_LocationOld_Location_ID",
                table: "OldClusters",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Center_Location_Reference",
                table: "OldClusters",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Reference_ID",
                table: "OldClusters",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Locations",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Center_LocationLocation_ID",
                table: "Clusters",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "AccessToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations",
                column: "Old_Location_ID");

            migrationBuilder.CreateTable(
                name: "ItineraryRisks",
                columns: table => new
                {
                    AccessToken = table.Column<string>(nullable: false),
                    ID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    TotalItineraryRisk = table.Column<int>(nullable: false),
                    LocationItineraryRisks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryRisks", x => x.AccessToken);
                    table.ForeignKey(
                        name: "FK_ItineraryRisks_Users_AccessToken",
                        column: x => x.AccessToken,
                        principalTable: "Users",
                        principalColumn: "AccessToken",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Risk = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_AccessToken",
                        column: x => x.AccessToken,
                        principalTable: "Users",
                        principalColumn: "AccessToken",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_Access_Token",
                table: "OldLocations",
                column: "Access_Token",
                unique: true,
                filter: "[Access_Token] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_ClusterOld_Cluster_Id",
                table: "OldLocations",
                column: "ClusterOld_Cluster_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OldClusters_Center_LocationOld_Location_ID",
                table: "OldClusters",
                column: "Center_LocationOld_Location_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AccessToken",
                table: "Locations",
                column: "AccessToken",
                unique: true,
                filter: "[AccessToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AccessToken",
                table: "Notifications",
                column: "AccessToken");

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters",
                column: "Center_LocationLocation_ID",
                principalTable: "Locations",
                principalColumn: "Location_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_AccessToken",
                table: "Locations",
                column: "AccessToken",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldClusters_OldLocations_Center_LocationOld_Location_ID",
                table: "OldClusters",
                column: "Center_LocationOld_Location_ID",
                principalTable: "OldLocations",
                principalColumn: "Old_Location_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldLocations_Users_Access_Token",
                table: "OldLocations",
                column: "Access_Token",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldLocations_OldClusters_ClusterOld_Cluster_Id",
                table: "OldLocations",
                column: "ClusterOld_Cluster_Id",
                principalTable: "OldClusters",
                principalColumn: "Old_Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_AccessToken",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_OldClusters_OldLocations_Center_LocationOld_Location_ID",
                table: "OldClusters");

            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_Users_Access_Token",
                table: "OldLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_OldClusters_ClusterOld_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropTable(
                name: "ItineraryRisks");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_Access_Token",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_ClusterOld_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldClusters_Center_LocationOld_Location_ID",
                table: "OldClusters");

            migrationBuilder.DropIndex(
                name: "IX_Locations_AccessToken",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserSurname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "carrierStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "currentlyLoggedIn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "loggedInAnoroc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "loggedInFacebook",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "loggedInGoogle",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Old_Location_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Access_Token",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Area_Reference_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "ClusterOld_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Old_Cluster_Reference_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Reference_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Center_LocationOld_Location_ID",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Center_Location_Reference",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Reference_ID",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Locations");

            migrationBuilder.AddColumn<long>(
                name: "User_ID",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Access_Token",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Anoroc_Log_In",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Carrier_Status",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Facebook_Log_In",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "First_Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Google_Log_In",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Last_Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OldLocation_ID",
                table: "OldLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "AreaReferenceID",
                table: "OldLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Cluster_Id",
                table: "OldLocations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Old_ClusterReferenceID",
                table: "OldLocations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccessToken",
                table: "OldLocations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Center_LocationLocation_ID",
                table: "OldClusters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Old_ClusterReferenceID",
                table: "Locations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccessToken",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Center_LocationLocation_ID",
                table: "Clusters",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "User_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations",
                column: "OldLocation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_Cluster_Id",
                table: "OldLocations",
                column: "Cluster_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OldClusters_Center_LocationLocation_ID",
                table: "OldClusters",
                column: "Center_LocationLocation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Old_ClusterReferenceID",
                table: "Locations",
                column: "Old_ClusterReferenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters",
                column: "Center_LocationLocation_ID",
                principalTable: "Locations",
                principalColumn: "Location_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_OldClusters_Old_ClusterReferenceID",
                table: "Locations",
                column: "Old_ClusterReferenceID",
                principalTable: "OldClusters",
                principalColumn: "Old_Cluster_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldClusters_Locations_Center_LocationLocation_ID",
                table: "OldClusters",
                column: "Center_LocationLocation_ID",
                principalTable: "Locations",
                principalColumn: "Location_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldLocations_Clusters_Cluster_Id",
                table: "OldLocations",
                column: "Cluster_Id",
                principalTable: "Clusters",
                principalColumn: "Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
