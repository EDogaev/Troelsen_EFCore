using AutoLot.Samples.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Samples
{
    public partial class ApplicationDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Radio> Radios { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Inventory", "dbo");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.MakeId, "IX_Inventory_MakeId");
                entity.Property(e => e.PetName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Color)
                    .HasColumnName("CarColor")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Black");
                entity.Property(e => e.IsDravable)
                    .HasDefaultValue(true);
                entity.Property(e => e.TimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                // отношение один-ко-многим (один Make ко многим Cars)
                entity.HasOne(d => d.MakeNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.MakeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Makes_MakeId");
            });

            // отношение один-к-одному (одно Radio к одному Car)
            modelBuilder.Entity<Radio>(entity =>
            {
                entity.HasIndex(e => e.CarId)
                    .IsUnique();
                entity.HasOne(e => e.CarNavigation)
                    .WithOne(p => p.RadioNavigation)
                    .HasForeignKey<Radio>(d => d.CarId);
            });

            // отношение многие-ко-многим
            modelBuilder.Entity<Car>()
                .HasMany(p => p.Drivers)
                .WithMany(p => p.Cars)
                .UsingEntity<Dictionary<string, object>>(
                    "CarDriver",
                    j => j
                        .HasOne<Driver>()
                        .WithMany()
                        .HasForeignKey("DriverID")
                        .HasConstraintName("FK_CarDriver_Drivers_DriverId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Car>()
                        .WithMany()
                        .HasForeignKey("CarId")
                        .HasConstraintName("FK_CarDriver_Cars_CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
