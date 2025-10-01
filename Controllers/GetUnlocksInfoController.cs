﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;
using BF2Statistics.Services;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace BF2Statistics.Controllers
{
    [Route("ASP/[controller].aspx")]
    public class GetUnlocksInfoController : Controller
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<GetUnlocksInfoController> _logger;
        private readonly IConfiguration _configuration;
        private readonly bool _allUnlocks; // 对应PHP版本的 $game_unlocks = 1

        public GetUnlocksInfoController(BF2StatisticsContext context, ILogger<GetUnlocksInfoController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _allUnlocks = _configuration.GetValue<bool>("GameSettings:AllUnlocks", true);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] int pid = 0)
        {
            try
            {
                if (pid <= 0 || !int.TryParse(pid.ToString(), out _))
                {
                    return Content(ResponseFormatService.InvalidSyntax(), "text/plain");
                }

                string nick;
                string unlockData = "";
                int earned = 0;

                // 检查是否启用AllUnlocks功能（对应PHP版本的game_unlocks = 1）
                if (_allUnlocks)
                {
                    // AllUnlocks模式：所有解锁都设为's'状态
                    nick = "All_Unlocks"; // 与PHP脚本保持一致
                    
                    // 基础解锁 (11, 22, 33, 44, 55, 66, 77)
                    for (int i = 11; i < 100; i += 11)
                    {
                        unlockData += $"D\t{i}\ts\n";
                    }
                    
                    // 特殊解锁 (88, 99, 111, 222, 333, 444, 555)
                    var specialUnlocks = new[] { 88, 99, 111, 222, 333, 444, 555 };
                    foreach (var unlock in specialUnlocks)
                    {
                        unlockData += $"D\t{unlock}\ts\n";
                    }
                }
                else
                {
                    // 正常模式：检查玩家数据和解锁状态
                    var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == pid);
                    if (player == null)
                    {
                        return Content(ResponseFormatService.FormatError("Player not found"), "text/plain");
                    }

                    nick = player.Name;
                    var score = player.Score;
                    var used = player.UsedUnlocks;

                    // 计算可用解锁数
                    var unlocksCount = await _context.Unlocks
                        .Where(u => u.PlayerId == pid && u.State == 1)
                        .CountAsync();

                    if (unlocksCount < 7)
                    {
                        if (score >= 50000) earned = 7;
                        else if (score >= 20000) earned = 6;
                        else if (score >= 8000) earned = 5;
                        else if (score >= 5000) earned = 4;
                        else if (score >= 2500) earned = 3;
                        else if (score >= 800) earned = 2;
                        else if (score >= 500) earned = 1;
                        earned -= used;
                    }

                    // 更新玩家可用解锁数
                    player.AvailableUnlocks = (byte)earned;
                    await _context.SaveChangesAsync();

                    // 获取基础解锁
                    var unlocks = await _context.Unlocks
                        .Where(u => u.PlayerId == pid && u.UnlockId < 78)
                        .ToListAsync();

                    foreach (var unlock in unlocks)
                    {
                        string state = unlock.State == 1 ? "s" : "n";
                        unlockData += $"D\t{unlock.UnlockId}\t{state}\n";
                    }

                    // 检查特殊解锁
                    unlockData += await CheckUnlock(88, 22, pid);
                    unlockData += await CheckUnlock(99, 33, pid);
                    unlockData += await CheckUnlock(111, 44, pid);
                    unlockData += await CheckUnlock(222, 55, pid);
                    unlockData += await CheckUnlock(333, 66, pid);
                    unlockData += await CheckUnlock(444, 11, pid);
                    unlockData += await CheckUnlock(555, 77, pid);
                }

                // 构建响应头
                var output = "O\n" +
                           $"H\tpid\tnick\tasof\n" +
                           $"D\t{pid}\t{nick}\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\n" +
                           $"H\tenlisted\tofficer\n" +
                           $"D\t{earned}\t0\n" +
                           "H\tid\tstate\n";

                // 计算响应长度（不包括制表符和换行符）
                // 遵循PHP脚本的校验和计算逻辑
                var checksumContent = output + unlockData;
                var totalLength = checksumContent.Replace("\t", "").Replace("\n", "").Length;

                var response = checksumContent + $"$\t{totalLength}\t$";
                
                return Content(response, "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unlocks info");
                return Content(ResponseFormatService.FormatError("Internal Server Error"), "text/plain");
            }
        }

        private async Task<string> CheckUnlock(int want, int need, int pid)
        {
            var hasNeeded = await _context.Unlocks
                .AnyAsync(u => u.PlayerId == pid && u.State == 1 && u.UnlockId == need);

            if (hasNeeded)
            {
                var unlock = await _context.Unlocks
                    .FirstOrDefaultAsync(u => u.PlayerId == pid && u.UnlockId == want);
                
                if (unlock != null)
                {
                    string state = unlock.State == 1 ? "s" : "n";
                    return $"D\t{unlock.UnlockId}\t{state}\n";
                }
            }
            
            return $"D\t{want}\tn\n";
        }

        private void CheckSpecialUnlocks(int pid, StringBuilder response)
        {
            // 模拟原版PHP的checkUnlock函数逻辑
            CheckUnlockDependency(pid, 88, 22, response);
            CheckUnlockDependency(pid, 99, 33, response);
            CheckUnlockDependency(pid, 111, 44, response);
            CheckUnlockDependency(pid, 222, 55, response);
            CheckUnlockDependency(pid, 333, 66, response);
            CheckUnlockDependency(pid, 444, 11, response);
            CheckUnlockDependency(pid, 555, 77, response);
        }

        private void CheckUnlockDependency(int pid, int want, int need, StringBuilder response)
        {
            // 检查是否需要解锁依赖
            var hasNeeded = _context.Unlocks.Any(u => u.PlayerId == pid && u.State == 1 && u.UnlockId == need);
            if (hasNeeded)
            {
                var wantedUnlock = _context.Unlocks.FirstOrDefault(u => u.PlayerId == pid && u.UnlockId == want);
                if (wantedUnlock != null)
                {
                    string state = wantedUnlock.State == 1 ? "s" : "n";
                    response.AppendLine($"D\t{wantedUnlock.UnlockId}\t{state}");
                }
                else
                {
                    response.AppendLine($"D\t{want}\tn");
                }
            }
            else
            {
                response.AppendLine($"D\t{want}\tn");
            }
        }
    }
}