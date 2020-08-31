using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class updatedOldTablesAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldClusters_OldLocations_Center_LocationOldLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_Users_AccessToken",
                table: "OldLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_AccessToken",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldClusters_Center_LocationOldLocation_ID",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "OldLocation_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "AreaReferenceID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Old_ClusterReferenceID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Center_LocationOldLocation_ID",
                table: "OldClusters");

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
                name: "Reference_ID",
                table: "OldClusters",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations",
                column: "Old_Location_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_Access_Token",
                table: "OldLocations",
                column: "Access_Token",
                unique: true,
                filter: "[Access_Token] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OldClusters_Center_LocationOld_Location_ID",
                table: "OldClusters",
                column: "Center_LocationOld_Location_ID");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldClusters_OldLocations_Center_LocationOld_Location_ID",
                table: "OldClusters");

            migrationBuilder.DropForeignKey(
                name: "FK_OldLocations_Users_Access_Token",
                table: "OldLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldLocations_Access_Token",
                table: "OldLocations");

            migrationBuilder.DropIndex(
                name: "IX_OldClusters_Center_LocationOld_Location_ID",
                table: "OldClusters");

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
                name: "Old_Cluster_Reference_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Reference_ID",
                table: "OldLocations");

            migrationBuilder.DropColumn(
                name: "Center_LocationOld_Location_ID",
                table: "OldClusters");

            migrationBuilder.DropColumn(
                name: "Reference_ID",
                table: "OldClusters");

            migrationBuilder.AddColumn<long>(
                name: "OldLocation_ID",
                table: "OldLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "OldLocations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AreaReferenceID",
                table: "OldLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Old_ClusterReferenceID",
                table: "OldLocations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Center_LocationOldLocation_ID",
                table: "OldClusters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldLocations",
                table: "OldLocations",
                column: "OldLocation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OldLocations_AccessToken",
                table: "OldLocations",
                column: "AccessToken",
                unique: true,
                filter: "[AccessToken] IS NOT NULL");

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
                name: "FK_OldLocations_Users_AccessToken",
                table: "OldLocations",
                column: "AccessToken",
                principalTable: "Users",
                principalColumn: "AccessToken",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
