using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Controllers
{
    [ApiController]
    [Route("ASP/[controller].aspx")]
    public class GetVehicleStatsController : ControllerBase
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetVehicleStatsController> _logger;

        public GetVehicleStatsController(BF2StatisticsContext context, ILogger<GetVehicleStatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pid)
        {
            try
            {
                _logger.LogInformation($"GetVehicleStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.FindAsync(pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content("O\nASPX/vehiclestats\n\n", "text/plain");
                }

                // 获取玩家载具统计
                var vehicleStats = await _context.Vehicles
                    .Where(v => v.Id == pid)
                    .FirstOrDefaultAsync();

                if (vehicleStats == null)
                {
                    _logger.LogInformation($"No vehicle stats found for player ID: {pid}");
                    return Content("O\nASPX/vehiclestats\n\n", "text/plain");
                }

                // 构建响应格式，匹配BF2统计格式
                var response = $"O\nASPX/vehiclestats\n\n" +
                    $"{vehicleStats.Time}\t{vehicleStats.Kills}\t{vehicleStats.Deaths}\t{vehicleStats.RoadKills}\n";

                _logger.LogInformation($"Vehicle stats retrieved for player ID: {pid}");
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving vehicle stats for player ID: {pid}");
                return Content("O\nASPX/vehiclestats\n\n", "text/plain");
            }
        }
    }
}