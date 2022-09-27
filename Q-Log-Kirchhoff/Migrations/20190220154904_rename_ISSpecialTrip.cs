using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class rename_ISSpecialTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSpecialTrip",
                table: "RegistrationHistory",
                newName: "IsSmallVehicle");

            migrationBuilder.RenameColumn(
                name: "IsSpecialTrip",
                table: "OpenRegistrations",
                newName: "IsSmallVehicle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSmallVehicle",
                table: "RegistrationHistory",
                newName: "IsSpecialTrip");

            migrationBuilder.RenameColumn(
                name: "IsSmallVehicle",
                table: "OpenRegistrations",
                newName: "IsSpecialTrip");
        }
    }
}
