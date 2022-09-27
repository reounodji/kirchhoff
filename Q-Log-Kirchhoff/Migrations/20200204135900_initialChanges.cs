using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class initialChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "UnknownCustomers");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "ForwardingAgency",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "Phonenumber",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "ForwardingAgency",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "Phonenumber",
                table: "OpenRegistrations");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "RegistrationHistory",
                newName: "LoadReference");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "RegistrationHistory",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "OpenRegistrations",
                newName: "LoadReference");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "OpenRegistrations",
                newName: "CompanyName");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnknownSuppliers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApproachTyp",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoadingStation",
                table: "RegistrationHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApproachTyp",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoadingStation",
                table: "OpenRegistrations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LoadingStation",
                table: "Gates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fitters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ColorCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fitters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LoadingStations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadingStations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ParcelServices",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ColorCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelServices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UnknownFitters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    FirstAppereance = table.Column<DateTime>(nullable: false),
                    NumberOfAppereances = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnknownFitters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UnknownParcelServices",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    FirstAppereance = table.Column<DateTime>(nullable: false),
                    NumberOfAppereances = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnknownParcelServices", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnknownSuppliers_Name",
                table: "UnknownSuppliers",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Fitters_Name",
                table: "Fitters",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingStations_Name",
                table: "LoadingStations",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelServices_Name",
                table: "ParcelServices",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UnknownFitters_Name",
                table: "UnknownFitters",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UnknownParcelServices_Name",
                table: "UnknownParcelServices",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fitters");

            migrationBuilder.DropTable(
                name: "LoadingStations");

            migrationBuilder.DropTable(
                name: "ParcelServices");

            migrationBuilder.DropTable(
                name: "UnknownFitters");

            migrationBuilder.DropTable(
                name: "UnknownParcelServices");

            migrationBuilder.DropIndex(
                name: "IX_UnknownSuppliers_Name",
                table: "UnknownSuppliers");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ApproachTyp",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "LoadingStation",
                table: "RegistrationHistory");

            migrationBuilder.DropColumn(
                name: "ApproachTyp",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "LoadingStation",
                table: "OpenRegistrations");

            migrationBuilder.DropColumn(
                name: "LoadingStation",
                table: "Gates");

            migrationBuilder.RenameColumn(
                name: "LoadReference",
                table: "RegistrationHistory",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "RegistrationHistory",
                newName: "Supplier");

            migrationBuilder.RenameColumn(
                name: "LoadReference",
                table: "OpenRegistrations",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "OpenRegistrations",
                newName: "Supplier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnknownSuppliers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForwardingAgency",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phonenumber",
                table: "RegistrationHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "OpenRegistrations",
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
                name: "ForwardingAgency",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phonenumber",
                table: "OpenRegistrations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UnknownCustomers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstAppereance = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NumberOfAppereances = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnknownCustomers", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                table: "Customers",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
