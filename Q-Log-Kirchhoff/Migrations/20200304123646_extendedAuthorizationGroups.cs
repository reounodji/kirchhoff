using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class extendedAuthorizationGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanSetSpeiclaTrips",
                table: "AuthorizationGroups",
                newName: "CanSetRelease");

            migrationBuilder.RenameColumn(
                name: "CanModifySupplier",
                table: "AuthorizationGroups",
                newName: "CanSetLoadingStation");

            migrationBuilder.RenameColumn(
                name: "CanModifyCustomer",
                table: "AuthorizationGroups",
                newName: "CanSetCall");

            migrationBuilder.RenameColumn(
                name: "CanModifyCompanyLists",
                table: "AuthorizationGroups",
                newName: "CanModifyUnknownApproachTyps");

            migrationBuilder.RenameColumn(
                name: "CanInspectUnknownCompanyLists",
                table: "AuthorizationGroups",
                newName: "CanModifyProcessingList");

            migrationBuilder.RenameColumn(
                name: "CanInspectCompanyLists",
                table: "AuthorizationGroups",
                newName: "CanModifyApproachTyps");

            migrationBuilder.RenameColumn(
                name: "CanExportUnknownCompanyLists",
                table: "AuthorizationGroups",
                newName: "CanInspectUnknownApproachTyps");

            migrationBuilder.AddColumn<bool>(
                name: "CanInspectApproachTyps",
                table: "AuthorizationGroups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanInspectApproachTyps",
                table: "AuthorizationGroups");

            migrationBuilder.RenameColumn(
                name: "CanSetRelease",
                table: "AuthorizationGroups",
                newName: "CanSetSpeiclaTrips");

            migrationBuilder.RenameColumn(
                name: "CanSetLoadingStation",
                table: "AuthorizationGroups",
                newName: "CanModifySupplier");

            migrationBuilder.RenameColumn(
                name: "CanSetCall",
                table: "AuthorizationGroups",
                newName: "CanModifyCustomer");

            migrationBuilder.RenameColumn(
                name: "CanModifyUnknownApproachTyps",
                table: "AuthorizationGroups",
                newName: "CanModifyCompanyLists");

            migrationBuilder.RenameColumn(
                name: "CanModifyProcessingList",
                table: "AuthorizationGroups",
                newName: "CanInspectUnknownCompanyLists");

            migrationBuilder.RenameColumn(
                name: "CanModifyApproachTyps",
                table: "AuthorizationGroups",
                newName: "CanInspectCompanyLists");

            migrationBuilder.RenameColumn(
                name: "CanInspectUnknownApproachTyps",
                table: "AuthorizationGroups",
                newName: "CanExportUnknownCompanyLists");
        }
    }
}
