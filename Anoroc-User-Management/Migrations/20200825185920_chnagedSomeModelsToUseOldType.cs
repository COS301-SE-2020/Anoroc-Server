using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class chnagedSomeModelsToUseOldType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_Clusters_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_Cluster_Id",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Cluster_Id",
                table: "OldLocations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Cluster_Id",
                table: "OldLocations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_Cluster_Id",
                table: "OldLocations",
                column: "Cluster_Id");

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
