using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Controllers
{
    [ApiController]
    [Route("ASP/[controller].aspx")]
    public class GetMapStatsController : ControllerBase
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetMapStatsController> _logger;

        public GetMapStatsController(BF2StatisticsContext context, ILogger<GetMapStatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pid)
        {
            try
            {
                _logger.LogInformation($"GetMapStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.FindAsync(pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content("ASPX/maps\n\n", "text/plain");
                }

                // 获取玩家地图统计
                var mapStats = await _context.Maps
                    .Where(m => m.Id == pid)
                    .FirstOrDefaultAsync();

                if (mapStats == null)
                {
                    _logger.LogInformation($"No map stats found for player ID: {pid}");
                    return Content("ASPX/maps\n\n", "text/plain");
                }

                // 构建响应格式，匹配BF2统计格式
                var response = $"ASPX/maps\n\n" +
                    $"{mapStats.Time}\t{mapStats.Wins}\t{mapStats.Losses}\t{mapStats.Score}\t{mapStats.BestScore}\t{mapStats.WorstScore}\t{mapStats.Bested}\t{mapStats.Worsted}\n";

                _logger.LogInformation($"Map stats retrieved for player ID: {pid}");
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving map stats for player ID: {pid}");
                return Content("ASPX/maps\n\n", "text/plain");
            }
        }
    }
}