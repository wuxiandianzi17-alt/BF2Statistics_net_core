using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;
using BF2Statistics.Services;

namespace BF2Statistics.Controllers
{
    [Route("ASP/[controller].aspx")]
    public class GetRankInfoController : Controller
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetRankInfoController> _logger;

        public GetRankInfoController(BF2StatisticsContext context, ILogger<GetRankInfoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] int pid = 0)
        {
            try
            {
                if (pid <= 0 || !int.TryParse(pid.ToString(), out _))
                {
                    return Content("Invalid syntax!", "text/plain");
                }

                var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                if (player == null)
                {
                    return Content("Player not found!", "text/plain");
                }

                // 遵循最新的测试格式：单行、空格分隔
                var output = $"O H rank chng decr D {player.Rank} {player.Chng} {player.Decr}";

                // 重置chng和decr值
                player.Chng = 0;
                player.Decr = 0;
                await _context.SaveChangesAsync();

                var checksum = output.Replace(" ", string.Empty).Length;
                var response = output + $" $ {checksum} $";
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rank info");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }
    }
}