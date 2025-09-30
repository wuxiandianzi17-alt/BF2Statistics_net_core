using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;
using BF2Statistics.Services;

namespace BF2Statistics.Controllers
{
    [Route("ASP/[controller].aspx")]
    public class GetBackendInfoController : Controller
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetBackendInfoController> _logger;

        public GetBackendInfoController(BF2StatisticsContext context, ILogger<GetBackendInfoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Index()
        {
            try
            {
                var output = "H\tver\tnow\n" +
                           "D\t0.1\t" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "\n" +
                           "H\tid\tkit\tname\tdescr\n" +
                           "D\t11\t0\tChsht_protecta\tProtecta shotgun with slugs\n" +
                           "D\t22\t1\tUsrif_g3a3\tH&K G3\n" +
                           "D\t33\t2\tUSSHT_Jackhammer\tJackhammer shotgun\n" +
                           "D\t44\t3\tUsrif_sa80\tSA-80\n" +
                           "D\t55\t4\tUsrif_g36c\tG36C\n" +
                           "D\t66\t5\tRULMG_PKM\tPKM\n" +
                           "D\t77\t6\tUSSNI_M95_Barret\tBarret M82A2 (.50 cal rifle)\n" +
                           "D\t88\t1\tsasrif_fn2000\tFN2000\n" +
                           "D\t99\t2\tsasrif_mp7\tMP-7\n" +
                           "D\t111\t3\tsasrif_g36e\tG36E\n" +
                           "D\t222\t4\tusrif_fnscarl\tFN SCAR - L\n" +
                           "D\t333\t5\tsasrif_mg36\tMG36\n" +
                           "D\t444\t0\teurif_fnp90\tP90\n" +
                           "D\t555\t6\tgbrif_l96a1\tL96A1\n";

                var response = "O\n" + output + "$\t" + (output.Length + 2) + "\t$";
                
                // 检查是否需要transpose格式
                var transpose = ResponseFormatService.ShouldTranspose(Request.Query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                if (transpose)
                {
                    // 对于backend info，transpose主要影响数据的排列方式
                    // 但由于这是静态数据，我们保持原格式
                }
                
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting backend info");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }
    }
}