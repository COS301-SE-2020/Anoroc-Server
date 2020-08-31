using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_OldClusters_Old_ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_Old_ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.AddColumn<long>(
                name: "ClusterOld_Cluster_Id",
                table: "OldLocations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_ClusterOld_Cluster_Id",
                table: "OldLocations",
                column: "ClusterOld_Cluster_Id");

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
                name: "FK_OldLocations_OldClusters_ClusterOld_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_ClusterOld_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "ClusterOld_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_Old_ClusterReferenceID",
                table: "OldLocations",
                column: "Old_ClusterReferenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_OldLocations_OldClusters_Old_ClusterReferenceID",
                table: "OldLocations",
                column: "Old_ClusterReferenceID",
                principalTable: "OldClusters",
                principalColumn: "Old_Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
