using System.Globalization;
using System.Net;
using BF2Statistics.Models;
using BF2Statistics.Data;
using Microsoft.EntityFrameworkCore;

namespace BF2Statistics.Services
{
    public class StatisticsService
    {
        private readonly ILogger<StatisticsService> _logger;
        private readonly BF2StatisticsContext _context;
        private readonly PlayerDataProcessor _playerDataProcessor;

        public StatisticsService(ILogger<StatisticsService> logger, BF2StatisticsContext context, PlayerDataProcessor playerDataProcessor)
        {
            _logger = logger;
            _context = context;
            _playerDataProcessor = playerDataProcessor;
        }

        public async Task<string> ProcessGameData(string gameData, IPAddress serverAddress)
        {
            try
            {
                _logger.LogInformation("Processing game data from server {ServerAddress}", serverAddress);
                
                // 解析游戏数据 (这里需要根据实际数据格式进行解析)
                var data = ParseGameData(gameData);
                
                // 注册或更新服务器信息
                await RegisterOrUpdateServer(data, serverAddress);
                
                // 处理玩家数据 - 使用PlayerDataProcessor处理详细的玩家和击杀数据
                await ProcessPlayerDataWithProcessor(data);
                
                // 处理军队数据
                await ProcessArmyData(data);
                
                // 处理地图数据
                await ProcessMapData(data);
                
                return "OK";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing game data from server {ServerAddress}", serverAddress);
                return "ERROR";
            }
        }

        private Dictionary<string, object> ParseGameData(string gameData)
        {
            // 改进的GameSpy数据解析逻辑，支持多种格式
            var data = new Dictionary<string, object>();
            
            if (!string.IsNullOrEmpty(gameData))
            {
                // 检测数据格式
                if (gameData.Contains("\\"))
                {
                    // GameSpy原始格式: \key\value\key\value
                    var cleanData = gameData.Trim('\\');
                    var parts = cleanData.Split('\\');
                    
                    for (int i = 0; i < parts.Length - 1; i += 2)
                    {
                        if (i + 1 < parts.Length)
                        {
                            var key = parts[i].Trim();
                            var value = parts[i + 1].Trim();
                            
                            if (!string.IsNullOrEmpty(key) && !key.Contains(' '))
                            {
                                // 尝试解析为数字，如果失败则保持为字符串
                                if (int.TryParse(value, out int intValue))
                                {
                                    data[key] = intValue;
                                }
                                else
                                {
                                    data[key] = value;
                                }
                            }
                        }
                    }
                }
                else if (gameData.Contains("="))
                {
                    // URL编码格式: key1=value1&key2=value2
                    var pairs = gameData.Split('&');
                    foreach (var pair in pairs)
                    {
                        var equalIndex = pair.IndexOf('=');
                        if (equalIndex > 0)
                        {
                            var key = pair.Substring(0, equalIndex);
                            var value = Uri.UnescapeDataString(pair.Substring(equalIndex + 1));
                            
                            // 尝试解析为数字，如果失败则保持为字符串
                            if (int.TryParse(value, out int intValue))
                            {
                                data[key] = intValue;
                            }
                            else
                            {
                                data[key] = value;
                            }
                        }
                    }
                }
                else
                {
                    // 制表符分隔格式: key\tvalue\nkey\tvalue
                    var lines = gameData.Split('\n');
                    foreach (var line in lines)
                    {
                        if (line.Contains('\t'))
                        {
                            var parts = line.Split('\t');
                            if (parts.Length >= 2)
                            {
                                var key = parts[0].Trim();
                                var value = parts[1].Trim();
                                
                                if (!string.IsNullOrEmpty(key))
                                {
                                    // 尝试解析为数字
                                    if (int.TryParse(value, out int intValue))
                                    {
                                        data[key] = intValue;
                                    }
                                    else
                                    {
                                        data[key] = value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return data;
        }

        private async Task RegisterOrUpdateServer(Dictionary<string, object> data, IPAddress serverAddress)
        {
            try
            {
                var serverIp = serverAddress.ToString();
                
                // 安全地解析端口号
                var serverPort = 0;
                if (data.ContainsKey("port"))
                {
                    if (data["port"] is string portStr && int.TryParse(portStr, out var parsedPort))
                        serverPort = parsedPort;
                    else if (data["port"] is int portInt)
                        serverPort = portInt;
                }
                
                // 查找现有服务器
                var existingServer = await _context.Servers
                    .FirstOrDefaultAsync(s => s.Ip == serverIp && s.Port == serverPort);
                
                if (existingServer == null)
                {
                    // 安全地解析queryport
                    var queryPort = 0;
                    if (data.ContainsKey("queryport"))
                    {
                        if (data["queryport"] is string queryPortStr && int.TryParse(queryPortStr, out var parsedQueryPort))
                            queryPort = parsedQueryPort;
                        else if (data["queryport"] is int queryPortInt)
                            queryPort = queryPortInt;
                    }
                    
                    // 创建新服务器记录
                    var newServer = new Server
                    {
                        Ip = serverIp,
                        Port = serverPort,
                        Prefix = data.ContainsKey("prefix") ? data["prefix"].ToString() ?? "" : "",
                        Name = data.ContainsKey("servername") ? data["servername"].ToString() : null,
                        QueryPort = queryPort,
                        RconPort = 4711, // 默认值
                        LastUpdate = DateTime.UtcNow
                    };
                    
                    _context.Servers.Add(newServer);
                    _logger.LogInformation("Registered new server: {ServerIp}:{ServerPort}", serverIp, serverPort);
                }
                else
                {
                    // 更新现有服务器信息
                    if (data.ContainsKey("prefix"))
                        existingServer.Prefix = data["prefix"].ToString() ?? "";
                    if (data.ContainsKey("servername"))
                        existingServer.Name = data["servername"].ToString();
                    if (data.ContainsKey("queryport"))
                    {
                        if (data["queryport"] is string queryPortStr && int.TryParse(queryPortStr, out var parsedQueryPort))
                            existingServer.QueryPort = parsedQueryPort;
                        else if (data["queryport"] is int queryPortInt)
                            existingServer.QueryPort = queryPortInt;
                    }
                    
                    existingServer.LastUpdate = DateTime.UtcNow;
                    
                    _logger.LogInformation("Updated server: {ServerIp}:{ServerPort}", serverIp, serverPort);
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering/updating server {ServerAddress}", serverAddress);
                throw;
            }
        }

        private async Task ProcessPlayerDataWithProcessor(Dictionary<string, object> data)
        {
            try
            {
                // 将Dictionary<string, object>转换为Dictionary<string, string>以供PlayerDataProcessor使用
                var gameSpyData = data.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value?.ToString() ?? ""
                );

                // 使用PlayerDataProcessor处理详细的玩家数据和击杀关系
                await _playerDataProcessor.ProcessSnapshotDataAsync(gameSpyData);

                // 处理奖励数据
                await ProcessPlayerData(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理玩家数据时发生错误");
            }
        }

        private async Task ProcessPlayerData(Dictionary<string, object> data)
        {
            try
            {
                // 从GameSpy数据中提取玩家奖励信息
                // 检查是否有奖励相关的数据
                var awardKeys = data.Keys.Where(k => k.StartsWith("award_") || k.Contains("medal") || k.Contains("ribbon")).ToList();
                
                if (awardKeys.Any())
                {
                    _logger.LogInformation("发现 {Count} 个奖励相关的数据键", awardKeys.Count);
                    
                    foreach (var awardKey in awardKeys)
                    {
                        await ProcessAwardDataAsync(awardKey, data[awardKey]);
                    }
                }

                // 处理其他玩家相关数据
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理玩家数据时发生错误");
            }
        }

        /// <summary>
        /// 处理奖励数据
        /// </summary>
        private async Task ProcessAwardDataAsync(string awardKey, object awardValue)
        {
            try
            {
                // 解析奖励键，提取奖励ID和玩家ID
                // 例如：award_123_456 表示玩家456获得奖励123
                var parts = awardKey.Split('_');
                if (parts.Length < 3) return;

                if (!int.TryParse(parts[1], out int awardId)) return;
                if (!int.TryParse(parts[2], out int playerId)) return;

                // 获取玩家名称
                var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
                string playerName = player?.Name ?? $"Player{playerId}";

                // 检查奖励是否已存在
                var existingAward = await _context.Awards
                    .FirstOrDefaultAsync(a => a.AwardId == awardId && a.PlayerId == playerId);

                if (existingAward == null)
                {
                    // 创建新奖励记录
                    var newAward = new Award
                    {
                        PlayerId = playerId,
                        PlayerName = playerName,
                        AwardId = awardId,
                        Level = 1,
                        When = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        First = 1,
                        Earned = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    };

                    _context.Awards.Add(newAward);
                    _logger.LogInformation("创建新奖励记录: PlayerId {PlayerId}, PlayerName {PlayerName}, AwardId {AwardId}", playerId, playerName, awardId);
                }
                else
                {
                    // 更新现有奖励
                    existingAward.Level++;
                    existingAward.PlayerName = playerName; // 更新玩家名称以防变化
                    existingAward.Earned = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    _logger.LogDebug("更新奖励记录: PlayerId {PlayerId}, AwardId {AwardId}, 新等级: {Level}", playerId, awardId, existingAward.Level);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理奖励数据时发生错误: {AwardKey}", awardKey);
            }
        }

        private async Task ProcessArmyData(Dictionary<string, object> data)
        {
            // 处理军队统计数据的逻辑
            await Task.CompletedTask;
        }

        private async Task ProcessMapData(Dictionary<string, object> data)
        {
            try
            {
                // 从GameSpy数据中提取地图信息
                if (!data.TryGetValue("mapname", out var mapNameObj) || mapNameObj == null)
                {
                    _logger.LogWarning("GameSpy数据中缺少mapname字段");
                    return;
                }

                string mapName = mapNameObj.ToString() ?? "";
                if (string.IsNullOrEmpty(mapName))
                {
                    _logger.LogWarning("地图名称为空");
                    return;
                }

                // 提取其他地图相关信息
                data.TryGetValue("mapid", out var mapIdObj);
                data.TryGetValue("mapstart", out var mapStartObj);
                data.TryGetValue("mapend", out var mapEndObj);

                int mapId = 0;
                if (mapIdObj != null && int.TryParse(mapIdObj.ToString(), out int parsedMapId))
                {
                    mapId = parsedMapId;
                }

                // 查找或创建MapInfo记录
                var existingMapInfo = await _context.MapInfos
                    .FirstOrDefaultAsync(m => m.Name == mapName);

                if (existingMapInfo == null)
                {
                    // 创建新的地图信息记录
                    var newMapInfo = new MapInfo
                    {
                        Name = mapName
                    };

                    _context.MapInfos.Add(newMapInfo);
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("创建新地图信息记录: {MapName}", mapName);
                }
                else
                {
                    _logger.LogDebug("地图信息已存在: {MapName}", mapName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理地图数据时发生错误");
            }
        }

        public string GetCountryFromIp(string ip)
        {
            try
            {
                var ipBytes = System.Net.IPAddress.Parse(ip).GetAddressBytes();
                var ipValue = BitConverter.ToUInt32(ipBytes.Reverse().ToArray(), 0);
                
                // 添加调试日志
                Console.WriteLine($"Looking up IP: {ip} -> {ipValue}");
                
                var totalRecords = _context.Ip2Nations.Count();
                Console.WriteLine($"Total IP2Nation records: {totalRecords}");
                
                // IP2Nation数据库存储的是IP范围的起始值，需要查找小于等于目标IP的最大值
                var ip2Nation = _context.Ip2Nations
                    .Where(i => i.Ip <= (int)ipValue)
                    .OrderByDescending(i => i.Ip)
                    .FirstOrDefault();
                    
                Console.WriteLine($"Found record: {ip2Nation?.Ip} -> {ip2Nation?.Country}");
                    
                return ip2Nation?.Country ?? "xx";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCountryFromIp: {ex.Message}");
                return "xx";
            }
        }

        public int GetUnixTimestamp()
        {
            return (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}