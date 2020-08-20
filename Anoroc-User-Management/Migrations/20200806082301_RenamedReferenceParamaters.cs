using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class RenamedReferenceParamaters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_RegionArea_ID",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "AreaReferenceID",
                table: "Locations");

            migrationBuilder.AlterColumn<long>(
                name: "RegionArea_ID",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_RegionArea_ID",
                table: "Locations",
                column: "RegionArea_ID",
                principalTable: "Areas",
                principalColumn: "Area_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Areas_RegionArea_ID",
                table: "Locations");

            migrationBuilder.AlterColumn<long>(
                name: "RegionArea_ID",
                table: "Locations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "AreaReferenceID",
                table: "Locations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Areas_RegionArea_ID",
                table: "Locations",
                column: "RegionArea_ID",
                principalTable: "Areas",
                principalColumn: "Area_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
