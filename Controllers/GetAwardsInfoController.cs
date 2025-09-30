using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;
using BF2Statistics.Services;

namespace BF2Statistics.Controllers
{
    [Route("ASP/[controller].aspx")]
    public class GetAwardsInfoController : Controller
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetAwardsInfoController> _logger;

        public GetAwardsInfoController(BF2StatisticsContext context, ILogger<GetAwardsInfoController> logger)
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

                var output = "O\n" +
                           $"H\tpid\tasof\n" +
                           $"D\t{pid}\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\n" +
                           "H\taward\tlevel\twhen\tfirst\n";

                var awards = await _context.Awards.Where(a => a.Id == pid).OrderBy(a => a.Id).ToListAsync();
                
                int count = 0;
                foreach (var award in awards)
                {
                    int first = 0;
                    if (award.AwardId > 2000000 && award.AwardId < 3000000) // medals
                    {
                        first = award.First;
                    }
                    
                    output += $"D\t{award.AwardId}\t{award.Level}\t{award.Earned}\t{first}\n";
                    count += award.AwardId.ToString().Length + award.Level.ToString().Length + 
                            award.Earned.ToString().Length + first.ToString().Length;
                }

                var num = count + 50;
                var response = output + $"$\t{num}\t$";
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting awards info");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }
    }
}