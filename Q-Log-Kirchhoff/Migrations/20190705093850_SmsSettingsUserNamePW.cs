using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class SmsSettingsUserNamePW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "SMSSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "SMSSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "SMSSettings");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "SMSSettings");
        }
    }
}
