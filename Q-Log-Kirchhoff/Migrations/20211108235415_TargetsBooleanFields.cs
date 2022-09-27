using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class TargetsBooleanFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Targets",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Targets",
                table: "OpenRegistrations");

            migrationBuilder.AddColumn<bool>(
                name: "GoodsReceiptCustomerEmpties",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GoodsReceiptdelivery",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadCustomerPickup",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadEmptiesCollection",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GoodsReceiptCustomerEmpties",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GoodsReceiptdelivery",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadCustomerPickup",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoadEmptiesCollection",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodsReceiptCustomerEmpties",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "GoodsReceiptdelivery",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "LoadCustomerPickup",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "LoadEmptiesCollection",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "GoodsReceiptCustomerEmpties",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "GoodsReceiptdelivery",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "LoadCustomerPickup",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "LoadEmptiesCollection",
                table: "OpenRegistrations");

            migrationBuilder.AddColumn<int>(
                name: "Targets",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Targets",
                table: "OpenRegistrations",
                nullable: true);
        }
    }
}
