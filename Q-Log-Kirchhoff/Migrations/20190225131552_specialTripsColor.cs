using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class specialTripsColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecialTripAssignmentColorCode",
                table: "GeneralSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialTripAssignmentColorCode",
                table: "GeneralSettings");
        }
    }
}
