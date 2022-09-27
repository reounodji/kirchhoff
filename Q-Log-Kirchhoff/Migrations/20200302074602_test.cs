using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "ForwardingAgencies");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "RegistrationHistory",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "OpenRegistrations",
                newName: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "LoadingStation",
                table: "RegistrationHistory",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "LoadingStation",
                table: "OpenRegistrations",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "RegistrationHistory",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "OpenRegistrations",
                newName: "PostalCode");

            migrationBuilder.AlterColumn<int>(
                name: "LoadingStation",
                table: "RegistrationHistory",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LoadingStation",
                table: "OpenRegistrations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "ForwardingAgencies",
                nullable: true);
        }
    }
}
