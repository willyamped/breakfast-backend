using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using BuberBreakfast.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BuberBreakfast.Data
{
    public class BreakfastDbContext : IdentityDbContext<ApplicationUser>
    {
        public BreakfastDbContext(DbContextOptions<BreakfastDbContext> options)
            : base(options)
        {
        }

        public DbSet<Breakfast> Breakfasts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

            modelBuilder.Entity<Breakfast>(entity =>
            {
                entity.ToTable("Breakfasts");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.StartDateTime).IsRequired();
                entity.Property(e => e.EndDateTime).IsRequired();

                entity.Property(e => e.Savory)
                      .HasConversion(stringListConverter)
                      .HasColumnType("json");

                entity.Property(e => e.Sweet)
                      .HasConversion(stringListConverter)
                      .HasColumnType("json");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
