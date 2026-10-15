using Microsoft.EntityFrameworkCore;

namespace RealtimeLab.Api.Data;

/// <summary>啟動時套用 EF Core migration；MySQL 容器剛起來時可能還連不上，所以重試幾次。</summary>
public static class DatabaseMigrator
{
    public static async Task MigrateAsync(IServiceProvider services, ILogger logger, int maxAttempts = 10)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await using var scope = services.CreateAsyncScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.MigrateAsync();
                logger.LogInformation("Database migrated");
                return;
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                logger.LogWarning(ex, "Migration attempt {Attempt} failed, retrying", attempt);
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}
