using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class updatedDatabaseModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_OldClusters_Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_OldClusters_Locations_Center_LocationLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropIndex(
                name: "IX_OldClusters_Center_LocationLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Center_LocationLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.AddColumn<long>(
                name: "Center_LocationOldLocation_ID",
                table: "OldClusters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_Old_ClusterReferenceID",
                table: "OldLocations",
                column: "Old_ClusterReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_OldClusters_Center_LocationOldLocation_ID",
                table: "OldClusters",
                column: "Center_LocationOldLocation_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OldClusters_OldLocations_Center_LocationOldLocation_ID",
                table: "OldClusters",
                column: "Center_LocationOldLocation_ID",
                principalTable: "OldLocations",
                principalColumn: "OldLocation_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldLocations_OldClusters_Old_ClusterReferenceID",
                table: "OldLocations",
                column: "Old_ClusterReferenceID",
                principalTable: "OldClusters",
                principalColumn: "Old_Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldClusters_OldLocations_Center_LocationOldLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_OldClusters_Old_ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_Old_ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldClusters_Center_LocationOldLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Center_LocationOldLocation_ID",
                table: "OldClusters");

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

            migrationBuilder.CreateIndex(
                name: "IX_OldClusters_Center_LocationLocation_ID",
                table: "OldClusters",
                column: "Center_LocationLocation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Old_ClusterReferenceID",
                table: "Locations",
                column: "Old_ClusterReferenceID");

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
        }
    }
}
