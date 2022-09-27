using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class reinitkirchhoff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Forwarder",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phonenumber",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCall",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusInactive",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusTarget",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusYard",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Targets",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfClearance",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfDisplay",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfInaction",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Forwarder",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phonenumber",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCall",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusInactive",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusTarget",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusYard",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Targets",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfClearance",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfDisplay",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfInaction",
                table: "OpenRegistrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Forwarder",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Phonenumber",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "StatusCall",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "StatusInactive",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "StatusTarget",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "StatusYard",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Targets",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "TimeOfClearance",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "TimeOfDisplay",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "TimeOfInaction",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "Forwarder",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "Phonenumber",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "StatusCall",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "StatusInactive",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "StatusTarget",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "StatusYard",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "Targets",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "TimeOfClearance",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "TimeOfDisplay",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "TimeOfInaction",
                table: "OpenRegistrations");
        }
    }
}
