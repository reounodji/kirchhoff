﻿// <auto-generated />
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
    [Migration("20190225082330_registrationColorCode")]
    partial class registrationColorCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
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

                    b.Property<bool>("CanExportUnknownCompanyLists");

                    b.Property<bool>("CanInspectCompanyLists");

                    b.Property<bool>("CanInspectUnknownCompanyLists");

                    b.Property<bool>("CanModifyAllSettings");

                    b.Property<bool>("CanModifyCompanyLists");

                    b.Property<bool>("CanModifyCustomer");

                    b.Property<bool>("CanModifySpecialTrips");

                    b.Property<bool>("CanModifySupplier");

                    b.Property<bool>("CanSetEntrance");

                    b.Property<bool>("CanSetExit");

                    b.Property<bool>("CanSetGate");

                    b.Property<bool>("CanSetProcessEnd");

                    b.Property<bool>("CanSetProcessStart");

                    b.Property<bool>("CanSetSpeiclaTrips");

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

            modelBuilder.Entity("MVC.Data.Entities.ClosedRegistration", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode");

                    b.Property<string>("CompressedLicensePlate");

                    b.Property<string>("CountryCode");

                    b.Property<string>("Customer");

                    b.Property<bool>("DeliverEmpties");

                    b.Property<bool>("DeliverFulls");

                    b.Property<string>("FirstName");

                    b.Property<string>("ForwardingAgency");

                    b.Property<string>("Gate");

                    b.Property<bool>("IsSmallVehicle");

                    b.Property<string>("Language");

                    b.Property<string>("LicensePlate");

                    b.Property<bool>("LoadEmpties");

                    b.Property<bool>("LoadFulls");

                    b.Property<int>("NumberOfPeople");

                    b.Property<int>("OpenRegistID");

                    b.Property<string>("Phonenumber");

                    b.Property<string>("PostalCode");

                    b.Property<DateTime>("ProcessEnd");

                    b.Property<DateTime>("ProcessStart");

                    b.Property<string>("SAPReference");

                    b.Property<int>("SpecialTripID");

                    b.Property<string>("Supplier");

                    b.Property<string>("Surname");

                    b.Property<DateTime>("TimeOfCall");

                    b.Property<DateTime>("TimeOfEntry");

                    b.Property<DateTime>("TimeOfExit");

                    b.Property<DateTime>("TimeOfRegistration");

                    b.HasKey("ID");

                    b.ToTable("RegistrationHistory");
                });

            modelBuilder.Entity("MVC.Data.Entities.ClosedSpecialTrip", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<string>("CompressedLicensePlate");

                    b.Property<string>("Customer");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Day");

                    b.Property<string>("ForwardingAgency");

                    b.Property<string>("Gate");

                    b.Property<string>("Information");

                    b.Property<string>("LicensePlate");

                    b.Property<int>("OpenSpecialTripID");

                    b.Property<int>("RegistrationID");

                    b.Property<string>("Supplier");

                    b.Property<DateTime>("TimeOfDay");

                    b.Property<DateTime>("TimeOfExit");

                    b.Property<DateTime>("TimeOfRegistration");

                    b.HasKey("ID");

                    b.ToTable("ClosedSpecialTrips");
                });

            modelBuilder.Entity("MVC.Data.Entities.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Customers");
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

            modelBuilder.Entity("MVC.Data.Entities.ForwardingAgency", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode");

                    b.Property<bool>("IsSpecialTripForwardingAgency");

                    b.Property<string>("Name");

                    b.Property<string>("PostalCode");

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

                    b.Property<int>("RecencyThresholdHours");

                    b.Property<int>("RegistrationTimeThreshold");

                    b.Property<int>("SpecialTripTimeThreshold");

                    b.HasKey("ID");

                    b.ToTable("GeneralSettings");
                });

            modelBuilder.Entity("MVC.Data.Entities.OpenRegistration", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode");

                    b.Property<string>("CompressedLicensePlate");

                    b.Property<string>("CountryCode");

                    b.Property<string>("Customer");

                    b.Property<bool>("DeliverEmpties");

                    b.Property<bool>("DeliverFulls");

                    b.Property<string>("FirstName");

                    b.Property<string>("ForwardingAgency");

                    b.Property<string>("Gate");

                    b.Property<bool>("IsSmallVehicle");

                    b.Property<string>("Language");

                    b.Property<string>("LicensePlate");

                    b.Property<bool>("LoadEmpties");

                    b.Property<bool>("LoadFulls");

                    b.Property<int>("NumberOfPeople");

                    b.Property<string>("Phonenumber");

                    b.Property<string>("PostalCode");

                    b.Property<DateTime>("ProcessEnd");

                    b.Property<DateTime>("ProcessStart");

                    b.Property<string>("SAPReference");

                    b.Property<int>("SpecialTripID");

                    b.Property<string>("Supplier");

                    b.Property<string>("Surname");

                    b.Property<DateTime>("TimeOfCall");

                    b.Property<DateTime>("TimeOfEntry");

                    b.Property<DateTime>("TimeOfExit");

                    b.Property<DateTime>("TimeOfRegistration");

                    b.HasKey("ID");

                    b.ToTable("OpenRegistrations");
                });

            modelBuilder.Entity("MVC.Data.Entities.OpenSpecialTrip", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompressedLicensePlate");

                    b.Property<string>("Customer");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Day");

                    b.Property<string>("ForwardingAgency");

                    b.Property<string>("Gate");

                    b.Property<string>("Information");

                    b.Property<string>("LicensePlate");

                    b.Property<int>("RegistrationID");

                    b.Property<string>("Supplier");

                    b.Property<DateTime>("TimeOfDay");

                    b.Property<DateTime>("TimeOfExit");

                    b.Property<DateTime>("TimeOfRegistration");

                    b.HasKey("ID");

                    b.ToTable("OpenSpecialTrips");
                });

            modelBuilder.Entity("MVC.Data.Entities.SMSSettings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("UseSMSService");

                    b.HasKey("ID");

                    b.ToTable("SMSSettings");
                });

            modelBuilder.Entity("MVC.Data.Entities.Supplier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Suppliers");
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

            modelBuilder.Entity("MVC.Data.Entities.UnknownCustomer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FirstAppereance");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfAppereances");

                    b.HasKey("ID");

                    b.ToTable("UnknownCustomers");
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

            modelBuilder.Entity("MVC.Data.Entities.UnknownSupplier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FirstAppereance");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfAppereances");

                    b.HasKey("ID");

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
