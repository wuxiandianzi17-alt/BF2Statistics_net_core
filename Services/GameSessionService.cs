using BF2Statistics.Data;
using BF2Statistics.Models;
using Microsoft.EntityFrameworkCore;

namespace BF2Statistics.Services
{
    public class GameSessionService
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GameSessionService> _logger;

        public GameSessionService(BF2StatisticsContext context, ILogger<GameSessionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 保存GameSpy统计数据到数据库
        /// </summary>
        /// <param name="gameSpyData">解析后的GameSpy参数</param>
        /// <param name="serverIp">服务器IP地址</param>
        /// <returns>创建的GameSession实体</returns>
        public async Task<GameSession?> SaveGameSessionAsync(Dictionary<string, string> gameSpyData, string serverIp)
        {
            try
            {
                _logger.LogInformation("开始保存GameSpy会话数据到数据库");

                var gameSession = new GameSession
                {
                    ServerIp = serverIp,
                    GamePort = GetIntValue(gameSpyData, "gameport", 0),
                    QueryPort = GetIntValue(gameSpyData, "queryport", 0),
                    MapName = GetStringValue(gameSpyData, "mapname", ""),
                    MapId = GetIntValue(gameSpyData, "mapid", 0),
                    MapStart = GetDoubleValue(gameSpyData, "mapstart", 0),
                    MapEnd = GetDoubleValue(gameSpyData, "mapend", 0),
                    Winner = GetIntValue(gameSpyData, "win", 0),
                    GameMode = GetIntValue(gameSpyData, "gm", 0),
                    ModId = GetIntValue(gameSpyData, "m", 1),
                    Version = GetStringValue(gameSpyData, "v", "bf2"),
                    PlayerCount = GetIntValue(gameSpyData, "pc", 0),
                    RoundsArmy1 = GetIntValue(gameSpyData, "ra1", 0),
                    ScoreArmy1 = GetIntValue(gameSpyData, "rs1", 0),
                    RoundsArmy2 = GetIntValue(gameSpyData, "ra2", 0),
                    ScoreArmy2 = GetIntValue(gameSpyData, "rs2", 0),
                    ScoreTeam2 = GetIntValue(gameSpyData, "rst2", 0),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.GameSessions.Add(gameSession);
                await _context.SaveChangesAsync();

                _logger.LogInformation("GameSpy会话数据已保存，ID: {SessionId}, 地图: {MapName}, 时长: {Duration}秒", 
                    gameSession.Id, gameSession.MapName, gameSession.GameDuration);

                // 同时更新或创建服务器记录
                await UpdateServerInfoAsync(gameSession);

                return gameSession;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存GameSpy会话数据时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 更新或创建服务器信息
        /// </summary>
        private async Task UpdateServerInfoAsync(GameSession session)
        {
            try
            {
                var server = await _context.Servers
                    .FirstOrDefaultAsync(s => s.Ip == session.ServerIp && s.Port == session.GamePort);

                if (server == null)
                {
                    // 创建新服务器记录
                    server = new Server
                    {
                        Ip = session.ServerIp,
                        Port = session.GamePort,
                        QueryPort = session.QueryPort,
                        Prefix = $"BF2_{session.ServerIp}_{session.GamePort}",
                        Name = $"BF2 Server {session.ServerIp}:{session.GamePort}",
                        LastUpdate = DateTime.UtcNow
                    };
                    _context.Servers.Add(server);
                }
                else
                {
                    // 更新现有服务器记录
                    server.LastUpdate = DateTime.UtcNow;
                    server.QueryPort = session.QueryPort;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("服务器信息已更新: {ServerIp}:{Port}", session.ServerIp, session.GamePort);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新服务器信息时发生错误");
            }
        }

        /// <summary>
        /// 获取最近的游戏会话
        /// </summary>
        public async Task<List<GameSession>> GetRecentSessionsAsync(int count = 10)
        {
            return await _context.GameSessions
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// 获取指定地图的统计信息
        /// </summary>
        public async Task<object> GetMapStatsAsync(string mapName)
        {
            var sessions = await _context.GameSessions
                .Where(s => s.MapName == mapName)
                .ToListAsync();

            return new
            {
                MapName = mapName,
                TotalGames = sessions.Count,
                AverageGameTime = sessions.Any() ? sessions.Average(s => s.GameDuration) : 0,
                TotalPlayTime = sessions.Sum(s => s.GameDuration)
            };
        }

        // 辅助方法
        private static string GetStringValue(Dictionary<string, string> data, string key, string defaultValue)
        {
            return data.TryGetValue(key, out var value) ? value : defaultValue;
        }

        private static int GetIntValue(Dictionary<string, string> data, string key, int defaultValue)
        {
            return data.TryGetValue(key, out var value) && int.TryParse(value, out var result) ? result : defaultValue;
        }

        private static double GetDoubleValue(Dictionary<string, string> data, string key, double defaultValue)
        {
            return data.TryGetValue(key, out var value) && double.TryParse(value, out var result) ? result : defaultValue;
        }
    }
}