using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BackendApi.Models;

namespace BackendApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure PropertyType entity
            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.ToTable("PropertyType", "Property");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            });

            // Configure Owner entity
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owner", "Owner");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.IsCompanyContact).HasDefaultValue(false);
            });

            // Configure Company entity
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company", "Owner");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CompanySite).HasMaxLength(500);
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Companies)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Property entity
            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Property", "Property");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PropertyLength).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PropertyCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Street).HasMaxLength(200);
                entity.Property(e => e.ZipCode).HasMaxLength(20);
                
                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Properties)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.PropertyType)
                      .WithMany()
                      .HasForeignKey(e => e.PropertyTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed PropertyType data
            modelBuilder.Entity<PropertyType>().HasData(
                new PropertyType { Id = 1, Type = "residential" },
                new PropertyType { Id = 2, Type = "commercial" },
                new PropertyType { Id = 3, Type = "industrial" },
                new PropertyType { Id = 4, Type = "raw land" },
                new PropertyType { Id = 5, Type = "special purpose" }
            );
        }
    }
}
