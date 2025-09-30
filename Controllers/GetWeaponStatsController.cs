using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Controllers
{
    [ApiController]
    [Route("ASP/[controller].aspx")]
    public class GetWeaponStatsController : ControllerBase
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetWeaponStatsController> _logger;

        public GetWeaponStatsController(BF2StatisticsContext context, ILogger<GetWeaponStatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pid)
        {
            try
            {
                _logger.LogInformation($"GetWeaponStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.FindAsync(pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content("O\nASPX/weaponstats\n\n", "text/plain");
                }

                // 获取玩家武器统计
                var weaponStats = await _context.Weapons
                    .Where(w => w.Id == pid)
                    .FirstOrDefaultAsync();

                if (weaponStats == null)
                {
                    _logger.LogInformation($"No weapon stats found for player ID: {pid}");
                    return Content("O\nASPX/weaponstats\n\n", "text/plain");
                }

                // 构建响应格式，匹配BF2统计格式
                var response = $"O\nASPX/weaponstats\n\n" +
                    $"{weaponStats.Time}\t{weaponStats.Kills}\t{weaponStats.Deaths}\t{weaponStats.Fired}\t{weaponStats.Hit}\t{weaponStats.Damage:F1}\t{weaponStats.Accuracy:F2}\n";

                _logger.LogInformation($"Weapon stats retrieved for player ID: {pid}");
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving weapon stats for player ID: {pid}");
                return Content("O\nASPX/weaponstats\n\n", "text/plain");
            }
        }
    }
}