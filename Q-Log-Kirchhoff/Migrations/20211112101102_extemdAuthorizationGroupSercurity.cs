using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class extemdAuthorizationGroupSercurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanProcessDeliveryList",
                table: "AuthorizationGroups",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanProcessLoadingList",
                table: "AuthorizationGroups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanProcessDeliveryList",
                table: "AuthorizationGroups");

            migrationBuilder.DropColumn(
                name: "CanProcessLoadingList",
                table: "AuthorizationGroups");
        }
    }
}
