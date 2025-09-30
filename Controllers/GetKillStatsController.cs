using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Controllers
{
    [ApiController]
    [Route("ASP/[controller].aspx")]
    public class GetKillStatsController : ControllerBase
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetKillStatsController> _logger;

        public GetKillStatsController(BF2StatisticsContext context, ILogger<GetKillStatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pid)
        {
            try
            {
                _logger.LogInformation($"GetKillStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.FindAsync(pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content("O\nASPX/kills\n\n", "text/plain");
                }

                // 获取玩家所有击杀统计 - 作为攻击者的记录
                var killStats = await _context.Kills
                    .Where(k => k.Attacker == pid)
                    .OrderBy(k => k.Victim)
                    .ToListAsync();

                // 构建响应格式，匹配BF2统计格式
                var responseBuilder = new System.Text.StringBuilder();
                responseBuilder.Append("O"); // 成功响应标识符
                responseBuilder.AppendLine();
                responseBuilder.AppendLine("ASPX/kills");
                responseBuilder.AppendLine();

                foreach (var kill in killStats)
                {
                    responseBuilder.AppendLine($"{kill.Victim}\t{kill.Count}");
                }

                var response = responseBuilder.ToString();
                _logger.LogInformation($"Kill stats retrieved for player ID: {pid}, found {killStats.Count} victim records");
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving kill stats for player ID: {pid}");
                return Content("O\nASPX/kills\n\n", "text/plain");
            }
        }
    }
}