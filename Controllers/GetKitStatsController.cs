using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Controllers
{
    [ApiController]
    [Route("ASP/[controller].aspx")]
    public class GetKitStatsController : ControllerBase
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetKitStatsController> _logger;

        public GetKitStatsController(BF2StatisticsContext context, ILogger<GetKitStatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pid)
        {
            try
            {
                _logger.LogInformation($"GetKitStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.FindAsync(pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content("O\nASPX/kits\n\n", "text/plain");
                }

                // 获取玩家装备统计
                var kitStats = await _context.Kits
                    .Where(k => k.Id == pid)
                    .FirstOrDefaultAsync();

                if (kitStats == null)
                {
                    _logger.LogInformation($"No kit stats found for player ID: {pid}");
                    return Content("O\nASPX/kits\n\n", "text/plain");
                }

                // 构建响应格式，匹配BF2统计格式
                var response = $"O\nASPX/kits\n\n" +
                    $"{kitStats.Time0}\t{kitStats.Time1}\t{kitStats.Time2}\t{kitStats.Time3}\t{kitStats.Time4}\t{kitStats.Time5}\t{kitStats.Time6}\n" +
                    $"{kitStats.Kills0}\t{kitStats.Kills1}\t{kitStats.Kills2}\t{kitStats.Kills3}\t{kitStats.Kills4}\t{kitStats.Kills5}\t{kitStats.Kills6}\n" +
                    $"{kitStats.Deaths0}\t{kitStats.Deaths1}\t{kitStats.Deaths2}\t{kitStats.Deaths3}\t{kitStats.Deaths4}\t{kitStats.Deaths5}\t{kitStats.Deaths6}\n";

                _logger.LogInformation($"Kit stats retrieved for player ID: {pid}");
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving kit stats for player ID: {pid}");
                return Content("O\nASPX/kits\n\n", "text/plain");
            }
        }
    }
}