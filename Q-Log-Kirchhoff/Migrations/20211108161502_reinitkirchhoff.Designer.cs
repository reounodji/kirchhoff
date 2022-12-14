// <auto-generated />
using System;
using MVC.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MVC.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20211108161502_reinitkirchhoff")]
    partial class reinitkirchhoff
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("MVC.Data.Entities.ADSettings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DomainNames");

                    b.Property<string>("DomainUserName");

                    b.Property<string>("DomainUserPassword");

                    b.Property<bool>("GenerateAccountsForNewADUsers");

                    b.Property<string>("ServerIP");

                    b.Property<bool>("UseAD");

                    b.HasKey("ID");

                    b.ToTable("ADSettings");
                });

            modelBuilder.Entity("MVC.Data.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AuthorizationGroup");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MVC.Data.Entities.AuthorizationGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ADGroupName");

                    b.Property<bool>("CanExportHistory");

                    b.Property<bool>("CanInspectApproachTyps");

                    b.Property<bool>("CanInspectUnknownApproachTyps");

                    b.Property<bool>("CanModifyAllSettings");

                    b.Property<bool>("CanModifyApproachTyps");

                    b.Property<bool>("CanModifyProcessingList");

                    b.Property<bool>("CanModifyUnknownApproachTyps");

                    b.Property<bool>("CanSetCall");

                    b.Property<bool>("CanSetEntrance");

                    b.Property<bool>("CanSetExit");

                    b.Property<bool>("CanSetGate");

                    b.Property<bool>("CanSetLoadingStation");

                    b.Property<bool>("CanSetProcessEnd");

                    b.Property<bool>("CanSetProcessStart");

                    b.Property<bool>("CanSetRelease");

                    b.Property<bool>("CanUseConfig");

                    b.Property<bool>("CanUseHistory");

                    b.Property<bool>("CanUseProcessingList");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("AuthorizationGroups");
                });

            modelBuilder.Entity("MVC.Data.Entities.BarrierControlSettings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EntryBarrierAPIUrl");

                    b.Property<string>("ExitBarrierAPIUrl");

                    b.Property<bool>("UseBarrierControl");

                    b.HasKey("ID");

                    b.ToTable("BarrierControlSettings");
                });

            modelBuilder.Entity("MVC.Data.Entities.ClosedRegistration", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApproachTyp");

                    b.Property<string>("ColorCode");

                    b.Property<string>("Comment");

                    b.Property<string>("CompanyName");

                    b.Property<string>("CompressedLicensePlate");

                    b.Property<string>("Customer");

                    b.Property<string>("FirstName");

                    b.Property<string>("Forwarder");

                    b.Property<string>("Gate");

                    b.Property<bool>("IsSmallVehicle");

                    b.Property<string>("Language");

                    b.Property<string>("Lastname");

                    b.Property<string>("LicensePlate");

                    b.Property<string>("LoadReference");

                    b.Property<string>("LoadingStation");

                    b.Property<int>("NumberOfPeople");

                    b.Property<int>("OpenRegistID");

                    b.Property<string>("Phonenumber");

                    b.Property<DateTime>("ProcessEnd");

                    b.Property<DateTime>("ProcessStart");

                    b.Property<int?>("StatusCall");

                    b.Property<int?>("StatusInactive");

                    b.Property<int?>("StatusTarget");

                    b.Property<int?>("StatusYard");

                    b.Property<string>("SupplierNumber");

                    b.Property<int?>("Targets");

                    b.Property<DateTime>("TimeOfCall");

                    b.Property<DateTime?>("TimeOfClearance");

                    b.Property<DateTime?>("TimeOfDisplay");

                    b.Property<DateTime>("TimeOfEntry");

                    b.Property<DateTime>("TimeOfExit");

                    b.Property<DateTime?>("TimeOfInaction");

                    b.Property<DateTime>("TimeOfRegistration");

                    b.Property<DateTime>("TimeOfRelease");

                    b.Property<bool>("WasSendingSuccessful");

                    b.HasKey("ID");

                    b.ToTable("RegistrationHistory");
                });

            modelBuilder.Entity("MVC.Data.Entities.DisplayConfiguration", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CharsPerLine");

                    b.Property<int>("EntryRemovalType");

                    b.Property<string>("IPAddress");

                    b.Property<int>("ModeBreakInMs");

                    b.Property<string>("Name");

                    b.Property<int>("Port");

                    b.Property<int>("Rows");

                    b.Property<int>("TcpTimeoutInMs");

                    b.Property<int>("Type");

                    b.Property<int>("curDisplayedStartingIndex");

                    b.HasKey("ID");

                    b.ToTable("Displays");
                });

            modelBuilder.Entity("MVC.Data.Entities.Fitter", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Fitters");
                });

            modelBuilder.Entity("MVC.Data.Entities.ForwardingAgency", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("ForwardingAgencies");
                });

            modelBuilder.Entity("MVC.Data.Entities.Gate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsOccupied");

                    b.Property<string>("LoadingStation");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Gates");
                });

            modelBuilder.Entity("MVC.Data.Entities.GeneralSettings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DefaultHistoryTimespan");

                    b.Property<int>("DisplayUpdateInterval");

                    b.Property<string>("ExceededWaitTimeColorCode");

                    b.Property<string>("ExitColorCode");

                    b.Property<string>("HoverColorCode");

                    b.Property<string>("NewEntryColorCode");

                    b.Property<string>("RecentChangeColorCode");

                    b.Property<int>("RegistrationTimeThreshold");

                    b.HasKey("ID");

                    b.ToTable("GeneralSettings");
                });

            modelBuilder.Entity("MVC.Data.Entities.LoadingStation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<bool>("ShowAll");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("LoadingStations");
                });

            modelBuilder.Entity("MVC.Data.Entities.OpenRegistration", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApproachTyp");

                    b.Property<string>("ColorCode");

                    b.Property<string>("Comment");

                    b.Property<string>("CompanyName");

                    b.Property<string>("CompressedLicensePlate");

                    b.Property<string>("Customer");

                    b.Property<string>("FirstName");

                    b.Property<string>("Forwarder");

                    b.Property<string>("Gate");

                    b.Property<bool>("IsSmallVehicle");

                    b.Property<string>("Language");

                    b.Property<string>("Lastname");

                    b.Property<string>("LicensePlate");

                    b.Property<string>("LoadReference");

                    b.Property<string>("LoadingStation");

                    b.Property<int>("NumberOfPeople");

                    b.Property<string>("Phonenumber");

                    b.Property<DateTime>("ProcessEnd");

                    b.Property<DateTime>("ProcessStart");

                    b.Property<int?>("StatusCall");

                    b.Property<int?>("StatusInactive");

                    b.Property<int?>("StatusTarget");

                    b.Property<int?>("StatusYard");

                    b.Property<string>("SupplierNumber");

                    b.Property<int?>("Targets");

                    b.Property<DateTime>("TimeOfCall");

                    b.Property<DateTime?>("TimeOfClearance");

                    b.Property<DateTime?>("TimeOfDisplay");

                    b.Property<DateTime>("TimeOfEntry");

                    b.Property<DateTime>("TimeOfExit");

                    b.Property<DateTime?>("TimeOfInaction");

                    b.Property<DateTime>("TimeOfRegistration");

                    b.Property<DateTime>("TimeOfRelease");

                    b.Property<bool>("WasSendingSuccessful");

                    b.HasKey("ID");

                    b.ToTable("OpenRegistrations");
                });

            modelBuilder.Entity("MVC.Data.Entities.ParcelService", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("ParcelServices");
                });

            modelBuilder.Entity("MVC.Data.Entities.SMSSettings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountReference");

                    b.Property<string>("Password");

                    b.Property<bool>("UseSMSService");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.ToTable("SMSSettings");
                });

            modelBuilder.Entity("MVC.Data.Entities.Supplier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("MVC.Data.Entities.SupplierNumber", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Number");

                    b.Property<string>("SupplierName");

                    b.HasKey("ID");

                    b.HasIndex("Number")
                        .IsUnique()
                        .HasFilter("[Number] IS NOT NULL");

                    b.ToTable("SupplierNumbers");
                });

            modelBuilder.Entity("MVC.Data.Entities.TerminalSettings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TimePerLanguage");

                    b.Property<int>("TimeTillReset");

                    b.HasKey("ID");

                    b.ToTable("TerminalSettings");
                });

            modelBuilder.Entity("MVC.Data.Entities.UnknownFitter", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FirstAppereance");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfAppereances");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("UnknownFitters");
                });

            modelBuilder.Entity("MVC.Data.Entities.UnknownForwardingAgency", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FirstAppereance");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfAppereances");

                    b.Property<string>("PostalCode");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("UnknownForwardingAgencies");
                });

            modelBuilder.Entity("MVC.Data.Entities.UnknownParcelService", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FirstAppereance");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfAppereances");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("UnknownParcelServices");
                });

            modelBuilder.Entity("MVC.Data.Entities.UnknownSupplier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FirstAppereance");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfAppereances");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("UnknownSuppliers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MVC.Data.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MVC.Data.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MVC.Data.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MVC.Data.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
