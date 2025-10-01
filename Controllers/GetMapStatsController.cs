using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;
using BF2Statistics.Services;
using System.Text;

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
        public async Task<IActionResult> Index([FromQuery] int pid, [FromQuery] string info = "mtm-,mwn-,mls-")
        {
            try
            {
                _logger.LogInformation($"GetMapStats request for player ID: {pid}");

                // 验证玩家是否存在
                var player = await _context.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pid);
                if (player == null)
                {
                    _logger.LogWarning($"Player with ID {pid} not found");
                    return Content(ResponseFormatService.FormatError("Player not found"), "text/plain");
                }

                // 获取玩家在所有地图上的统计数据，并转换为字典以便快速查找
                var playerMapStats = await _context.Maps
                    .Where(m => m.Id == pid)
                    .ToDictionaryAsync(m => m.MapId, m => m);

                // 获取所有地图信息，用于构建完整的列
                var allMapInfos = await _context.MapInfos.AsNoTracking().OrderBy(mi => mi.Id).ToListAsync();

                var responseBuilder = new StringBuilder();
                responseBuilder.AppendLine("O");
                responseBuilder.AppendLine("H\tasof");
                responseBuilder.AppendLine($"D\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");

                // --- 构建Header ---
                var headers = new List<string> { "pid", "nick" };
                if (info.Contains("mtm-")) foreach (var mapInfo in allMapInfos) headers.Add($"mtm-{mapInfo.Id}");
                if (info.Contains("mwn-")) foreach (var mapInfo in allMapInfos) headers.Add($"mwn-{mapInfo.Id}");
                if (info.Contains("mls-")) foreach (var mapInfo in allMapInfos) headers.Add($"mls-{mapInfo.Id}");
                if (info.Contains("mbs-")) foreach (var mapInfo in allMapInfos) headers.Add($"mbs-{mapInfo.Id}");
                if (info.Contains("mws-")) foreach (var mapInfo in allMapInfos) headers.Add($"mws-{mapInfo.Id}");
                responseBuilder.AppendLine($"H\t{string.Join("\t", headers)}");

                // --- 构建Data ---
                var data = new List<object> { player.Id, player.Name };
                if (info.Contains("mtm-")) foreach (var mapInfo in allMapInfos) data.Add(playerMapStats.TryGetValue(mapInfo.Id, out var stat) ? stat.Time : 0);
                if (info.Contains("mwn-")) foreach (var mapInfo in allMapInfos) data.Add(playerMapStats.TryGetValue(mapInfo.Id, out var stat) ? stat.Win : 0);
                if (info.Contains("mls-")) foreach (var mapInfo in allMapInfos) data.Add(playerMapStats.TryGetValue(mapInfo.Id, out var stat) ? stat.Loss : 0);
                if (info.Contains("mbs-")) foreach (var mapInfo in allMapInfos) data.Add(playerMapStats.TryGetValue(mapInfo.Id, out var stat) ? stat.Best : 0);
                if (info.Contains("mws-")) foreach (var mapInfo in allMapInfos) data.Add(playerMapStats.TryGetValue(mapInfo.Id, out var stat) ? stat.Worst : 0);
                responseBuilder.AppendLine($"D\t{string.Join("\t", data)}");

                // --- 计算并添加校验和 ---
                var responseString = responseBuilder.ToString();
                var checksum = responseString.Replace("\t", "").Replace("\n", "").Length;
                responseBuilder.Append($"$\t{checksum}\t$");

                _logger.LogInformation($"Map stats retrieved for player ID: {pid}");
                return Content(responseBuilder.ToString(), "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving map stats for player ID: {pid}");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }
    }
}