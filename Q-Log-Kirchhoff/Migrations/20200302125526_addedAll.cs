using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class addedAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowAll",
                table: "LoadingStations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowAll",
                table: "LoadingStations");
        }
    }
}
