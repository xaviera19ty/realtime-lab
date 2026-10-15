using Microsoft.EntityFrameworkCore;
using RealtimeLab.Api.Data.Entities;

namespace RealtimeLab.Api.Data;

/// <summary>
/// 所有資料表的唯一來源。改了 Entity 之後要新增 migration 並重新匯出 db/schema.sql（見 README）。
/// 資料表與欄位一律 snake_case，讓 Go 的 sqlc 讀起來自然。
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnName("id");
            e.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(100);
            e.Property(u => u.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            e.Property(u => u.Role).HasColumnName("role").HasMaxLength(20).IsRequired().HasDefaultValue(UserRoles.User);
            e.Property(u => u.IsGuest).HasColumnName("is_guest");
            e.Property(u => u.IsDemo).HasColumnName("is_demo");
            e.Property(u => u.CreatedAt).HasColumnName("created_at");
        });
    }
}
