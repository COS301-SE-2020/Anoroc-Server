using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anoroc_User_Management.Migrations
{
    public partial class UpdatedUserItinerary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Access_Token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Anoroc_Log_In",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Carrier_Status",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Facebook_Log_In",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "First_Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Google_Log_In",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Last_Name",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserID",
                table: "Users",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "UserSurname",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "carrierStatus",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "loggedInAnoroc",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "loggedInFacebook",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "loggedInGoogle",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "AccessToken");

            migrationBuilder.CreateTable(
                name: "ItineraryRisks",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    UserAccessToken = table.Column<string>(nullable: true),
                    TotalItineraryRisk = table.Column<int>(nullable: false),
                    LocationItineraryRisks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItineraryRisks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserSurname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "carrierStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "loggedInAnoroc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "loggedInFacebook",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "loggedInGoogle",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "User_ID",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Access_Token",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Anoroc_Log_In",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Carrier_Status",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Facebook_Log_In",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "First_Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Google_Log_In",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Last_Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "User_ID");
        }
    }
}
