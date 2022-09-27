using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class delivertypRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliverEmpties",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "DeliverFulls",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "LoadEmpties",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "LoadFulls",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "DeliverEmpties",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "DeliverFulls",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "LoadEmpties",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "LoadFulls",
                table: "OpenRegistrations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeliverEmpties",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliverFulls",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadEmpties",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadFulls",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliverEmpties",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliverFulls",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadEmpties",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadFulls",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);
        }
    }
}
