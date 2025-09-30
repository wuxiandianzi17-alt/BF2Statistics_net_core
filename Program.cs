using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Connections;
var builder = WebApplication.CreateBuilder(args);

// 配置日志
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// 配置Kestrel服务器选项
builder.WebHost.ConfigureKestrel(options =>
{
    // 配置服务器选项以处理BF2游戏服务器的非标准HTTP请求
    options.Limits.MaxRequestBodySize = null; // 移除请求体大小限制
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(30);
    options.Limits.MaxRequestHeaderCount = 100;
    options.Limits.MaxRequestHeadersTotalSize = 32768;
    options.Limits.MaxRequestLineSize = 8192;
    options.Limits.MaxRequestBufferSize = null;
    options.Limits.MaxResponseBufferSize = null;
    
    // 允许同步IO操作
    options.AllowSynchronousIO = true;
    
    // 完全禁用HTTP/1.1的Content-Length验证
    options.DisableStringReuse = true;
    options.AddServerHeader = false;
    
    // 关键：禁用请求体长度验证
    options.Limits.MinRequestBodyDataRate = null;
    options.Limits.MinResponseDataRate = null;
    
    // 80端口：仅处理HTTP请求 (.aspx) → ASP.NET Core控制器处理
    options.ListenAnyIP(80, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
    
    // 8080端口：GameSpy协议专用端口 - 使用TCP socket监听
    options.ListenAnyIP(8080, listenOptions =>
    {
        // 使用TCP连接处理而不是HTTP协议
        listenOptions.UseConnectionHandler<GameSpyConnectionHandler>();
    });
});

// 启用ASP.NET Core控制器
builder.Services.AddControllers();

// 配置SQLite数据库 - 数据库文件应该在程序运行目录
var dbPath = Path.Combine(AppContext.BaseDirectory, "bf2statistics.db");
builder.Services.AddDbContext<BF2StatisticsContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// 注册服务
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<DataInitializationService>();
builder.Services.AddScoped<GameSessionService>();
builder.Services.AddScoped<PlayerDataProcessor>();
builder.Services.AddScoped<AwardsProcessingService>();
builder.Services.AddScoped<SmocGenCheckService>();
builder.Services.AddTransient<GameSpyConnectionHandler>();

// 配置CORS（如果需要）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 生产环境配置
app.UseCors("AllowAll");

// 启用控制器映射（用于处理.aspx请求）
app.MapControllers();

// 确保数据库已创建并初始化
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BF2StatisticsContext>();
    context.Database.EnsureCreated();
    
    // 初始化数据库（仅初始化必要的数据，如IP2Nation）
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();
    
    // 初始化测试数据（army, kits, vehicles, weapons, mapinfo等）
    var dataInitializer = scope.ServiceProvider.GetRequiredService<DataInitializationService>();
    await dataInitializer.InitializeTestData();
}

app.Run();
