using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class registrationColorCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "OpenRegistrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "OpenRegistrations");
        }
    }
}
