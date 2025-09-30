using Microsoft.EntityFrameworkCore;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Services
{
    public class SmocGenCheckService
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<SmocGenCheckService> _logger;

        public SmocGenCheckService(BF2StatisticsContext context, ILogger<SmocGenCheckService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 检查并更新SMOC (Sergeant Major Of The Corps) - 每月第一天执行
        /// </summary>
        public async Task SmocCheckAsync()
        {
            try
            {
                // 只在每月第一天执行
                if (DateTime.Now.Day != 1)
                    return;

                _logger.LogInformation("执行SMOC检查...");

                // 获取所有rank=10的玩家，按分数排序
                var players = await _context.Players
                    .Where(p => p.Rank == 10)
                    .OrderByDescending(p => p.Score)
                    .Select(p => new { p.Id, p.Score })
                    .ToListAsync();

                if (!players.Any())
                {
                    _logger.LogInformation("没有找到rank=10的玩家");
                    return;
                }

                var topPlayerId = players.First().Id;

                // 检查是否已有SMOC奖章持有者
                var currentSmoc = await _context.Awards
                    .FirstOrDefaultAsync(a => a.AwardId == 6666666);

                if (currentSmoc != null)
                {
                    // 如果不是同一个玩家，则更换SMOC
                    if (currentSmoc.Id != topPlayerId)
                    {
                        // 移除旧的SMOC奖章和等级
                        _context.Awards.Remove(currentSmoc);
                        
                        var oldPlayer = await _context.Players.FindAsync(currentSmoc.Id);
                        if (oldPlayer != null)
                        {
                            oldPlayer.Rank = 10;
                        }

                        // 授予新的SMOC奖章和等级
                        var newSmocAward = new Award
                        {
                            Id = topPlayerId,
                            AwardId = 6666666
                        };
                        _context.Awards.Add(newSmocAward);

                        var newPlayer = await _context.Players.FindAsync(topPlayerId);
                        if (newPlayer != null)
                        {
                            newPlayer.Rank = 11;
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"SMOC从玩家{currentSmoc.Id}转移到玩家{topPlayerId}");
                    }
                }
                else
                {
                    // 没有现有SMOC，直接授予
                    var newSmocAward = new Award
                    {
                        Id = topPlayerId,
                        AwardId = 6666666
                    };
                    _context.Awards.Add(newSmocAward);

                    var newPlayer = await _context.Players.FindAsync(topPlayerId);
                    if (newPlayer != null)
                    {
                        newPlayer.Rank = 11;
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"授予玩家{topPlayerId} SMOC奖章");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMOC检查过程中发生错误");
            }
        }

        /// <summary>
        /// 检查并更新GEN (General) - 每月第一天执行
        /// </summary>
        public async Task GenCheckAsync()
        {
            try
            {
                // 只在每月第一天执行
                if (DateTime.Now.Day != 1)
                    return;

                _logger.LogInformation("执行GEN检查...");

                // 获取所有rank=20的玩家，按分数排序
                var players = await _context.Players
                    .Where(p => p.Rank == 20)
                    .OrderByDescending(p => p.Score)
                    .Select(p => new { p.Id, p.Score })
                    .ToListAsync();

                if (!players.Any())
                {
                    _logger.LogInformation("没有找到rank=20的玩家");
                    return;
                }

                var topPlayerId = players.First().Id;

                // 检查是否已有GEN奖章持有者
                var currentGen = await _context.Awards
                    .FirstOrDefaultAsync(a => a.AwardId == 6666667);

                if (currentGen != null)
                {
                    // 如果不是同一个玩家，则更换GEN
                    if (currentGen.Id != topPlayerId)
                    {
                        // 移除旧的GEN奖章和等级
                        _context.Awards.Remove(currentGen);
                        
                        var oldPlayer = await _context.Players.FindAsync(currentGen.Id);
                        if (oldPlayer != null)
                        {
                            oldPlayer.Rank = 20;
                        }

                        // 授予新的GEN奖章和等级
                        var newGenAward = new Award
                        {
                            Id = topPlayerId,
                            AwardId = 6666667
                        };
                        _context.Awards.Add(newGenAward);

                        var newPlayer = await _context.Players.FindAsync(topPlayerId);
                        if (newPlayer != null)
                        {
                            newPlayer.Rank = 21;
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"GEN从玩家{currentGen.Id}转移到玩家{topPlayerId}");
                    }
                }
                else
                {
                    // 没有现有GEN，直接授予
                    var newGenAward = new Award
                    {
                        Id = topPlayerId,
                        AwardId = 6666667
                    };
                    _context.Awards.Add(newGenAward);

                    var newPlayer = await _context.Players.FindAsync(topPlayerId);
                    if (newPlayer != null)
                    {
                        newPlayer.Rank = 21;
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"授予玩家{topPlayerId} GEN奖章");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GEN检查过程中发生错误");
            }
        }

        /// <summary>
        /// 执行SMOC和GEN检查
        /// </summary>
        public async Task PerformMonthlyChecksAsync()
        {
            await SmocCheckAsync();
            await GenCheckAsync();
        }
    }
}