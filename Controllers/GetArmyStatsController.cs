using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Controllers
{
    [ApiController]
    [Route("ASP/[controller].aspx")]
    public class GetArmyStatsController : ControllerBase
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetArmyStatsController> _logger;

        public GetArmyStatsController(BF2StatisticsContext context, ILogger<GetArmyStatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pid)
        {
            try
            {
                _logger.LogInformation($"GetArmyStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.FindAsync(pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content("ASPX/armies\n\n", "text/plain");
                }

                // 获取玩家军队统计
                var armyStats = await _context.Armies
                    .Where(a => a.Id == pid)
                    .FirstOrDefaultAsync();

                if (armyStats == null)
                {
                    _logger.LogInformation($"No army stats found for player ID: {pid}");
                    return Content("ASPX/armies\n\n", "text/plain");
                }

                // 构建响应格式，匹配BF2统计格式
                var response = $"ASPX/armies\n\n" +
                    $"{armyStats.Time0}\t{armyStats.Time1}\t{armyStats.Time2}\n" +
                    $"{armyStats.Wins}\t{armyStats.Losses}\n" +
                    $"{armyStats.Score}\t{armyStats.BestScore}\t{armyStats.WorstScore}\n";

                _logger.LogInformation($"Army stats retrieved for player ID: {pid}");
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving army stats for player ID: {pid}");
                return Content("ASPX/armies\n\n", "text/plain");
            }
        }
    }
}