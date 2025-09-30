using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;
using BF2Statistics.Services;

namespace BF2Statistics.Controllers
{
    [Route("ASP/[controller].aspx")]
    public class SearchForPlayersController : Controller
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<SearchForPlayersController> _logger;

        public SearchForPlayersController(BF2StatisticsContext context, ILogger<SearchForPlayersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] string nick = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nick))
                {
                    return Content(ResponseFormatService.InvalidSyntax(), "text/plain");
                }

                var output = "O\n" +
                           "H\tasof\n" +
                           $"D\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\n" +
                           "H\tn\tpid\tnick\tscore\n";

                var players = await _context.Players
                    .Where(p => EF.Functions.Like(p.Name, $"%{nick}%"))
                    .Take(50)
                    .ToListAsync();

                var num = 1;
                var count = 31;

                foreach (var player in players)
                {
                    output += $"D\t{num++}\t{player.Id}\t{player.Name}\t{player.Score}\n";
                    count += player.Id.ToString().Length + (player.Name?.Length ?? 0) + player.Score.ToString().Length + 4;
                }

                var response = output + $"$\t{count}\t$";
                
                // 检查是否需要转置格式
                bool transpose = ResponseFormatService.ShouldTranspose(Request.Query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                if (transpose)
                {
                    // 对于search players，transpose主要影响数据的排列方式
                    // 但由于这是表格数据，我们保持原格式
                }
                
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for players");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }
    }
}