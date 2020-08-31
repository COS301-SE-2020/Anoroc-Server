using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class centerLocationReferenceNotObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Center_Location_Reference",
                table: "OldClusters",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Center_Location_Reference",
                table: "OldClusters");
        }
    }
}
