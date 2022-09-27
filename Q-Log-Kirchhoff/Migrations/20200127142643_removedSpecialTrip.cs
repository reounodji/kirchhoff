using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class removedSpecialTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedSpecialTrips");

            migrationBuilder.DropTable(
                name: "OpenSpecialTrips");

            migrationBuilder.DropColumn(
                name: "SpecialTripID",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "SpecialTripID",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "RecencyThresholdHours",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "SpecialTripAssignmentColorCode",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "SpecialTripTimeThreshold",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "IsSpecialTripForwardingAgency",
                table: "ForwardingAgencies");

            migrationBuilder.DropColumn(
                name: "CanModifySpecialTrips",
                table: "AuthorizationGroups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecialTripID",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecialTripID",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecencyThresholdHours",
                table: "GeneralSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialTripAssignmentColorCode",
                table: "GeneralSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialTripTimeThreshold",
                table: "GeneralSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialTripForwardingAgency",
                table: "ForwardingAgencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanModifySpecialTrips",
                table: "AuthorizationGroups",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ClosedSpecialTrips",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    CompressedLicensePlate = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    ForwardingAgency = table.Column<string>(nullable: true),
                    Gate = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    LicensePlate = table.Column<string>(nullable: true),
                    OpenSpecialTripID = table.Column<int>(nullable: false),
                    RegistrationID = table.Column<int>(nullable: false),
                    Supplier = table.Column<string>(nullable: true),
                    TimeOfDay = table.Column<DateTime>(nullable: false),
                    TimeOfExit = table.Column<DateTime>(nullable: false),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedSpecialTrips", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OpenSpecialTrips",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompressedLicensePlate = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    ForwardingAgency = table.Column<string>(nullable: true),
                    Gate = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    LicensePlate = table.Column<string>(nullable: true),
                    RegistrationID = table.Column<int>(nullable: false),
                    Supplier = table.Column<string>(nullable: true),
                    TimeOfDay = table.Column<DateTime>(nullable: false),
                    TimeOfExit = table.Column<DateTime>(nullable: false),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenSpecialTrips", x => x.ID);
                });
        }
    }
}
