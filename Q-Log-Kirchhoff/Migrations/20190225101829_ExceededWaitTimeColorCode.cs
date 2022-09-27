using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class ExceededWaitTimeColorCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExceededWaitTimeColorCode",
                table: "GeneralSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceededWaitTimeColorCode",
                table: "GeneralSettings");
        }
    }
}
