using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class updatedReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Clusters_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.AlterColumn<long>(
                name: "Old_Cluster_Reference_ID",
                table: "OldLocations",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ClusterReferenceID",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Clusters_ClusterReferenceID",
                table: "Locations",
                column: "ClusterReferenceID",
                principalTable: "Clusters",
                principalColumn: "Cluster_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Clusters_ClusterReferenceID",
                table: "Locations");

            migrationBuilder.AlterColumn<long>(
                name: "Old_Cluster_Reference_ID",
                table: "OldLocations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ClusterReferenceID",
                table: "Locations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Clusters_ClusterReferenceID",
                table: "Locations",
                column: "ClusterReferenceID",
                principalTable: "Clusters",
                principalColumn: "Cluster_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
