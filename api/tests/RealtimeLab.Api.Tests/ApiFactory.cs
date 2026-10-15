using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RealtimeLab.Api.Tests;

/// <summary>
/// 不需要資料庫的測試用：關掉啟動時的 migration。
/// 需要真的 MySQL、Redis 的整合測試之後用 Testcontainers 另外建立。
/// </summary>
public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Database:MigrateOnStartup", "false");
        builder.UseSetting("Core:BaseUrl", "http://127.0.0.1:1");
    }
}
