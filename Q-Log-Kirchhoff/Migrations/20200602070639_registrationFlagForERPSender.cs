using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class registrationFlagForERPSender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WasSendingSuccessful",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WasSendingSuccessful",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WasSendingSuccessful",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "WasSendingSuccessful",
                table: "OpenRegistrations");
        }
    }
}
