using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class addInfoToSpecialTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "OpenSpecialTrips",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "ClosedSpecialTrips",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "OpenSpecialTrips");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "ClosedSpecialTrips");
        }
    }
}
