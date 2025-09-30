using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;
using BF2Statistics.Services;

namespace BF2Statistics.Controllers
{
    [Route("ASP/[controller].aspx")]
    public class GetLeaderboardController : Controller
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetLeaderboardController> _logger;

        public GetLeaderboardController(BF2StatisticsContext context, ILogger<GetLeaderboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] string type = "", [FromQuery] string id = "", [FromQuery] int pid = 0, [FromQuery] int after = 1, [FromQuery] int before = 0, [FromQuery] int pos = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    return Content(ResponseFormatService.InvalidSyntax(), "text/plain");
                }

                var output = "O\n";
                output += "H\tsize\tasof\n";
                var num = 10;

                var min = (pos - 1) - before;
                var max = after;

                if (type == "score")
                {
                    if (id == "overall")
                    {
                        var totalCount = await _context.Players.CountAsync(p => p.Score > 0);
                        output += $"D\t{totalCount}\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\n";
                        num += totalCount.ToString().Length + 11;
                        output += "H\tn\tpid\tnick\tscore\ttotaltime\tplayerrank\tcountrycode\n";
                        num += 44;

                        if (pid == 0)
                        {
                            var players = await _context.Players
                                .Where(p => p.Score > 0)
                                .OrderByDescending(p => p.Score)
                                .Skip(min)
                                .Take(max)
                                .ToListAsync();

                            foreach (var player in players)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t{pos++}\t{player.Id}\t{player.Name}\t{player.Score}\t{player.Time}\t{player.Rank}\t{country}\n";
                                num += pos.ToString().Length + player.Id.ToString().Length + (player.Name?.Length ?? 0) + 
                                      player.Score.ToString().Length + player.Time.ToString().Length + player.Rank.ToString().Length + 3;
                            }
                        }
                        else
                        {
                            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                            if (player != null)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t1\t{pid}\t{player.Name}\t{player.Score}\t{player.Time}\t{player.Rank}\t{country}\n";
                                num += pid.ToString().Length + (player.Name?.Length ?? 0) + player.Score.ToString().Length + 
                                      player.Time.ToString().Length + player.Rank.ToString().Length + 4;
                            }
                        }
                    }
                    else if (id == "commander")
                    {
                        var totalCount = await _context.Players.CountAsync(p => p.CmdScore > 0);
                        output += $"D\t{totalCount}\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\n";
                        num += totalCount.ToString().Length + 11;
                        output += "H\tn\tpid\tnick\tcoscore\tcotime\tplayerrank\tcountrycode\n";
                        num += 43;

                        if (pid == 0)
                        {
                            var players = await _context.Players
                                .Where(p => p.CmdScore > 0)
                                .OrderByDescending(p => p.CmdScore)
                                .Skip(min)
                                .Take(max)
                                .ToListAsync();

                            foreach (var player in players)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t{pos++}\t{player.Id}\t{player.Name}\t{player.CmdScore}\t{player.CmdTime}\t{player.Rank}\t{country}\n";
                                num += pos.ToString().Length + player.Id.ToString().Length + (player.Name?.Length ?? 0) + 
                                      player.CmdScore.ToString().Length + player.CmdTime.ToString().Length + player.Rank.ToString().Length + 3;
                            }
                        }
                        else
                        {
                            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                            if (player != null)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t1\t{pid}\t{player.Name}\t{player.CmdScore}\t{player.CmdTime}\t{player.Rank}\t{country}\n";
                                num += pid.ToString().Length + (player.Name?.Length ?? 0) + player.CmdScore.ToString().Length + 
                                      player.CmdTime.ToString().Length + player.Rank.ToString().Length + 4;
                            }
                        }
                    }
                    else if (id == "team")
                    {
                        var totalCount = await _context.Players.CountAsync(p => p.TeamScore > 0);
                        output += $"D\t{totalCount}\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\n";
                        num += totalCount.ToString().Length + 11;
                        output += "H\tn\tpid\tnick\tteamscore\ttotaltime\tplayerrank\tcountrycode\n";
                        num += 48;

                        if (pid == 0)
                        {
                            var players = await _context.Players
                                .Where(p => p.TeamScore > 0)
                                .OrderByDescending(p => p.TeamScore)
                                .Skip(min)
                                .Take(max)
                                .ToListAsync();

                            foreach (var player in players)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t{pos++}\t{player.Id}\t{player.Name}\t{player.TeamScore}\t{player.Time}\t{player.Rank}\t{country}\n";
                                num += pos.ToString().Length + player.Id.ToString().Length + (player.Name?.Length ?? 0) + 
                                      player.TeamScore.ToString().Length + player.Time.ToString().Length + player.Rank.ToString().Length + 3;
                            }
                        }
                        else
                        {
                            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                            if (player != null)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t1\t{pid}\t{player.Name}\t{player.TeamScore}\t{player.Time}\t{player.Rank}\t{country}\n";
                                num += pid.ToString().Length + (player.Name?.Length ?? 0) + player.TeamScore.ToString().Length + 
                                      player.Time.ToString().Length + player.Rank.ToString().Length + 4;
                            }
                        }
                    }
                    else if (id == "combat")
                    {
                        var totalCount = await _context.Players.CountAsync(p => p.Kills > 0);
                        output += $"D\t{totalCount}\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\n";
                        num += totalCount.ToString().Length + 11;
                        output += "H\tn\tpid\tnick\tscore\ttotalkills\ttotaltime\tplayerrank\tcountrycode\n";
                        num += 54;

                        if (pid == 0)
                        {
                            var players = await _context.Players
                                .Where(p => p.Kills > 0)
                                .OrderByDescending(p => p.SkillScore)
                                .Skip(min)
                                .Take(max)
                                .ToListAsync();

                            foreach (var player in players)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t{pos++}\t{player.Id}\t{player.Name}\t{player.SkillScore}\t{player.Kills}\t{player.Time}\t{player.Rank}\t{country}\n";
                                num += pos.ToString().Length + player.Id.ToString().Length + (player.Name?.Length ?? 0) + 
                                      player.SkillScore.ToString().Length + player.Kills.ToString().Length + player.Time.ToString().Length + player.Rank.ToString().Length + 3;
                            }
                        }
                        else
                        {
                            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                            if (player != null)
                            {
                                var country = player.Country?.ToUpper() ?? "";
                                output += $"D\t1\t{pid}\t{player.Name}\t{player.SkillScore}\t{player.Kills}\t{player.Time}\t{player.Rank}\t{country}\n";
                                num += pid.ToString().Length + (player.Name?.Length ?? 0) + player.SkillScore.ToString().Length + 
                                      player.Kills.ToString().Length + player.Time.ToString().Length + player.Rank.ToString().Length + 4;
                            }
                        }
                    }
                }

                var response = output + $"$\t{num}\t$";
                
                // 检查是否需要转置格式
                bool transpose = ResponseFormatService.ShouldTranspose(Request.Query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                if (transpose)
                {
                    // 对于leaderboard，transpose主要影响数据的排列方式
                    // 但由于这是表格数据，我们保持原格式
                }
                
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leaderboard");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }
    }
}