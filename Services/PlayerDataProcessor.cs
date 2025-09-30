using BF2Statistics.Data;
using BF2Statistics.Models;
using Microsoft.EntityFrameworkCore;

namespace BF2Statistics.Services
{
    public class PlayerDataProcessor
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<PlayerDataProcessor> _logger;

        public PlayerDataProcessor(BF2StatisticsContext context, ILogger<PlayerDataProcessor> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 处理击杀关系数据
        /// </summary>
        private async Task ProcessKillRelationshipsAsync(Dictionary<string, string> gameSpyData, List<PlayerSnapshotData> players)
        {
            try
            {
                _logger.LogInformation("开始处理击杀关系数据");

                // 创建玩家ID映射
                var playerIdMap = players.ToDictionary(p => p.Pid, p => p);

                // 处理每个玩家对其他玩家的击杀数据
                foreach (var attacker in players)
                {
                    foreach (var victim in players)
                    {
                        if (attacker.Pid == victim.Pid) continue; // 跳过自己

                        // 尝试从GameSpy数据中获取击杀关系数据
                        // 格式可能是 kill_攻击者ID_受害者ID 或类似格式
                        string killKey = $"kill_{attacker.Pid}_{victim.Pid}";
                        if (gameSpyData.TryGetValue(killKey, out string? killCountStr) && 
                            int.TryParse(killCountStr, out int killCount) && killCount > 0)
                        {
                            await UpdateKillRelationshipAsync(attacker.Pid, victim.Pid, killCount);
                        }
                    }

                    // 也可以尝试其他可能的击杀数据格式
                    // 例如：基于武器的击杀数据等
                    await ProcessWeaponKillsAsync(attacker, gameSpyData);
                }

                _logger.LogInformation("击杀关系数据处理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理击杀关系数据时发生错误");
            }
        }

        /// <summary>
        /// 更新击杀关系记录
        /// </summary>
        private async Task UpdateKillRelationshipAsync(int attackerId, int victimId, int killCount)
        {
            try
            {
                var existingKill = await _context.Kills
                    .FirstOrDefaultAsync(k => k.Attacker == attackerId && k.Victim == victimId);

                if (existingKill == null)
                {
                    // 创建新的击杀记录
                    var newKill = new Kill
                    {
                        Attacker = attackerId,
                        Victim = victimId,
                        Count = killCount
                    };

                    _context.Kills.Add(newKill);
                    _logger.LogDebug("创建新击杀记录: 攻击者 {AttackerId} -> 受害者 {VictimId}, 击杀数 {Count}", 
                        attackerId, victimId, killCount);
                }
                else
                {
                    // 更新现有击杀记录
                    existingKill.Count += killCount;
                    _logger.LogDebug("更新击杀记录: 攻击者 {AttackerId} -> 受害者 {VictimId}, 新增击杀数 {Count}, 总计 {Total}", 
                        attackerId, victimId, killCount, existingKill.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新击杀关系记录时发生错误: 攻击者 {AttackerId} -> 受害者 {VictimId}", 
                    attackerId, victimId);
            }
        }

        /// <summary>
        /// 处理武器击杀数据
        /// </summary>
        private async Task ProcessWeaponKillsAsync(PlayerSnapshotData player, Dictionary<string, string> gameSpyData)
        {
            try
            {
                // 获取玩家的武器统计记录
                var weaponStats = await _context.Weapons.FirstOrDefaultAsync(w => w.Id == player.Pid);
                if (weaponStats == null) return;

                bool hasUpdates = false;

                // 处理各种武器的击杀数据
                for (int weaponId = 0; weaponId <= 8; weaponId++)
                {
                    string weaponKillKey = $"wkl{weaponId}_{player.Pid}";
                    if (gameSpyData.TryGetValue(weaponKillKey, out string? weaponKillStr) && 
                        int.TryParse(weaponKillStr, out int weaponKills) && weaponKills > 0)
                    {
                        // 根据武器ID更新对应的击杀数
                        switch (weaponId)
                        {
                            case 0: weaponStats.Kills0 += weaponKills; break;
                            case 1: weaponStats.Kills1 += weaponKills; break;
                            case 2: weaponStats.Kills2 += weaponKills; break;
                            case 3: weaponStats.Kills3 += weaponKills; break;
                            case 4: weaponStats.Kills4 += weaponKills; break;
                            case 5: weaponStats.Kills5 += weaponKills; break;
                            case 6: weaponStats.Kills6 += weaponKills; break;
                            case 7: weaponStats.Kills7 += weaponKills; break;
                            case 8: weaponStats.Kills8 += weaponKills; break;
                        }
                        hasUpdates = true;
                        _logger.LogDebug("更新玩家 {PlayerId} 武器 {WeaponId} 击杀数: +{Kills}", 
                            player.Pid, weaponId, weaponKills);
                    }
                }

                // 处理特殊武器击杀
                var specialWeapons = new Dictionary<string, Action<int>>
                {
                    ["knifekills"] = kills => weaponStats.KnifeKills += kills,
                    ["c4kills"] = kills => weaponStats.C4Kills += kills,
                    ["handgrenadekills"] = kills => weaponStats.HandGrenadeKills += kills,
                    ["claymorekills"] = kills => weaponStats.ClaymoreKills += kills,
                    ["shockpadkills"] = kills => weaponStats.ShockpadKills += kills,
                    ["atminekills"] = kills => weaponStats.AtmineKills += kills,
                    ["tacticalkills"] = kills => weaponStats.TacticalKills += kills,
                    ["grapplinghookkills"] = kills => weaponStats.GrapplinghookKills += kills,
                    ["ziplinekills"] = kills => weaponStats.ZiplineKills += kills
                };

                foreach (var specialWeapon in specialWeapons)
                {
                    string specialKillKey = $"{specialWeapon.Key}_{player.Pid}";
                    if (gameSpyData.TryGetValue(specialKillKey, out string? specialKillStr) && 
                        int.TryParse(specialKillStr, out int specialKills) && specialKills > 0)
                    {
                        specialWeapon.Value(specialKills);
                        hasUpdates = true;
                        _logger.LogDebug("更新玩家 {PlayerId} 特殊武器 {WeaponType} 击杀数: +{Kills}", 
                            player.Pid, specialWeapon.Key, specialKills);
                    }
                }

                if (hasUpdates)
                {
                    _logger.LogDebug("玩家 {PlayerId} 武器击杀数据已更新", player.Pid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理玩家 {PlayerId} 武器击杀数据时发生错误", player.Pid);
            }
        }

        /// <summary>
        /// 处理GameSpy SNAPSHOT数据中的玩家数据
        /// </summary>
        public async Task ProcessSnapshotDataAsync(Dictionary<string, string> gameSpyData)
        {
            try
            {
                _logger.LogInformation("开始处理SNAPSHOT中的玩家数据");

                // 解析玩家数据
                var players = ParsePlayersFromSnapshot(gameSpyData);
                
                foreach (var playerData in players)
                {
                    await ProcessPlayerDataAsync(playerData);
                }

                // 处理击杀关系数据
                await ProcessKillRelationshipsAsync(gameSpyData, players);

                await _context.SaveChangesAsync();
                _logger.LogInformation("玩家数据处理完成，处理了 {Count} 个玩家", players.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理玩家数据时发生错误");
                throw;
            }
        }

        private List<PlayerSnapshotData> ParsePlayersFromSnapshot(Dictionary<string, string> gameSpyData)
        {
            var players = new List<PlayerSnapshotData>();
            
            try
            {
                // 输出所有GameSpy数据键值对用于调试
                _logger.LogInformation("GameSpy数据包含以下键值对:");
                foreach (var kvp in gameSpyData.OrderBy(x => x.Key))
                {
                    _logger.LogInformation("键: '{Key}' = '{Value}'", kvp.Key, kvp.Value);
                }
                
                // 获取玩家数量
                int playerCount = 0;
                if (gameSpyData.TryGetValue("pc", out string? pcValue) && int.TryParse(pcValue, out playerCount))
                {
                    _logger.LogInformation("SNAPSHOT包含 {Count} 个玩家", playerCount);
                }
                else
                {
                    _logger.LogWarning("无法获取玩家数量，pc字段值: {Value}", pcValue);
                    return players;
                }
                
                // 解析每个玩家的数据
                for (int i = 0; i < playerCount; i++)
                {
                    _logger.LogInformation("开始解析玩家 {Index}", i);
                    
                    var player = new PlayerSnapshotData();
                    
                    // 获取玩家昵称 - 尝试多种格式
                    string nameKey = $"name_{i}";
                    if (!gameSpyData.TryGetValue(nameKey, out string? playerName))
                    {
                        nameKey = $"nick_{i}";
                        if (!gameSpyData.TryGetValue(nameKey, out playerName))
                        {
                            // 尝试不带下划线的格式
                            nameKey = $"name{i}";
                            if (!gameSpyData.TryGetValue(nameKey, out playerName))
                            {
                                nameKey = $"nick{i}";
                                gameSpyData.TryGetValue(nameKey, out playerName);
                            }
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(playerName))
                    {
                        player.Name = playerName;
                        _logger.LogInformation("玩家 {Index} 昵称: {Name}", i, playerName);
                    }
                    else
                    {
                        _logger.LogWarning("玩家 {Index} 缺少昵称字段", i);
                        continue;
                    }
                    
                    // 获取玩家ID - 尝试多种格式
                    string pidKey = $"pID_{i}";
                    if (!gameSpyData.TryGetValue(pidKey, out string? pidValue))
                    {
                        pidKey = $"pid_{i}";
                        if (!gameSpyData.TryGetValue(pidKey, out pidValue))
                        {
                            // 尝试不带下划线的格式
                            pidKey = $"pID{i}";
                            if (!gameSpyData.TryGetValue(pidKey, out pidValue))
                            {
                                pidKey = $"pid{i}";
                                gameSpyData.TryGetValue(pidKey, out pidValue);
                            }
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(pidValue) && int.TryParse(pidValue, out int playerId))
                    {
                        player.Pid = playerId;
                        _logger.LogInformation("玩家 {Index} PID: {PID}", i, playerId);
                    }
                    else
                    {
                        _logger.LogWarning("玩家 {Index} 缺少PID字段或解析失败，值: {Value}", i, pidValue);
                        continue;
                    }
                    
                    // 解析所有可能的统计字段 - 使用更健壮的解析方法
                    ParsePlayerStatFieldRobust(gameSpyData, i, "rs", out int score);
                    player.Score = score;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "kills", out int kills);
                    player.Kills = kills;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "deaths", out int deaths);
                    player.Deaths = deaths;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "heals", out int heals);
                    player.Heals = heals;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "revives", out int revives);
                    player.Revives = revives;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "ammos", out int ammos);
                    player.Ammos = ammos;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "repairs", out int repairs);
                    player.Repairs = repairs;
                    
                    // 补充遗漏的字段
                    ParsePlayerStatFieldRobust(gameSpyData, i, "captures", out int captures);
                    player.Captures = captures;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "neutralizes", out int neutralizes);
                    player.Neutralizes = neutralizes;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "captureassists", out int captureAssists);
                    player.CaptureAssists = captureAssists;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "neutralizeassists", out int neutralizeAssists);
                    player.NeutralizeAssists = neutralizeAssists;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "defends", out int defends);
                    player.Defends = defends;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "damageassists", out int damageAssists);
                    player.DamageAssists = damageAssists;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "targetassists", out int targetAssists);
                    player.TargetAssists = targetAssists;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "driverspecials", out int driverSpecials);
                    player.DriverSpecials = driverSpecials;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "driverassists", out int driverAssists);
                    player.DriverAssists = driverAssists;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "passengerassists", out int passengerAssists);
                    player.PassengerAssists = passengerAssists;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "teamkills", out int teamKills);
                    player.TeamKills = teamKills;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "teamdamage", out int teamDamage);
                    player.TeamDamage = teamDamage;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "teamvehicledamage", out int teamVehicleDamage);
                    player.TeamVehicleDamage = teamVehicleDamage;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "suicides", out int suicides);
                    player.Suicides = suicides;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "killstreak", out int killStreak);
                    player.KillStreak = killStreak;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "deathstreak", out int deathStreak);
                    player.DeathStreak = deathStreak;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "rank", out int rank);
                    player.Rank = (byte)Math.Min(rank, 255);
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "cmdtime", out int cmdTime);
                    player.CmdTime = cmdTime;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "sqltime", out int sqlTime);
                    player.SqlTime = sqlTime;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "sqmtime", out int sqmTime);
                    player.SqmTime = sqmTime;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "lwtime", out int lwTime);
                    player.LwTime = lwTime;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "wins", out int wins);
                    player.Wins = wins;
                    
                    ParsePlayerStatFieldRobust(gameSpyData, i, "losses", out int losses);
                    player.Losses = losses;
                    
                    // 解析IP地址 - 尝试多种格式
                    string ipKey = $"ip_{i}";
                    if (!gameSpyData.TryGetValue(ipKey, out string? ipValue))
                    {
                        ipKey = $"ip{i}";
                        gameSpyData.TryGetValue(ipKey, out ipValue);
                    }
                    if (!string.IsNullOrEmpty(ipValue))
                    {
                        player.Ip = ipValue;
                    }
                    
                    // 解析国家代码 - 尝试多种格式
                    string countryKey = $"country_{i}";
                    if (!gameSpyData.TryGetValue(countryKey, out string? countryValue))
                    {
                        countryKey = $"country{i}";
                        gameSpyData.TryGetValue(countryKey, out countryValue);
                    }
                    if (!string.IsNullOrEmpty(countryValue))
                    {
                        player.Country = countryValue;
                    }
                    
                    _logger.LogInformation("玩家 {Index} 解析结果: PID={PID}, Name='{Name}', Score={Score}, Kills={Kills}, Deaths={Deaths}", 
                        i, player.Pid, player.Name, player.Score, player.Kills, player.Deaths);
                    
                    // 验证玩家数据有效性
                    if (player.Pid > 0 && !string.IsNullOrEmpty(player.Name))
                    {
                        players.Add(player);
                        _logger.LogInformation("玩家 {Index} 数据有效，已添加到列表", i);
                    }
                    else
                    {
                        _logger.LogWarning("玩家 {Index} 数据无效，跳过。PID={PID}, Name='{Name}'", i, player.Pid, player.Name);
                    }
                }
                
                _logger.LogInformation("最终解析出 {Count} 个有效玩家", players.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析玩家数据时发生错误");
            }
            
            return players;
        }

        private void ParsePlayerStatField(Dictionary<string, string> gameSpyData, int playerIndex, string fieldName, out int value)
        {
            value = 0;
            string key = $"{fieldName}_{playerIndex}";
            if (gameSpyData.TryGetValue(key, out string? stringValue) && int.TryParse(stringValue, out int parsedValue))
            {
                value = parsedValue;
            }
        }

        private void ParsePlayerStatFieldRobust(Dictionary<string, string> gameSpyData, int playerIndex, string fieldName, out int value)
        {
            value = 0;
            
            // 尝试多种键格式
            string[] keyFormats = {
                $"{fieldName}_{playerIndex}",  // 标准格式: field_0
                $"{fieldName}{playerIndex}",   // 无下划线格式: field0
                $"{fieldName.ToLower()}_{playerIndex}",  // 小写格式: field_0
                $"{fieldName.ToLower()}{playerIndex}",   // 小写无下划线: field0
                $"{fieldName.ToUpper()}_{playerIndex}",  // 大写格式: FIELD_0
                $"{fieldName.ToUpper()}{playerIndex}"    // 大写无下划线: FIELD0
            };
            
            foreach (string key in keyFormats)
            {
                if (gameSpyData.TryGetValue(key, out string? stringValue) && 
                    !string.IsNullOrWhiteSpace(stringValue) && 
                    int.TryParse(stringValue.Trim(), out int parsedValue))
                {
                    value = parsedValue;
                    return;
                }
            }
            
            // 如果所有格式都失败，记录调试信息
            _logger.LogDebug("无法解析玩家 {PlayerIndex} 的字段 '{FieldName}'，尝试的键: {Keys}", 
                playerIndex, fieldName, string.Join(", ", keyFormats));
        }

        private async Task ProcessPlayerDataAsync(PlayerSnapshotData playerData)
        {
            try
            {
                // 查找或创建玩家
                var existingPlayer = await _context.Players
                    .FirstOrDefaultAsync(p => p.Id == playerData.Pid);

                if (existingPlayer == null)
                {
                    // 创建新玩家
                    var newPlayer = new Player
                    {
                        Id = playerData.Pid,
                        Name = playerData.Name.Trim(),
                        Score = playerData.Score,
                        Kills = playerData.Kills,
                        Deaths = playerData.Deaths,
                        Heals = playerData.Heals,
                        Revives = playerData.Revives,
                        Ammos = playerData.Ammos,
                        Repairs = playerData.Repairs,
                        Captures = playerData.Captures,
                        Neutralizes = playerData.Neutralizes,
                        CaptureAssists = playerData.CaptureAssists,
                        NeutralizeAssists = playerData.NeutralizeAssists,
                        Defends = playerData.Defends,
                        DamageAssists = playerData.DamageAssists,
                        TargetAssists = playerData.TargetAssists,
                        DriverSpecials = playerData.DriverSpecials,
                        DriverAssists = playerData.DriverAssists,
                        PassengerAssists = playerData.PassengerAssists,
                        TeamKills = playerData.TeamKills,
                        TeamDamage = playerData.TeamDamage,
                        TeamVehicleDamage = playerData.TeamVehicleDamage,
                        Suicides = playerData.Suicides,
                        KillStreak = playerData.KillStreak,
                        DeathStreak = playerData.DeathStreak,
                        Rank = playerData.Rank,
                        CmdTime = playerData.CmdTime,
                        SqlTime = playerData.SqlTime,
                        SqmTime = playerData.SqmTime,
                        LwTime = playerData.LwTime,
                        Wins = playerData.Wins,
                        Losses = playerData.Losses,
                        Ip = playerData.Ip,
                        Country = playerData.Country,
                        Rounds = 1,
                        Time = 1800, // 假设每局30分钟
                        LastOnline = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        Joined = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    };

                    _context.Players.Add(newPlayer);
                    _logger.LogInformation("创建新玩家: {Name} (PID: {Pid})", playerData.Name, playerData.Pid);
                    
                    // 先保存玩家数据，确保外键约束满足
                    await _context.SaveChangesAsync();
                    
                    // 然后创建所有相关的初始统计数据
                    await CreateInitialPlayerStatsAsync(playerData.Pid);
                }
                else
                {
                    // 更新现有玩家
                    existingPlayer.Name = playerData.Name.Trim();
                    existingPlayer.Score += playerData.Score;
                    existingPlayer.Kills += playerData.Kills;
                    existingPlayer.Deaths += playerData.Deaths;
                    existingPlayer.Heals += playerData.Heals;
                    existingPlayer.Revives += playerData.Revives;
                    existingPlayer.Ammos += playerData.Ammos;
                    existingPlayer.Repairs += playerData.Repairs;
                    existingPlayer.Captures += playerData.Captures;
                    existingPlayer.Neutralizes += playerData.Neutralizes;
                    existingPlayer.CaptureAssists += playerData.CaptureAssists;
                    existingPlayer.NeutralizeAssists += playerData.NeutralizeAssists;
                    existingPlayer.Defends += playerData.Defends;
                    existingPlayer.DamageAssists += playerData.DamageAssists;
                    existingPlayer.TargetAssists += playerData.TargetAssists;
                    existingPlayer.DriverSpecials += playerData.DriverSpecials;
                    existingPlayer.DriverAssists += playerData.DriverAssists;
                    existingPlayer.PassengerAssists += playerData.PassengerAssists;
                    existingPlayer.TeamKills += playerData.TeamKills;
                    existingPlayer.TeamDamage += playerData.TeamDamage;
                    existingPlayer.TeamVehicleDamage += playerData.TeamVehicleDamage;
                    existingPlayer.Suicides += playerData.Suicides;
                    
                    // 对于最大值字段，取较大值
                    existingPlayer.KillStreak = Math.Max(existingPlayer.KillStreak, playerData.KillStreak);
                    existingPlayer.DeathStreak = Math.Max(existingPlayer.DeathStreak, playerData.DeathStreak);
                    
                    // 更新等级（取较高值）
                    existingPlayer.Rank = Math.Max(existingPlayer.Rank, playerData.Rank);
                    
                    // 累加时间统计
                    existingPlayer.CmdTime += playerData.CmdTime;
                    existingPlayer.SqlTime += playerData.SqlTime;
                    existingPlayer.SqmTime += playerData.SqmTime;
                    existingPlayer.LwTime += playerData.LwTime;
                    
                    // 累加胜负统计
                    existingPlayer.Wins += playerData.Wins;
                    existingPlayer.Losses += playerData.Losses;
                    
                    // 更新IP和国家（使用最新值）
                    if (!string.IsNullOrEmpty(playerData.Ip))
                        existingPlayer.Ip = playerData.Ip;
                    if (!string.IsNullOrEmpty(playerData.Country))
                        existingPlayer.Country = playerData.Country;
                    
                    existingPlayer.Rounds += 1;
                    existingPlayer.Time += 1800; // 假设每局30分钟
                    existingPlayer.LastOnline = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    // 更新可用解锁数
                    await UpdatePlayerUnlocksAsync(existingPlayer);
                    
                    _logger.LogInformation("更新玩家: {Name} (PID: {Pid})", playerData.Name, playerData.Pid);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("玩家数据处理完成: {Name}", playerData.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理玩家数据时发生错误");
                throw;
            }
        }

        private async Task CreateInitialPlayerStatsAsync(int playerId)
        {
            _logger.LogInformation("为新玩家 {PlayerId} 创建所有初始统计数据", playerId);

            // 使用事务确保所有操作的原子性
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. 创建解锁数据
                if (!await _context.Unlocks.AnyAsync(u => u.PlayerId == playerId))
                {
                    var unlocks = new List<Unlock>();
                    // BF2的基础解锁ID是 11, 22, 33, 44, 55, 66, 77
                    for (int i = 1; i <= 7; i++)
                    {
                        unlocks.Add(new Unlock { PlayerId = playerId, UnlockId = i * 11, State = 0 });
                    }
                    await _context.Unlocks.AddRangeAsync(unlocks);
                    _logger.LogInformation("为玩家 {PlayerId} 创建了 {Count} 个解锁项", playerId, unlocks.Count);
                }

                // 2. 创建兵种、武器、载具、军队的初始记录
                if (!await _context.Kits.AnyAsync(k => k.Id == playerId))
                    _context.Kits.Add(new Kit { Id = playerId });

                if (!await _context.Weapons.AnyAsync(w => w.Id == playerId))
                    _context.Weapons.Add(new Weapon { Id = playerId });

                if (!await _context.Vehicles.AnyAsync(v => v.Id == playerId))
                    _context.Vehicles.Add(new Vehicle { Id = playerId });

                if (!await _context.Armies.AnyAsync(a => a.Id == playerId))
                    _context.Armies.Add(new Army { Id = playerId });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("为玩家 {PlayerId} 成功创建了所有初始统计表。", playerId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "为玩家 {PlayerId} 创建初始统计数据时失败，事务已回滚。", playerId);
            }
        }

        private async Task UpdatePlayerUnlocksAsync(Player player)
        {
            // 根据分数计算可获得的解锁数
            int earnedUnlocks = 0;
            if (player.Score >= 50000) earnedUnlocks = 7;
            else if (player.Score >= 20000) earnedUnlocks = 6;
            else if (player.Score >= 8000) earnedUnlocks = 5;
            else if (player.Score >= 5000) earnedUnlocks = 4;
            else if (player.Score >= 2500) earnedUnlocks = 3;
            else if (player.Score >= 800) earnedUnlocks = 2;
            else if (player.Score >= 500) earnedUnlocks = 1;

            // 计算已使用的解锁数
            var usedUnlocks = await _context.Unlocks
                .CountAsync(u => u.PlayerId == player.Id && u.State == 1);

            // 更新玩家解锁统计
            player.AvailableUnlocks = (byte)Math.Max(0, earnedUnlocks - usedUnlocks);
            player.UsedUnlocks = (byte)usedUnlocks;

            _logger.LogInformation("更新玩家 {Name} 解锁统计: 分数={Score}, 可用解锁={Available}, 已用解锁={Used}", 
                player.Name, player.Score, player.AvailableUnlocks, player.UsedUnlocks);
        }
    }

    public class PlayerSnapshotData
    {
        public int Pid { get; set; }
        public string Name { get; set; } = "";
        public int Score { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Heals { get; set; }
        public int Revives { get; set; }
        public int Ammos { get; set; }
        public int Repairs { get; set; }
        
        // 补充的字段
        public int Captures { get; set; }
        public int Neutralizes { get; set; }
        public int CaptureAssists { get; set; }
        public int NeutralizeAssists { get; set; }
        public int Defends { get; set; }
        public int DamageAssists { get; set; }
        public int TargetAssists { get; set; }
        public int DriverSpecials { get; set; }
        public int DriverAssists { get; set; }
        public int PassengerAssists { get; set; }
        public int TeamKills { get; set; }
        public int TeamDamage { get; set; }
        public int TeamVehicleDamage { get; set; }
        public int Suicides { get; set; }
        public int KillStreak { get; set; }
        public int DeathStreak { get; set; }
        public byte Rank { get; set; }
        public int CmdTime { get; set; }
        public int SqlTime { get; set; }
        public int SqmTime { get; set; }
        public int LwTime { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Ip { get; set; } = "";
        public string Country { get; set; } = "xx";
    }
}