using AutoLot.Dal.Exceptions;
using AutoLot.Models.Entities;
using AutoLot.Models.Entities.Owned;
using AutoLot.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutoLot.Dal.EfStructures;

public partial class ApplicationDbContext : DbContext
{
    public DbSet<SeriLogEntry>? LogEntries { get; set; }

    public DbSet<CreditRisk>? CreditRisks { get; set; }

    public DbSet<Customer>? Customers { get; set; }

    public DbSet<Car>? Cars { get; set; }

    public DbSet<Make>? Makes { get; set; }

    public DbSet<Order>? Orders { get; set; }

    public DbSet<CustomerOrderViewModel>? CustomerOrderViewModels { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        SavingChanges += (s, e) =>
        {
            Console.WriteLine($"Saving changes for {((ApplicationDbContext)s)!.Database!.GetConnectionString()}");
        };

        SavedChanges += (s, e) =>
        {
            Console.WriteLine($"Saved {e!.EntitiesSavedCount} changes for {((ApplicationDbContext)s)!.Database!.GetConnectionString()}");
        };

        SaveChangesFailed += (s, e) =>
        {
            Console.WriteLine($"An exception occurred! {e.Exception.Message}");
        };

        ChangeTracker.Tracked += ChangeTrackerOnTracked;
        ChangeTracker.StateChanged += ChangeTrackerOnStateChanged;
    }

    private void ChangeTrackerOnStateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is not Car c)
        {
            return;
        }

        var action = string.Empty;
        Console.WriteLine($"Car {c.PetName} was {e.OldState} before the state changed to {e.NewState}");
        // Если свойство NewState сущности имеет значение Unchanged, тогда выполняется проверка
        // свойства OldState для выяснения, сущность была добавлена или же модифицирована.
        switch (e.NewState)
        {
            case EntityState.Unchanged:
                action = e.OldState switch
                {
                    EntityState.Added => "Added",
                    EntityState.Modified => "Edited",
                    _ => action
                };
                Console.WriteLine($"The object was {action}");
                break;
        }
    }

    private void ChangeTrackerOnTracked(object? sender, EntityTrackedEventArgs e)
    {
        var source = e.FromQuery ? "Database" : "Code";
        if (e.Entry.Entity is Car c)
        {
            Console.WriteLine($"Car entry {c.PetName} was added from {source}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SeriLogEntry>(entity =>
        {
            entity.Property(e => e.Properties)
                .HasColumnType("Xml");
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("GetDate()");
        });

        modelBuilder.Entity<CreditRisk>(entity =>
        {
            entity.HasOne(d => d.CustomerNavigation)
                .WithMany(p => p!.CreditRisks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CreditRisks_Customers");

            // конфигурирование свойства типа принадлежащей сущности, чтобы сопоставить с именами столбцов для FirstName и LastName,
            // и добавление вычисляемого значения для свойства FullName.
            entity.OwnsOne(o => o.PersonalInformation,
                pd =>
                {
                    pd.Property<string>(nameof(Person.FirstName))
                        .HasColumnName(nameof(Person.FirstName))
                        .HasColumnType("nvarchar(50)");
                    pd.Property<string>(nameof(Person.LastName))
                        .HasColumnName(nameof(Person.LastName))
                        .HasColumnType("nvarchar(50)");
                    pd.Property(p => p.FullName)
                        .HasColumnName(nameof(Person.FullName))
                        .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
                });
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.OwnsOne(o => o.PersonalInformation,
                pd =>
                {
                    pd.Property<string>(nameof(Person.FirstName))
                        .HasColumnName(nameof(Person.FirstName))
                        .HasColumnType("nvarchar(50)");
                    pd.Property<string>(nameof(Person.LastName))
                        .HasColumnName(nameof(Person.LastName))
                        .HasColumnType("nvarchar(50)");
                    pd.Property(p => p.FullName)
                        .HasColumnName(nameof(Person.FullName))
                        .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
                });
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasQueryFilter(c => c.IsDravable);
            // устанавливает стандартное значение свойства IsDrivable в true
            entity.Property(p => p.IsDravable)
                .HasField("_isDravable")
                .HasDefaultValue(true);
            entity.HasOne(d => d.MakeNavigation)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Make_Inventory");
        });

        modelBuilder.Entity<Make>(entity =>
        {
            entity.HasMany(e => e.Cars)
                .WithOne(c => c.MakeNavigation)
                .HasForeignKey(k => k.MakeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Make_Inventory");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.CarNavigation)
                .WithMany(p => p!.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Inventory");

            entity.HasOne(d => d.CustomerNavigation)
                .WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Customers");
        });

        // фильтр "только управляемые автомобили"
        modelBuilder.Entity<Order>().HasQueryFilter(e => e.CarNavigation!.IsDravable);

        modelBuilder.Entity<CustomerOrderViewModel>(entity =>
        {
            entity.HasNoKey().ToView("CustomerOrderView", "dbo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public override int SaveChanges()
    {
        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new CustomConcurrencyException("Произошла ошибка параллелизма!");
        }
        catch (RetryLimitExceededException ex)
        {
            throw new CustomRetryLimitExceededException("Превышен предел на кол-во повторных попыток DbResiliency!");
        }
        catch (DbUpdateException ex)
        {
            throw new CustomDbUpdateException("Произошла ошибка при обновлении базы данных!");
        }
        catch (Exception ex)
        {
            throw new CustomException("Произошла ошибка при обновлении базы данных!");
        }
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
