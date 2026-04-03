using Microsoft.EntityFrameworkCore;
using NestAPI.Entities;
using System.Reflection.Metadata;

namespace NestAPI.Data;

public class DateBaseContext : DbContext
{
    public DateBaseContext(DbContextOptions<DateBaseContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Entities.Document> Documents => Set<Entities.Document>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация пользователя
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
        });

        // Конфигурация документа
        modelBuilder.Entity<Entities.Document>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Title).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Content).HasMaxLength(5000);

            // Настройка связи One-to-Many
            entity.HasOne(d => d.Owner)
                  .WithMany(u => u.Documents)
                  .HasForeignKey(d => d.OwnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Глобальный фильтр для Soft Delete
            entity.HasQueryFilter(d => !d.IsDeleted);
        });
    }
}