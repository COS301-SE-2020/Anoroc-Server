using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class UpdatedForeigkKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriers_Totals_TotalsID",
                table: "Carriers");

            migrationBuilder.DropForeignKey(
                name: "FK_Dates_Totals_TotalsID",
                table: "Dates");

            migrationBuilder.AlterColumn<long>(
                name: "TotalsID",
                table: "Dates",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TotalsID",
                table: "Carriers",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carriers_Totals_TotalsID",
                table: "Carriers",
                column: "TotalsID",
                principalTable: "Totals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dates_Totals_TotalsID",
                table: "Dates",
                column: "TotalsID",
                principalTable: "Totals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriers_Totals_TotalsID",
                table: "Carriers");

            migrationBuilder.DropForeignKey(
                name: "FK_Dates_Totals_TotalsID",
                table: "Dates");

            migrationBuilder.AlterColumn<long>(
                name: "TotalsID",
                table: "Dates",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TotalsID",
                table: "Carriers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Carriers_Totals_TotalsID",
                table: "Carriers",
                column: "TotalsID",
                principalTable: "Totals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dates_Totals_TotalsID",
                table: "Dates",
                column: "TotalsID",
                principalTable: "Totals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
