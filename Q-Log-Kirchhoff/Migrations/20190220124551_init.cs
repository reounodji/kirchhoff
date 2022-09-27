using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADSettings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UseAD = table.Column<bool>(nullable: false),
                    GenerateAccountsForNewADUsers = table.Column<bool>(nullable: false),
                    ServerIP = table.Column<string>(nullable: true),
                    DomainNames = table.Column<string>(nullable: true),
                    DomainUserName = table.Column<string>(nullable: true),
                    DomainUserPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationGroups",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ADGroupName = table.Column<string>(nullable: true),
                    CanUseProcessingList = table.Column<bool>(nullable: false),
                    CanSetSpeiclaTrips = table.Column<bool>(nullable: false),
                    CanModifySupplier = table.Column<bool>(nullable: false),
                    CanModifyCustomer = table.Column<bool>(nullable: false),
                    CanSetGate = table.Column<bool>(nullable: false),
                    CanSetEntrance = table.Column<bool>(nullable: false),
                    CanSetProcessStart = table.Column<bool>(nullable: false),
                    CanSetProcessEnd = table.Column<bool>(nullable: false),
                    CanSetExit = table.Column<bool>(nullable: false),
                    CanModifySpecialTrips = table.Column<bool>(nullable: false),
                    CanUseHistory = table.Column<bool>(nullable: false),
                    CanExportHistory = table.Column<bool>(nullable: false),
                    CanUseConfig = table.Column<bool>(nullable: false),
                    CanModifyAllSettings = table.Column<bool>(nullable: false),
                    CanInspectCompanyLists = table.Column<bool>(nullable: false),
                    CanModifyCompanyLists = table.Column<bool>(nullable: false),
                    CanInspectUnknownCompanyLists = table.Column<bool>(nullable: false),
                    CanExportUnknownCompanyLists = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClosedSpecialTrips",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    TimeOfDay = table.Column<DateTime>(nullable: false),
                    ForwardingAgency = table.Column<string>(nullable: true),
                    Supplier = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    Gate = table.Column<string>(nullable: true),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false),
                    TimeOfExit = table.Column<DateTime>(nullable: false),
                    CompressedLicensePlate = table.Column<string>(nullable: true),
                    LicensePlate = table.Column<string>(nullable: true),
                    RegistrationID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    OpenSpecialTripID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedSpecialTrips", x => x.ID);
                });

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
                name: "Displays",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IPAddress = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    TcpTimeoutInMs = table.Column<int>(nullable: false),
                    ModeBreakInMs = table.Column<int>(nullable: false),
                    curDisplayedStartingIndex = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Rows = table.Column<int>(nullable: false),
                    CharsPerLine = table.Column<int>(nullable: false),
                    EntryRemovalType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Displays", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ForwardingAgencies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    IsSpecialTripForwardingAgency = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForwardingAgencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Gates",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GeneralSettings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegistrationTimeThreshold = table.Column<int>(nullable: false),
                    RecencyThresholdHours = table.Column<int>(nullable: false),
                    SpecialTripTimeThreshold = table.Column<int>(nullable: false),
                    DefaultHistoryTimespan = table.Column<int>(nullable: false),
                    DisplayUpdateInterval = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OpenRegistrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LicensePlate = table.Column<string>(nullable: true),
                    CompressedLicensePlate = table.Column<string>(nullable: true),
                    IsSpecialTrip = table.Column<bool>(nullable: false),
                    SpecialTripID = table.Column<int>(nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    ForwardingAgency = table.Column<string>(nullable: true),
                    DeliverFulls = table.Column<bool>(nullable: false),
                    DeliverEmpties = table.Column<bool>(nullable: false),
                    Supplier = table.Column<string>(nullable: true),
                    LoadFulls = table.Column<bool>(nullable: false),
                    LoadEmpties = table.Column<bool>(nullable: false),
                    Customer = table.Column<string>(nullable: true),
                    NumberOfPeople = table.Column<int>(nullable: false),
                    SAPReference = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    Gate = table.Column<string>(nullable: true),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false),
                    TimeOfCall = table.Column<DateTime>(nullable: false),
                    TimeOfEntry = table.Column<DateTime>(nullable: false),
                    ProcessStart = table.Column<DateTime>(nullable: false),
                    ProcessEnd = table.Column<DateTime>(nullable: false),
                    TimeOfExit = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenRegistrations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OpenSpecialTrips",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    TimeOfDay = table.Column<DateTime>(nullable: false),
                    ForwardingAgency = table.Column<string>(nullable: true),
                    Supplier = table.Column<string>(nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    Gate = table.Column<string>(nullable: true),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false),
                    TimeOfExit = table.Column<DateTime>(nullable: false),
                    CompressedLicensePlate = table.Column<string>(nullable: true),
                    LicensePlate = table.Column<string>(nullable: true),
                    RegistrationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenSpecialTrips", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LicensePlate = table.Column<string>(nullable: true),
                    CompressedLicensePlate = table.Column<string>(nullable: true),
                    IsSpecialTrip = table.Column<bool>(nullable: false),
                    SpecialTripID = table.Column<int>(nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    ForwardingAgency = table.Column<string>(nullable: true),
                    DeliverFulls = table.Column<bool>(nullable: false),
                    DeliverEmpties = table.Column<bool>(nullable: false),
                    Supplier = table.Column<string>(nullable: true),
                    LoadFulls = table.Column<bool>(nullable: false),
                    LoadEmpties = table.Column<bool>(nullable: false),
                    Customer = table.Column<string>(nullable: true),
                    NumberOfPeople = table.Column<int>(nullable: false),
                    SAPReference = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    Gate = table.Column<string>(nullable: true),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false),
                    TimeOfCall = table.Column<DateTime>(nullable: false),
                    TimeOfEntry = table.Column<DateTime>(nullable: false),
                    ProcessStart = table.Column<DateTime>(nullable: false),
                    ProcessEnd = table.Column<DateTime>(nullable: false),
                    TimeOfExit = table.Column<DateTime>(nullable: false),
                    OpenRegistID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMSSettings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UseSMSService = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TerminalSettings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimePerLanguage = table.Column<int>(nullable: false),
                    TimeTillReset = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminalSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UnknownCustomers",
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
                    table.PrimaryKey("PK_UnknownCustomers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UnknownForwardingAgencies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    FirstAppereance = table.Column<DateTime>(nullable: false),
                    NumberOfAppereances = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnknownForwardingAgencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UnknownSuppliers",
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
                    table.PrimaryKey("PK_UnknownSuppliers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AuthorizationGroup = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationGroups_Name",
                table: "AuthorizationGroups",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                table: "Customers",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ForwardingAgencies_Name",
                table: "ForwardingAgencies",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Gates_Name",
                table: "Gates",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UnknownForwardingAgencies_Name",
                table: "UnknownForwardingAgencies",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADSettings");

            migrationBuilder.DropTable(
                name: "AuthorizationGroups");

            migrationBuilder.DropTable(
                name: "ClosedSpecialTrips");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Displays");

            migrationBuilder.DropTable(
                name: "ForwardingAgencies");

            migrationBuilder.DropTable(
                name: "Gates");

            migrationBuilder.DropTable(
                name: "GeneralSettings");

            migrationBuilder.DropTable(
                name: "OpenRegistrations");

            migrationBuilder.DropTable(
                name: "OpenSpecialTrips");

            migrationBuilder.DropTable(
                name: "RegistrationHistory");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SMSSettings");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "TerminalSettings");

            migrationBuilder.DropTable(
                name: "UnknownCustomers");

            migrationBuilder.DropTable(
                name: "UnknownForwardingAgencies");

            migrationBuilder.DropTable(
                name: "UnknownSuppliers");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
