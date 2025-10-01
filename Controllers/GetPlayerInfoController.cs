using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;
using BF2Statistics.Services;
using System.Text;
using System.Linq;

namespace BF2Statistics.Controllers
{
    [Route("ASP/[controller].aspx")]
    public class GetPlayerInfoController(BF2StatisticsContext context, ILogger<GetPlayerInfoController> logger) : Controller
    {
        private readonly BF2StatisticsContext _context = context;
        private readonly ILogger<GetPlayerInfoController> _logger = logger;

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] int pid = 0, [FromQuery] string info = "")
        {
            try
            {
                if (pid <= 0 || string.IsNullOrEmpty(info))
                {
                    return Content(ResponseFormatService.InvalidSyntax(), "text/plain");
                }

                var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                if (player == null)
                {
                    return Content(ResponseFormatService.FormatError("Player not found"), "text/plain");
                }

                // 根据 info 参数的值执行不同的逻辑分支
                if (info == "rank")
                {
                    // --- 处理 rank 请求 ---
                    // 修正：遵循PHP脚本的单行、空格分隔格式
                    var output = $"O H asof D {DateTimeOffset.UtcNow.ToUnixTimeSeconds()} H pid nick rank chng decr D {player.Id} {player.Name} {player.Rank} {player.Chng} {player.Decr}";

                    // 重置chng和decr值
                    player.Chng = 0;
                    player.Decr = 0;
                    await _context.SaveChangesAsync();

                    // 修正：根据空格分隔的格式计算校验和
                    var checksum = output.Replace(" ", string.Empty).Length;
                    var response = output + $" $ {checksum} $";

                    return Content(response, "text/plain");
                }
                
                // 检查是否为完整的统计信息请求
                string requiredKeys = "per*,cmb*,twsc,cpcp,cacp,dfcp,kila,heal,rviv,rsup,rpar,";
                if (info.Contains(requiredKeys))
                {
                    // --- 处理完整的玩家统计信息请求 ---
                    var weapons = await _context.Weapons.FirstOrDefaultAsync(w => w.Id == pid) ?? new Weapon();
                    var vehicles = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == pid) ?? new Vehicle();
                    var kits = await _context.Kits.FirstOrDefaultAsync(k => k.Id == pid) ?? new Kit();
                    var army = await _context.Armies.FirstOrDefaultAsync(a => a.Id == pid) ?? new Army();

                    var responseBuilder = new StringBuilder();
                    responseBuilder.AppendLine("O");
                    responseBuilder.AppendLine("H\tasof");
                    responseBuilder.AppendLine($"D\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");

                    // 构建Header
                    responseBuilder.AppendLine("H\tpid\tnick\tscor\tjond\twins\tloss\tmode0\tmode1\tmode2\ttime\tsmoc\tcmsc\tosaa\tkill\tkila\tdeth\tsuic\tbksk\twdsk\ttvcr\ttopr\tklpm\tdtpm\tospm\tklpr\tdtpr\ttwsc\tcpcp\tcacp\tdfcp\theal\trviv\trsup\trpar\ttgte\tdkas\tdsab\tcdsc\trank\tkick\tbbrs\ttcdr\tban\tlbtl\tvrk\ttsql\ttsqm\ttlwf\tmvks\tvmks\tmvns\tvmrs\tvmns\tvmrs\tfkit\tfmap\tfveh\tfwea\ttnv\ttgm\twtm-0\twtm-1\twtm-2\twtm-3\twtm-4\twtm-5\twtm-6\twtm-7\twtm-8\twkl-0\twkl-1\twkl-2\twkl-3\twkl-4\twkl-5\twkl-6\twkl-7\twkl-8\twdt-0\twdt-1\twdt-2\twdt-3\twdt-4\twdt-5\twdt-6\twdt-7\twdt-8\twac-0\twac-1\twac-2\twac-3\twac-4\twac-5\twac-6\twac-7\twac-8\twkd-0\twkd-1\twkd-2\twkd-3\twkd-4\twkd-5\twkd-6\twkd-7\twkd-8\tvtm-0\tvtm-1\tvtm-2\tvtm-3\tvtm-4\tvtm-5\tvtm-6\tvkl-0\tvkl-1\tvkl-2\tvkl-3\tvkl-4\tvkl-5\tvkl-6\tvdt-0\tvdt-1\tvdt-2\tvdt-3\tvdt-4\tvdt-5\tvdt-6\tvkd-0\tvkd-1\tvkd-2\tvkd-3\tvkd-4\tvkd-5\tvkd-6\tvkr-0\tvkr-1\tvkr-2\tvkr-3\tvkr-4\tvkr-5\tvkr-6\tatm-0\tatm-1\tatm-2\tatm-3\tatm-4\tatm-5\tatm-6\tatm-7\tatm-8\tawn-0\tawn-1\tawn-2\tawn-3\tawn-4\tawn-5\tawn-6\tawn-7\tawn-8\talo-0\talo-1\talo-2\talo-3\talo-4\talo-5\talo-6\talo-7\talo-8\tabr-0\tabr-1\tabr-2\tabr-3\tabr-4\tabr-5\tabr-6\tabr-7\tabr-8\tktm-0\tktm-1\tktm-2\tktm-3\tktm-4\tktm-5\tktm-6\tkkl-0\tkkl-1\tkkl-2\tkkl-3\tkkl-4\tkkl-5\tkkl-6\tkdt-0\tkdt-1\tkdt-2\tkdt-3\tkdt-4\tkdt-5\tkdt-6\tkkd-0\tkkd-1\tkkd-2\tkkd-3\tkkd-4\tkkd-5\tkkd-6");

                    // --- 计算衍生统计数据 ---
                    double timePlayedInMinutes = player.Time > 0 ? player.Time / 60.0 : 0;
                    double killsPerMin = timePlayedInMinutes > 0 ? player.Kills / timePlayedInMinutes : 0;
                    double deathsPerMin = timePlayedInMinutes > 0 ? player.Deaths / timePlayedInMinutes : 0;
                    double scorePerMin = timePlayedInMinutes > 0 ? player.Score / timePlayedInMinutes : 0;
                    double killsPerRound = player.Rounds > 0 ? (double)player.Kills / player.Rounds : 0;
                    double deathsPerRound = player.Rounds > 0 ? (double)player.Deaths / player.Rounds : 0;
                    double overallAccuracy = weapons.Fired > 0 ? (weapons.Hit / (double)weapons.Fired) * 100 : 0;
                    int smoc = player.Rank == 11 ? 1 : 0;

                    var kitTimes = new[] { kits.Time0, kits.Time1, kits.Time2, kits.Time3, kits.Time4, kits.Time5, kits.Time6 };
                    int favKit = kitTimes.Any(t => t > 0) ? Array.IndexOf(kitTimes, kitTimes.Max()) : 0;

                    var vehicleTimes = new[] { vehicles.Time0, vehicles.Time1, vehicles.Time2, vehicles.Time3, vehicles.Time4, vehicles.Time5, vehicles.Time6 };
                    int favVehicle = vehicleTimes.Any(t => t > 0) ? Array.IndexOf(vehicleTimes, vehicleTimes.Max()) : 0;

                    var weaponTimes = new[] { weapons.Time0, weapons.Time1, weapons.Time2, weapons.Time3, weapons.Time4, weapons.Time5, weapons.Time6, weapons.Time7, weapons.Time8 };
                    int favWeapon = weaponTimes.Any(t => t > 0) ? Array.IndexOf(weaponTimes, weaponTimes.Max()) : 0;

                    var playerMaps = await _context.Maps.Where(m => m.Id == pid).ToListAsync();
                    int favMap = playerMaps.Any() ? playerMaps.OrderByDescending(m => m.Time).First().MapId : 0;

                    // --- 构建数据行 (D\t...) ---
                    var data = new List<object>
                    {
                        player.Id, player.Name, player.Score, player.Joined, player.Wins, player.Losses,
                        player.Mode0, player.Mode1, player.Mode2, player.Time, smoc, player.CmdScore,
                        $"{overallAccuracy:F2}", player.Kills, player.Kills, player.Deaths, player.Suicides,
                        player.KillStreak, player.DeathStreak, 0/*tvcr*/, 0/*topr*/,
                        $"{killsPerMin:F2}", $"{deathsPerMin:F2}", $"{scorePerMin:F2}",
                        $"{killsPerRound:F2}", $"{deathsPerRound:F2}",
                        player.TeamScore, player.Captures, player.CaptureAssists, player.Defends,
                        player.Heals, player.Revives, player.Ammos, player.Repairs, player.TargetAssists,
                        player.DriverAssists, player.DriverSpecials, player.CmdScore, player.Rank,
                        player.Kicked, player.RndScore, player.CmdTime, player.Banned, player.LastOnline,
                        0/*vrk*/, player.SqlTime, player.SqmTime, player.LwTime,
                        0/*mvks*/, 0/*vmks*/, 0/*mvns*/, 0/*mvrs*/, 0/*vmns*/, 0/*vmrs*/,
                        favKit, favMap, favVehicle, favWeapon, 0/*tnv*/, 0/*tgm*/,
                    };

                    // 武器统计
                    data.AddRange(new object[] { weapons.Time0, weapons.Time1, weapons.Time2, weapons.Time3, weapons.Time4, weapons.Time5, weapons.Time6, weapons.Time7, weapons.Time8 });
                    data.AddRange(new object[] { weapons.Kills0, weapons.Kills1, weapons.Kills2, weapons.Kills3, weapons.Kills4, weapons.Kills5, weapons.Kills6, weapons.Kills7, weapons.Kills8 });
                    data.AddRange(new object[] { weapons.Deaths0, weapons.Deaths1, weapons.Deaths2, weapons.Deaths3, weapons.Deaths4, weapons.Deaths5, weapons.Deaths6, weapons.Deaths7, weapons.Deaths8 });
                    data.AddRange(new object[] {
                        weapons.Fired0 > 0 ? (weapons.Hit0 / (double)weapons.Fired0) * 100 : 0,
                        weapons.Fired1 > 0 ? (weapons.Hit1 / (double)weapons.Fired1) * 100 : 0,
                        weapons.Fired2 > 0 ? (weapons.Hit2 / (double)weapons.Fired2) * 100 : 0,
                        weapons.Fired3 > 0 ? (weapons.Hit3 / (double)weapons.Fired3) * 100 : 0,
                        weapons.Fired4 > 0 ? (weapons.Hit4 / (double)weapons.Fired4) * 100 : 0,
                        weapons.Fired5 > 0 ? (weapons.Hit5 / (double)weapons.Fired5) * 100 : 0,
                        weapons.Fired6 > 0 ? (weapons.Hit6 / (double)weapons.Fired6) * 100 : 0,
                        weapons.Fired7 > 0 ? (weapons.Hit7 / (double)weapons.Fired7) * 100 : 0,
                        weapons.Fired8 > 0 ? (weapons.Hit8 / (double)weapons.Fired8) * 100 : 0
                    }.Select(d => $"{d:F2}"));
                    data.AddRange(new object[] {
                        GetRatio(weapons.Kills0, weapons.Deaths0), GetRatio(weapons.Kills1, weapons.Deaths1), GetRatio(weapons.Kills2, weapons.Deaths2),
                        GetRatio(weapons.Kills3, weapons.Deaths3), GetRatio(weapons.Kills4, weapons.Deaths4), GetRatio(weapons.Kills5, weapons.Deaths5),
                        GetRatio(weapons.Kills6, weapons.Deaths6), GetRatio(weapons.Kills7, weapons.Deaths7), GetRatio(weapons.Kills8, weapons.Deaths8)
                    });

                    // 载具统计
                    data.AddRange(new object[] { vehicles.Time0, vehicles.Time1, vehicles.Time2, vehicles.Time3, vehicles.Time4, vehicles.Time5, vehicles.Time6 });
                    data.AddRange(new object[] { vehicles.Kills0, vehicles.Kills1, vehicles.Kills2, vehicles.Kills3, vehicles.Kills4, vehicles.Kills5, vehicles.Kills6 });
                    data.AddRange(new object[] { vehicles.Deaths0, vehicles.Deaths1, vehicles.Deaths2, vehicles.Deaths3, vehicles.Deaths4, vehicles.Deaths5, vehicles.Deaths6 });
                    data.AddRange(new object[] {
                        GetRatio(vehicles.Kills0, vehicles.Deaths0), GetRatio(vehicles.Kills1, vehicles.Deaths1), GetRatio(vehicles.Kills2, vehicles.Deaths2),
                        GetRatio(vehicles.Kills3, vehicles.Deaths3), GetRatio(vehicles.Kills4, vehicles.Deaths4), GetRatio(vehicles.Kills5, vehicles.Deaths5),
                        GetRatio(vehicles.Kills6, vehicles.Deaths6)
                    });
                    data.AddRange(new object[] { vehicles.Roadkills0, vehicles.Roadkills1, vehicles.Roadkills2, vehicles.Roadkills3, vehicles.Roadkills4, vehicles.Roadkills5, vehicles.Roadkills6 });

                    // 军队统计
                    data.AddRange(new object[] { army.Time0, army.Time1, army.Time2, army.Time3, army.Time4, army.Time5, army.Time6, army.Time7, army.Time8 });
                    data.AddRange(new object[] { army.Win0, army.Win1, army.Win2, army.Win3, army.Win4, army.Win5, army.Win6, army.Win7, army.Win8 });
                    data.AddRange(new object[] { army.Loss0, army.Loss1, army.Loss2, army.Loss3, army.Loss4, army.Loss5, army.Loss6, army.Loss7, army.Loss8 });
                    data.AddRange(new object[] {
                        GetRatio(army.Win0, army.Loss0), GetRatio(army.Win1, army.Loss1), GetRatio(army.Win2, army.Loss2),
                        GetRatio(army.Win3, army.Loss3), GetRatio(army.Win4, army.Loss4), GetRatio(army.Win5, army.Loss5),
                        GetRatio(army.Win6, army.Loss6), GetRatio(army.Win7, army.Loss7), GetRatio(army.Win8, army.Loss8)
                    });

                    // 兵种统计
                    data.AddRange(new object[] { kits.Time0, kits.Time1, kits.Time2, kits.Time3, kits.Time4, kits.Time5, kits.Time6 });
                    data.AddRange(new object[] { kits.Kills0, kits.Kills1, kits.Kills2, kits.Kills3, kits.Kills4, kits.Kills5, kits.Kills6 });
                    data.AddRange(new object[] { kits.Deaths0, kits.Deaths1, kits.Deaths2, kits.Deaths3, kits.Deaths4, kits.Deaths5, kits.Deaths6 });
                    data.AddRange(new object[] {
                        GetRatio(kits.Kills0, kits.Deaths0), GetRatio(kits.Kills1, kits.Deaths1), GetRatio(kits.Kills2, kits.Deaths2),
                        GetRatio(kits.Kills3, kits.Deaths3), GetRatio(kits.Kills4, kits.Deaths4), GetRatio(kits.Kills5, kits.Deaths5),
                        GetRatio(kits.Kills6, kits.Deaths6)
                    });

                    responseBuilder.AppendLine("D\t" + string.Join("\t", data));

                    var responseString = responseBuilder.ToString();
                    var checksum = responseString.Replace("\t", "").Replace("\n", "").Length;
                    responseBuilder.Append($"$\t{checksum}\t$");

                    return Content(responseBuilder.ToString(), "text/plain");
                }
                else
                {
                    // 如果 info 参数不是 'rank' 也不是完整的统计请求，则返回语法错误
                    return Content(ResponseFormatService.InvalidSyntax(), "text/plain");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting player info");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }

        private static string GetRatio(int val1, int val2)
        {
            if (val2 == 0)
            {
                return $"{val1}:0";
            }
            // 使用浮点数除法以获得更精确的比率
            return $"{val1 / (double)val2:F2}"; // 修正：确保进行浮点数除法
        }
    }
}