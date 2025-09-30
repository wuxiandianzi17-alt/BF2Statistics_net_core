using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using BF2Statistics.Models;
using BF2Statistics.Data;

namespace BF2Statistics.Services
{
    public class DatabaseInitializer
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(BF2StatisticsContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Starting database initialization...");
                
                // 创建数据库
                await _context.Database.EnsureCreatedAsync();
                
                // 检查是否需要初始化基础数据
                if (await _context.Ip2Nations.AnyAsync())
                {
                    _logger.LogInformation("Database already contains IP2Nation data, skipping initialization");
                    return;
                }

                // 初始化IP到国家的映射数据
                await InitializeIp2NationData();
                
                _logger.LogInformation("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database initialization");
                throw;
            }
        }

        private async Task InitializeIp2NationData()
        {
            _logger.LogInformation("Initializing IP2Nation data from SQL file...");
            
            try
            {
                // 使用相对路径，从应用程序根目录查找文件
                var sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ip2nation.sql");
                
                if (!File.Exists(sqlFilePath))
                {
                    _logger.LogWarning($"IP2Nation SQL file not found at {sqlFilePath}, using fallback data");
                    await InitializeFallbackIp2NationData();
                    return;
                }

                var ip2Nations = await ParseIp2NationSqlFile(sqlFilePath);
                
                if (ip2Nations.Any())
                {
                    // 分批插入数据以避免内存问题
                    const int batchSize = 1000;
                    var totalCount = 0;
                    
                    for (int i = 0; i < ip2Nations.Count; i += batchSize)
                    {
                        var batch = ip2Nations.Skip(i).Take(batchSize).ToList();
                        await _context.Ip2Nations.AddRangeAsync(batch);
                        await _context.SaveChangesAsync();
                        totalCount += batch.Count;
                        
                        if (totalCount % 5000 == 0)
                        {
                            _logger.LogInformation($"Loaded {totalCount} IP2Nation entries...");
                        }
                    }
                    
                    _logger.LogInformation($"IP2Nation data initialized with {totalCount} entries from SQL file");
                }
                else
                {
                    _logger.LogWarning("No valid IP2Nation data found in SQL file, using fallback data");
                    await InitializeFallbackIp2NationData();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading IP2Nation data from SQL file, using fallback data");
                await InitializeFallbackIp2NationData();
            }
        }

        private async Task<List<Models.Ip2Nation>> ParseIp2NationSqlFile(string filePath)
        {
            var ip2Nations = new List<Models.Ip2Nation>();
            var lines = await File.ReadAllLinesAsync(filePath);
            
            _logger.LogDebug($"Reading {lines.Length} lines from {filePath}");
            
            foreach (var line in lines)
            {
                // 解析INSERT INTO ip2nation语句
                if (line.StartsWith("INSERT INTO ip2nation") && line.Contains("VALUES"))
                {
                    try
                    {
                        // 解析 INSERT INTO ip2nation (ip, country) VALUES(ip_value, 'country_code');
                        var match = Regex.Match(line, @"INSERT INTO ip2nation \(ip, country\) VALUES\((\d+), '([^']+)'\);");
                        if (match.Success)
                        {
                            var ipValue = match.Groups[1].Value;
                            var countryCode = match.Groups[2].Value;
                            
                            _logger.LogDebug($"Parsing line: IP={ipValue}, Country={countryCode}");
                            
                            if (int.TryParse(ipValue, out int ip) && !string.IsNullOrEmpty(countryCode))
                            {
                                ip2Nations.Add(new Models.Ip2Nation
                                {
                                    Ip = ip,
                                    Country = countryCode
                                });
                                
                                if (ip2Nations.Count % 1000 == 0)
                                {
                                    _logger.LogDebug($"Parsed {ip2Nations.Count} records so far");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug($"Failed to parse line: {line}. Error: {ex.Message}");
                        // 继续处理其他行
                    }
                }
            }
            
            _logger.LogDebug($"Total parsed records: {ip2Nations.Count}");
            return ip2Nations;
        }

        private async Task InitializeFallbackIp2NationData()
        {
            _logger.LogInformation("Using fallback IP2Nation data...");
            
            var ip2Nations = new[]
            {
                new Models.Ip2Nation { Ip = unchecked((int)167772160), Country = "us" }, // 10.0.0.0/8
                new Models.Ip2Nation { Ip = unchecked((int)2886729728), Country = "gb" }, // 172.16.0.0/12
                new Models.Ip2Nation { Ip = unchecked((int)3232235520), Country = "de" }, // 192.168.0.0/16
                new Models.Ip2Nation { Ip = unchecked((int)2130706432), Country = "us" }, // 127.0.0.0/8
            };

            await _context.Ip2Nations.AddRangeAsync(ip2Nations);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Fallback IP2Nation data initialized with {ip2Nations.Length} entries");
        }
    }
}