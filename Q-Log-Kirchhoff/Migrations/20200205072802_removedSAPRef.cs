using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class removedSAPRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SAPReference",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "SAPReference",
                table: "OpenRegistrations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SAPReference",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SAPReference",
                table: "OpenRegistrations",
                nullable: true);
        }
    }
}
