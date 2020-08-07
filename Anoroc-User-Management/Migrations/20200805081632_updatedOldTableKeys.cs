using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class updatedOldTableKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_OldClusters_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldClusters",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Location_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Cluster_Id",
                table: "OldClusters");

            migrationBuilder.AddColumn<long>(
                name: "OldLocation_ID",
                table: "OldLocations",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "OldClusterReferenceID",
                table: "OldLocations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OldCluster_Id",
                table: "OldClusters",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "OldClusterReferenceID",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations",
                column: "OldLocation_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldClusters",
                table: "OldClusters",
                column: "OldCluster_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_OldClusterReferenceID",
                table: "Locations",
                column: "OldClusterReferenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_OldClusters_OldClusterReferenceID",
                table: "Locations",
                column: "OldClusterReferenceID",
                principalTable: "OldClusters",
                principalColumn: "OldCluster_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_OldClusters_OldClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldClusters",
                table: "OldClusters");

            migrationBuilder.DropIndex(
                name: "IX_Locations_OldClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "OldLocation_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "OldClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "OldCluster_Id",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "OldClusterReferenceID",
                table: "Locations");

            migrationBuilder.AddColumn<long>(
                name: "Location_ID",
                table: "OldLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ClusterReferenceID",
                table: "OldLocations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Cluster_Id",
                table: "OldClusters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations",
                column: "Location_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldClusters",
                table: "OldClusters",
                column: "Cluster_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_OldClusters_ClusterReferenceID",
                table: "Locations",
                column: "ClusterReferenceID",
                principalTable: "OldClusters",
                principalColumn: "Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
