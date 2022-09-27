using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC.Data.Entities;

namespace MVC.Data.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public DbSet<OpenRegistration> OpenRegistrations { get; set; }

        public DbSet<ClosedRegistration> RegistrationHistory { get; set; }

        public DbSet<ForwardingAgency> ForwardingAgencies { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<SupplierNumber> SupplierNumbers { get; set; }

        public DbSet<Fitter> Fitters { get; set; }

        public DbSet<ParcelService> ParcelServices { get; set; }

        public DbSet<Gate> Gates { get; set; }

        public DbSet<UnknownForwardingAgency> UnknownForwardingAgencies { get; set; }

        public DbSet<UnknownSupplier> UnknownSuppliers { get; set; }

        public DbSet<UnknownFitter> UnknownFitters { get; set; }

        public DbSet<UnknownParcelService> UnknownParcelServices { get; set; }

        public DbSet<ADSettings> ADSettings { get; set; }

        public DbSet<AuthorizationGroup> AuthorizationGroups { get; set; }

        public DbSet<GeneralSettings> GeneralSettings { get; set; }

        public DbSet<LoadingStation> LoadingStations { get; set; }

        public DbSet<BarrierControlSettings> BarrierControlSettings { get; set; }

        public DbSet<TerminalSettings> TerminalSettings { get; set; }

        public DbSet<DisplayConfiguration> Displays { get; set; }

        public DbSet<SMSSettings> SMSSettings { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ForwardingAgency>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<ForwardingAgency>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<Fitter>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<Fitter>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<ParcelService>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<ParcelService>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<Supplier>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<SupplierNumber>()
            .HasKey(c => new { c.ID });
            modelBuilder.Entity<SupplierNumber>().HasIndex(g => g.Number).IsUnique();

            modelBuilder.Entity<Gate>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<Gate>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<UnknownForwardingAgency>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<UnknownForwardingAgency>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<UnknownParcelService>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<UnknownParcelService>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<UnknownFitter>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<UnknownFitter>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<UnknownSupplier>()
                    .HasKey(c => new { c.ID });
            modelBuilder.Entity<UnknownSupplier>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<LoadingStation>()
                   .HasKey(c => new { c.ID });
            modelBuilder.Entity<LoadingStation>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<ADSettings>()
                .HasKey(c => new { c.ID });

            modelBuilder.Entity<AuthorizationGroup>()
                .HasKey(c => new { c.ID });
            modelBuilder.Entity<AuthorizationGroup>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<GeneralSettings>()
                .HasKey(c => new { c.ID });

            modelBuilder.Entity<BarrierControlSettings>()
                .HasKey(c => new { c.ID });

            modelBuilder.Entity<DisplayConfiguration>()
                .HasKey(c => new { c.ID });


            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");

            });

        }
    }
}
