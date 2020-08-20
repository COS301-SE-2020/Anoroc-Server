using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class linkingCenterLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.AlterColumn<long>(
                name: "Center_LocationLocation_ID",
                table: "Clusters",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters",
                column: "Center_LocationLocation_ID",
                principalTable: "Locations",
                principalColumn: "Location_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters");

            migrationBuilder.AlterColumn<long>(
                name: "Center_LocationLocation_ID",
                table: "Clusters",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Locations_Center_LocationLocation_ID",
                table: "Clusters",
                column: "Center_LocationLocation_ID",
                principalTable: "Locations",
                principalColumn: "Location_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
