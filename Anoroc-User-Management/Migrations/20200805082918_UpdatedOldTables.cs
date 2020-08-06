using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class UpdatedOldTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_OldClusters_OldClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldClusters",
                table: "OldClusters");

            migrationBuilder.DropIndex(
                name: "IX_Locations_OldClusterReferenceID",
                table: "Locations");

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
                name: "Old_ClusterReferenceID",
                table: "OldLocations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Old_Cluster_Id",
                table: "OldClusters",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Old_ClusterReferenceID",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldClusters",
                table: "OldClusters",
                column: "Old_Cluster_Id");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_OldClusters_Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldClusters",
                table: "OldClusters");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Old_ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Old_Cluster_Id",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Old_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.AddColumn<long>(
                name: "OldClusterReferenceID",
                table: "OldLocations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OldCluster_Id",
                table: "OldClusters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "OldClusterReferenceID",
                table: "Locations",
                type: "bigint",
                nullable: true);

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
    }
}
