using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class colors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExitColorCode",
                table: "GeneralSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoverColorCode",
                table: "GeneralSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewEntryColorCode",
                table: "GeneralSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecentChangeColorCode",
                table: "GeneralSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExitColorCode",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "HoverColorCode",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "NewEntryColorCode",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "RecentChangeColorCode",
                table: "GeneralSettings");
        }
    }
}
