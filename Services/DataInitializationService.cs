using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;
using BF2Statistics.Data;

namespace BF2Statistics.Services
{
    public class DataInitializationService
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<DataInitializationService> _logger;

        public DataInitializationService(BF2StatisticsContext context, ILogger<DataInitializationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeTestData()
        {
            try
            {
                // 检查是否需要初始化基础数据表
                var needsArmyData = !await _context.Armies.AnyAsync();
                var needsKitsData = !await _context.Kits.AnyAsync();
                var needsVehiclesData = !await _context.Vehicles.AnyAsync();
                var needsWeaponsData = !await _context.Weapons.AnyAsync();
                var needsMapInfoData = !await _context.MapInfos.AnyAsync();
                var needsMapData = !await _context.Maps.AnyAsync();
                var needsAwardData = !await _context.Awards.AnyAsync();
                var needsKillData = !await _context.Kills.AnyAsync();
                
                if (!needsArmyData && !needsKitsData && !needsVehiclesData && !needsWeaponsData && !needsMapInfoData && !needsMapData && !needsAwardData && !needsKillData)
                {
                    _logger.LogInformation("All base data tables already contain data, skipping initialization");
                    return;
                }

                _logger.LogInformation("Initializing missing base data tables...");

                // 只有在Players表为空时才创建测试玩家数据
                if (!await _context.Players.AnyAsync())
                {
                    // 创建测试玩家数据
                    var testPlayers = new[]
                    {
                        new Player
                        {
                            Id = 1,
                            Name = "TestPlayer1",
                            Country = "US",
                            Joined = (int)DateTimeOffset.UtcNow.AddDays(-30).ToUnixTimeSeconds(),
                            LastOnline = (int)DateTimeOffset.UtcNow.AddHours(-2).ToUnixTimeSeconds(),
                            Time = 7200, // 2 hours
                            Rounds = 25,
                            Ip = "192.168.1.100",
                            Score = 15000,
                            CmdScore = 500,
                            SkillScore = 800,
                            TeamScore = 1200,
                            Kills = 150,
                            Deaths = 75,
                            Captures = 20,
                            Defends = 15,
                            DamageAssists = 45,
                            Heals = 30,
                            Revives = 12,
                            Ammos = 25,
                            Repairs = 18,
                            TargetAssists = 8
                        },
                        new Player
                        {
                            Id = 2,
                            Name = "TestPlayer2",
                            Country = "DE",
                            Joined = (int)DateTimeOffset.UtcNow.AddDays(-15).ToUnixTimeSeconds(),
                            LastOnline = (int)DateTimeOffset.UtcNow.AddHours(-1).ToUnixTimeSeconds(),
                            Time = 5400, // 1.5 hours
                            Rounds = 18,
                            Ip = "192.168.1.101",
                            Score = 12000,
                            CmdScore = 300,
                            SkillScore = 600,
                            TeamScore = 900,
                            Kills = 120,
                            Deaths = 60,
                            Captures = 15,
                            Defends = 12,
                            DamageAssists = 35,
                            Heals = 22,
                            Revives = 8,
                            Ammos = 18,
                            Repairs = 14,
                            TargetAssists = 6
                        },
                        new Player
                        {
                            Id = 3,
                            Name = "TestPlayer3",
                            Country = "UK",
                            Joined = (int)DateTimeOffset.UtcNow.AddDays(-7).ToUnixTimeSeconds(),
                            LastOnline = (int)DateTimeOffset.UtcNow.AddMinutes(-30).ToUnixTimeSeconds(),
                            Time = 3600, // 1 hour
                            Rounds = 12,
                            Ip = "192.168.1.102",
                            Score = 8000,
                            CmdScore = 200,
                            SkillScore = 400,
                            TeamScore = 600,
                            Kills = 80,
                            Deaths = 40,
                            Captures = 10,
                            Defends = 8,
                            DamageAssists = 25,
                            Heals = 15,
                            Revives = 5,
                            Ammos = 12,
                            Repairs = 10,
                            TargetAssists = 4
                        }
                    };

                    await _context.Players.AddRangeAsync(testPlayers);

                    // 创建测试奖励数据
                    var testAwards = new[]
                    {
                        new Award { Id = 1, PlayerId = 1, PlayerName = "TestPlayer1", AwardId = 1, Level = 1, Earned = (int)DateTimeOffset.UtcNow.AddDays(-5).ToUnixTimeSeconds(), First = 1 },
                        new Award { Id = 2, PlayerId = 2, PlayerName = "TestPlayer2", AwardId = 2, Level = 1, Earned = (int)DateTimeOffset.UtcNow.AddDays(-3).ToUnixTimeSeconds(), First = 0 },
                        new Award { Id = 3, PlayerId = 1, PlayerName = "TestPlayer1", AwardId = 3, Level = 2, Earned = (int)DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds(), First = 1 }
                    };

                    await _context.Awards.AddRangeAsync(testAwards);

                    // 创建测试军队数据
                    var testArmies = new[]
                    {
                        new Army
                        {
                            Id = 1,
                            Time0 = 3600,
                            Win0 = 15,
                            Loss0 = 10,
                            Score0 = 8000,
                            Best0 = 1200,
                            Worst0 = 200
                        },
                        new Army
                        {
                            Id = 2,
                            Time1 = 2400,
                            Win1 = 8,
                            Loss1 = 7,
                            Score1 = 5000,
                            Best1 = 800,
                            Worst1 = 150
                        },
                        new Army
                        {
                            Id = 3,
                            Time0 = 2700,
                            Win0 = 10,
                            Loss0 = 8,
                            Score0 = 6000,
                            Best0 = 900,
                            Worst0 = 180
                        }
                    };

                    await _context.Armies.AddRangeAsync(testArmies);

                    // 创建测试IP2Nation数据
                    var testIp2Nations = new[]
                    {
                        new Ip2Nation { Ip = unchecked((int)3232235876), Country = "US" }, // 192.168.1.100
                        new Ip2Nation { Ip = unchecked((int)3232235877), Country = "DE" }, // 192.168.1.101
                        new Ip2Nation { Ip = unchecked((int)3232235878), Country = "UK" }  // 192.168.1.102
                    };

                    await _context.Ip2Nations.AddRangeAsync(testIp2Nations);

                    // 创建测试解锁数据
                    var testUnlocks = new[]
                    {
                        new Unlock
                        {
                            PlayerId = 1,
                            UnlockId = 11,
                            State = 1
                        },
                        new Unlock
                        {
                            PlayerId = 2,
                            UnlockId = 22,
                            State = 1
                        }
                    };

                    await _context.Unlocks.AddRangeAsync(testUnlocks);

                    // 创建测试武器数据
                    var testWeapons = new[]
                    {
                        new Weapon
                        {
                            Id = 1,
                            Time0 = 1800,
                            Kills0 = 45,
                            Fired0 = 500,
                            Hit0 = 180
                        },
                        new Weapon
                        {
                            Id = 2,
                            Time1 = 300,
                            Kills1 = 8,
                            Fired1 = 25,
                            Hit1 = 12
                        },
                        new Weapon
                        {
                            Id = 3,
                            Time2 = 1500,
                            Kills2 = 38,
                            Fired2 = 420,
                            Hit2 = 152
                        }
                    };

                    await _context.Weapons.AddRangeAsync(testWeapons);

                    // 创建测试载具数据
                    var testVehicles = new[]
                    {
                        new Vehicle
                        {
                            Id = 1,
                            Time0 = 900,
                            Kills0 = 15,
                            Deaths0 = 3,
                            RK0 = 2
                        },
                        new Vehicle
                        {
                            Id = 2,
                            Time1 = 720,
                            Kills1 = 12,
                            Deaths1 = 2,
                            RK1 = 1
                        }
                    };

                    await _context.Vehicles.AddRangeAsync(testVehicles);

                    // 创建测试兵种数据
                    var testKits = new[]
                    {
                        new Kit
                        {
                            Id = 1,
                            Time0 = 2400,
                            Kills0 = 60,
                            Deaths0 = 20
                        },
                        new Kit
                        {
                            Id = 2,
                            Time1 = 1800,
                            Kills1 = 30,
                            Deaths1 = 15
                        },
                        new Kit
                        {
                            Id = 3,
                            Time0 = 2100,
                            Kills0 = 50,
                            Deaths0 = 18
                        }
                    };

                    await _context.Kits.AddRangeAsync(testKits);

                    // 创建测试地图数据
                    var testMaps = new[]
                    {
                        new Map
                        {
                            Id = 1,
                            Name = "Strike at Karkand",
                            Time = 1800,
                            Wins = 8,
                            Losses = 4,
                            BestScore = 1500,
                            WorstScore = 200
                        },
                        new Map
                        {
                            Id = 2,
                            Name = "Wake Island",
                            Time = 1200,
                            Wins = 5,
                            Losses = 3,
                            BestScore = 1200,
                            WorstScore = 150
                        }
                    };

                    await _context.Maps.AddRangeAsync(testMaps);

                    // 创建测试击杀数据
                    var testKills = new[]
                    {
                        new Kill
                        {
                            Id = 1,
                            Attacker = 1,
                            Victim = 2,
                            Count = 15
                        },
                        new Kill
                        {
                            Id = 2,
                            Attacker = 2,
                            Victim = 3,
                            Count = 12
                        },
                        new Kill
                        {
                            Id = 3,
                            Attacker = 3,
                            Victim = 1,
                            Count = 8
                        },
                        new Kill
                        {
                            Id = 4,
                            Attacker = 1,
                            Victim = 3,
                            Count = 10
                        }
                    };

                    await _context.Kills.AddRangeAsync(testKills);

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Test data initialization completed successfully");
                }
                
                // 独立初始化maps、awards、kills数据（即使Players表不为空）
                if (needsMapData)
                {
                    var testMaps = new[]
                    {
                        new Map
                        {
                            Id = 1,
                            Time = 1800,
                            Wins = 8,
                            Losses = 4,
                            BestScore = 1500,
                            WorstScore = 200
                        },
                        new Map
                        {
                            Id = 2,
                            Time = 1200,
                            Wins = 5,
                            Losses = 3,
                            BestScore = 1200,
                            WorstScore = 150
                        }
                    };
                    
                    await _context.Maps.AddRangeAsync(testMaps);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Maps data initialized successfully");
                }
                
                if (needsAwardData)
                {
                    var testAwards = new[]
                    {
                        new Award { Id = 1, AwardId = 1, Level = 1, Earned = (int)DateTimeOffset.UtcNow.AddDays(-5).ToUnixTimeSeconds(), First = 1 },
                        new Award { Id = 2, AwardId = 2, Level = 1, Earned = (int)DateTimeOffset.UtcNow.AddDays(-3).ToUnixTimeSeconds(), First = 0 },
                        new Award { Id = 3, AwardId = 3, Level = 2, Earned = (int)DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds(), First = 1 }
                    };
                    
                    await _context.Awards.AddRangeAsync(testAwards);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Awards data initialized successfully");
                }
                
                if (needsKillData)
                {
                    var testKills = new[]
                    {
                        new Kill
                        {
                            Id = 1,
                            Attacker = 1,
                            Victim = 2,
                            Count = 15
                        },
                        new Kill
                        {
                            Id = 2,
                            Attacker = 2,
                            Victim = 3,
                            Count = 12
                        },
                        new Kill
                        {
                            Id = 3,
                            Attacker = 3,
                            Victim = 1,
                            Count = 8
                        },
                        new Kill
                        {
                            Id = 4,
                            Attacker = 1,
                            Victim = 3,
                            Count = 10
                        }
                    };
                    
                    await _context.Kills.AddRangeAsync(testKills);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Kills data initialized successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing test data");
                throw;
            }
        }
    }
}