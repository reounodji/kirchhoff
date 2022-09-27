using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class AddSupplierNumberforRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupplierNumber",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierNumber",
                table: "OpenRegistrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierNumber",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "SupplierNumber",
                table: "OpenRegistrations");
        }
    }
}
