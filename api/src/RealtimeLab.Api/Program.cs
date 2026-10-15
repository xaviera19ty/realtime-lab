using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using RealtimeLab.Api.Clients;
using RealtimeLab.Api.Data;
using RealtimeLab.Api.Infrastructure;
using Serilog;
using Serilog.Formatting.Compact;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 結構化 log：一律輸出 JSON，和 Go 的 slog 一致
builder.Host.UseSerilog((ctx, cfg) => cfg
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter()));

var config = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(config.GetConnectionString("MySql")!));

// abortConnect=false：Redis 還沒起來時不讓整個服務啟動失敗，由 /readyz 回報
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect($"{config.GetConnectionString("Redis")},abortConnect=false"));

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(config.GetConnectionString("Mongo")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<RequestIdHandler>();
builder.Services.AddHttpClient<CoreClient>(client =>
{
    client.BaseAddress = new Uri(config["Core:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(5);
}).AddHttpMessageHandler<RequestIdHandler>();

builder.Services.AddHealthChecks()
    .AddCheck<MySqlHealthCheck>("mysql", tags: ["ready"])
    .AddCheck<RedisHealthCheck>("redis", tags: ["ready"])
    .AddCheck<MongoHealthCheck>("mongo", tags: ["ready"]);

// Web API 給前台、MVC（Razor）給之後的管理後台
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (config.GetValue("Database:MigrateOnStartup", true))
{
    await DatabaseMigrator.MigrateAsync(app.Services, app.Logger);
}

app.UseMiddleware<RequestIdMiddleware>();
app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (ctx, _, ex) => ex is null && ctx.Request.Path.StartsWithSegments("/healthz")
        ? Serilog.Events.LogEventLevel.Verbose
        : Serilog.Events.LogEventLevel.Information;
});

app.MapHealthChecks("/healthz", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/readyz", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthResponseWriter.WriteAsync,
});
app.MapControllers();

app.Run();

// 讓整合測試的 WebApplicationFactory 找得到進入點
public partial class Program;
