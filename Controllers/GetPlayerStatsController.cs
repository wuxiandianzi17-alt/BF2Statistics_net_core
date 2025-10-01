using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Controllers
{
    [ApiController]
    [Route("ASP/[controller].aspx")]
    public class GetPlayerStatsController : ControllerBase
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetPlayerStatsController> _logger;

        public GetPlayerStatsController(BF2StatisticsContext context, ILogger<GetPlayerStatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pid)
        {
            try
            {
                _logger.LogInformation($"GetPlayerStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.FindAsync(pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content("O\nASPX/playerstats\n\n", "text/plain");
                }

                // 获取各种统计数据
                var weaponStats = await _context.Weapons
                    .Where(w => w.Id == pid)
                    .FirstOrDefaultAsync();

                var vehicleStats = await _context.Vehicles
                    .Where(v => v.Id == pid)
                    .FirstOrDefaultAsync();

                var kitStats = await _context.Kits
                    .Where(k => k.Id == pid)
                    .FirstOrDefaultAsync();

                var mapStats = await _context.Maps
                    .Where(m => m.Id == pid)
                    .FirstOrDefaultAsync();

                var armyStats = await _context.Armies
                    .Where(a => a.Id == pid)
                    .FirstOrDefaultAsync();

                // 构建综合统计响应
                var response = $"O\nASPX/playerstats\n\n";
                
                // 基础玩家信息
                response += $"PID: {player.Id}\tName: {player.Name}\tCountry: {player.Country}\n";
                response += $"Score: {player.Score}\tKills: {player.Kills}\tDeaths: {player.Deaths}\tK/D: {(player.Deaths > 0 ? (float)player.Kills / player.Deaths : 0):F2}\n";
                response += $"Time: {player.Time}\tRounds: {player.Rounds}\tWins: {player.Wins}\tLosses: {player.Losses}\n";
                response += $"Rank: {player.Rank}\tLastOnline: {player.LastOnline}\n\n";

                // 武器统计
                if (weaponStats != null)
                {
                    response += $"Weapon Stats:\tTime: {weaponStats.Time}\tKills: {weaponStats.Kills}\tDeaths: {weaponStats.Deaths}\n";
                    response += $"Fired: {weaponStats.Fired}\tHit: {weaponStats.Hit}\tAccuracy: {weaponStats.Accuracy:F2}%\n\n";
                }

                // 载具统计
                if (vehicleStats != null)
                {
                    response += $"Vehicle Stats:\tTime: {vehicleStats.Time}\tKills: {vehicleStats.Kills}\tDeaths: {vehicleStats.Deaths}\tRoadKills: {vehicleStats.RoadKills}\n\n";
                }

                // 装备统计
                if (kitStats != null)
                {
                    response += $"Kit Stats:\tTime: {kitStats.Time0},{kitStats.Time1},{kitStats.Time2},{kitStats.Time3},{kitStats.Time4},{kitStats.Time5},{kitStats.Time6}\n";
                    response += $"Kills: {kitStats.Kills0},{kitStats.Kills1},{kitStats.Kills2},{kitStats.Kills3},{kitStats.Kills4},{kitStats.Kills5},{kitStats.Kills6}\n";
                    response += $"Deaths: {kitStats.Deaths0},{kitStats.Deaths1},{kitStats.Deaths2},{kitStats.Deaths3},{kitStats.Deaths4},{kitStats.Deaths5},{kitStats.Deaths6}\n\n";
                }

                // 地图统计
                if (mapStats != null)
                {
                    response += $"Map Stats:\tTime: {mapStats.Time}\tWin: {mapStats.Win}\tLoss: {mapStats.Loss}\tBest: {mapStats.Best}\tWorst: {mapStats.Worst}\n\n";
                }

                // 军队统计
                if (armyStats != null)
                {
                    response += $"Army Stats:\tTime: {armyStats.Time0},{armyStats.Time1},{armyStats.Time2}\n";
                    response += $"Wins: {armyStats.Wins}\tLosses: {armyStats.Losses}\tScore: {armyStats.Score}\n";
                }

                _logger.LogInformation($"Player stats retrieved for player ID: {pid}");
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving player stats for player ID: {pid}");
                return Content("O\nASPX/playerstats\n\n", "text/plain");
            }
        }
    }
}