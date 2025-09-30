using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;

namespace BF2Statistics.Controllers
{
    [Route("ASP/selectunlock.aspx")]
    public class SelectUnlockController : Controller
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<SelectUnlockController> _logger;

        public SelectUnlockController(BF2StatisticsContext context, ILogger<SelectUnlockController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] int pid = 0, [FromQuery] int id = 0)
        {
            _logger.LogInformation("SelectUnlock请求: PID={Pid}, UnlockID={Id}", pid, id);
            
            try
            {
                if (pid <= 0 || id <= 0 || !int.TryParse(pid.ToString(), out _) || !int.TryParse(id.ToString(), out _))
                {
                    _logger.LogWarning("SelectUnlock参数无效: PID={Pid}, ID={Id}", pid, id);
                    return Content("Invalid syntax!", "text/plain");
                }

                // 查找或创建解锁记录
                var unlock = await _context.Unlocks.FirstOrDefaultAsync(u => u.PlayerId == pid && u.UnlockId == id);
                if (unlock == null)
                {
                    // 如果不存在，创建新的解锁记录
                    unlock = new Unlock
                    {
                        PlayerId = pid,
                        UnlockId = id,
                        State = 1 // 设置为已选择状态
                    };
                    _context.Unlocks.Add(unlock);
                    _logger.LogInformation("创建新解锁记录: PID={Pid}, UnlockID={Id}", pid, id);
                }
                else
                {
                    // 如果存在，更新状态为已选择
                    unlock.State = 1;
                    _logger.LogInformation("更新解锁状态: PID={Pid}, UnlockID={Id}", pid, id);
                }

                // 获取并更新玩家的解锁统计
                var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                if (player != null)
                {
                    // 重新计算已使用的解锁数量
                    var usedUnlocks = await _context.Unlocks
                        .CountAsync(u => u.PlayerId == pid && u.State == 1);
                    
                    player.UsedUnlocks = (byte)usedUnlocks;
                    
                    // 计算可用解锁数量（基于等级的解锁数量减去已使用的）
                    int earnedUnlocks = GetRankUnlocks(player.Rank);
                    player.AvailableUnlocks = (byte)Math.Max(0, earnedUnlocks - usedUnlocks);
                    
                    _logger.LogInformation("更新玩家解锁统计: PID={Pid}, 已用={Used}, 可用={Available}", 
                        pid, player.UsedUnlocks, player.AvailableUnlocks);
                }

                await _context.SaveChangesAsync();

                // 返回空字符串，与旧版本保持一致
                return Content("", "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理SelectUnlock时发生错误: PID={Pid}, ID={Id}", pid, id);
                return Content("ERROR", "text/plain");
            }
        }

        private int GetRankUnlocks(int rank)
        {
            // 根据等级确定解锁数量（与GetUnlocksInfoController保持一致）
            if (rank >= 9) return 7;      // Master Gunnery Sergeant
            if (rank >= 7) return 6;      // Master Sergeant  
            if (rank >= 6) return 5;      // Gunnery Sergeant
            if (rank >= 5) return 4;      // Staff Sergeant
            if (rank >= 4) return 3;      // Sergeant
            if (rank >= 3) return 2;      // Corporal
            if (rank >= 2) return 1;      // Lance Corporal
            return 0;                     // Private
        }
    }
}