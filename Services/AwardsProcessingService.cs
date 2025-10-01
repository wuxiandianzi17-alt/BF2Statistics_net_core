using BF2Statistics.Data;
using BF2Statistics.Models;
using Microsoft.EntityFrameworkCore;

namespace BF2Statistics.Services
{
    public class AwardsProcessingService
    {
        private readonly BF2StatisticsContext _context;
        private readonly ILogger<AwardsProcessingService> _logger;

        public AwardsProcessingService(BF2StatisticsContext context, ILogger<AwardsProcessingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 处理奖章和徽章，基于游戏数据
        /// </summary>
        public async Task ProcessAwardsAsync(Dictionary<string, string> data, int playerId, string version = "bf2")
        {
            try
            {
                var awards = new List<(int AwardId, int Count)>();

                // BF2 基础奖章和徽章
                ProcessBF2Badges(data, playerId.ToString(), awards);
                ProcessBF2Ribbons(data, playerId.ToString(), awards);
                ProcessBF2Medals(data, playerId.ToString(), awards);

                // 如果是SF版本，处理SF特有奖章
                if (version == "xpack")
                {
                    ProcessSFBadges(data, playerId.ToString(), awards);
                    ProcessSFRibbons(data, playerId.ToString(), awards);
                    ProcessSFMedals(data, playerId.ToString(), awards);
                }

                // 保存奖章到数据库
                await SaveAwardsAsync(playerId, awards);

                _logger.LogInformation($"为玩家 {playerId} 处理了 {awards.Count / 2} 个奖章");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理玩家 {playerId} 的奖章时发生错误");
            }
        }

        private void ProcessBF2Badges(Dictionary<string, string> data, string x, List<(int, int)> awards)
        {
            // BF2 徽章处理 - 支持索引格式和数组格式
            var badges = new Dictionary<string, int>
            {
                { $"kcb_{x}", 1031406 },   // Knife Combat Badge
                { $"pcb_{x}", 1031619 },   // Pistol Combat Badge
                { $"Acb_{x}", 1031119 },   // Assault Combat Badge
                { $"Atcb_{x}", 1031120 },  // Anti-Tank Combat Badge
                { $"Sncb_{x}", 1031109 },  // Sniper Combat Badge
                { $"Socb_{x}", 1031115 },  // Support Combat Badge
                { $"Sucb_{x}", 1031121 },  // Supply Combat Badge
                { $"Ecb_{x}", 1031105 },   // Engineer Combat Badge
                { $"Mcb_{x}", 1031113 },   // Medic Combat Badge
                { $"Eob_{x}", 1032415 },   // Explosive Ordnance Badge
                { $"Fab_{x}", 1190601 },   // First Aid Badge
                { $"Eb_{x}", 1190507 },    // Engineer Badge
                { $"Rb_{x}", 1191819 },    // Resupply Badge
                { $"Cb_{x}", 1190304 },    // Command Badge
                { $"Ab_{x}", 1220118 },    // Armor Badge
                { $"Tb_{x}", 1222016 },    // Transport Badge
                { $"Hb_{x}", 1220803 },    // Helicopter Badge
                { $"Avb_{x}", 1220122 },   // Aviation Badge
                { $"adb_{x}", 1220104 },   // Air Defense Badge
                { $"Swb_{x}", 1031923 }    // Special Weapons Badge
            };

            // 数组格式的字段名（不带索引）
            var arrayBadges = new Dictionary<string, int>
            {
                { "kcb_", 1031406 },   // Knife Combat Badge
                { "pcb_", 1031619 },   // Pistol Combat Badge
                { "Acb_", 1031119 },   // Assault Combat Badge
                { "Atcb_", 1031120 },  // Anti-Tank Combat Badge
                { "Sncb_", 1031109 },  // Sniper Combat Badge
                { "Socb_", 1031115 },  // Support Combat Badge
                { "Sucb_", 1031121 },  // Supply Combat Badge
                { "Ecb_", 1031105 },   // Engineer Combat Badge
                { "Mcb_", 1031113 },   // Medic Combat Badge
                { "Eob_", 1032415 },   // Explosive Ordnance Badge
                { "Fab_", 1190601 },   // First Aid Badge
                { "Eb_", 1190507 },    // Engineer Badge
                { "Rb_", 1191819 },    // Resupply Badge
                { "Cb_", 1190304 },    // Command Badge
                { "Ab_", 1220118 },    // Armor Badge
                { "Tb_", 1222016 },    // Transport Badge
                { "Hb_", 1220803 },    // Helicopter Badge
                { "Avb_", 1220122 },   // Aviation Badge
                { "adb_", 1220104 },   // Air Defense Badge
                { "Swb_", 1031923 }    // Special Weapons Badge
            };

            // 首先尝试索引格式
            foreach (var badge in badges)
            {
                if (data.ContainsKey(badge.Key) && int.TryParse(data[badge.Key], out int count) && count > 0)
                {
                    awards.Add((badge.Value, count));
                }
            }

            // 如果没有找到索引格式的数据，尝试数组格式
            if (!awards.Any())
            {
                int playerIndex = int.Parse(x);
                foreach (var badge in arrayBadges)
                {
                    if (data.ContainsKey(badge.Key))
                    {
                        var values = data[badge.Key].Split('\t');
                        if (playerIndex < values.Length && int.TryParse(values[playerIndex], out int count) && count > 0)
                        {
                            awards.Add((badge.Value, count));
                        }
                    }
                }
            }
        }

        private void ProcessBF2Ribbons(Dictionary<string, string> data, string x, List<(int, int)> awards)
        {
            // BF2 丝带处理 - 支持索引格式和数组格式
            var ribbons = new Dictionary<string, int>
            {
                { $"Car_{x}", 3240301 },   // Combat Action Ribbon
                { $"Mur_{x}", 3211305 },   // Meritorious Unit Ribbon
                { $"Ior_{x}", 3150914 },   // Infantry Officer Ribbon
                { $"Sor_{x}", 3151920 },   // Staff Officer Ribbon
                { $"Dsr_{x}", 3190409 },   // Distinguished Service Ribbon
                { $"Wcr_{x}", 3242303 },   // Weapons Commendation Ribbon
                { $"Vur_{x}", 3212201 },   // Valorous Unit Ribbon
                { $"Lmr_{x}", 3241213 },   // Legion of Merit Ribbon
                { $"Csr_{x}", 3190318 },   // Combat Service Ribbon
                { $"Msr_{x}", 3131318 },   // Meritorious Service Ribbon
                { $"Gcr_{x}", 3070318 },   // Good Conduct Ribbon
                { $"Acr_{x}", 3010318 },   // Achievement Ribbon
                { $"Ccr_{x}", 3030318 },   // Commendation Ribbon
                { $"Hsr_{x}", 3081318 },   // Heroic Service Ribbon
                { $"Dhr_{x}", 3040818 },   // Distinguished Honor Ribbon
                { $"Phr_{x}", 3161808 },   // Purple Heart Ribbon
                { $"Esr_{x}", 3051318 },   // Exceptional Service Ribbon
                { $"Osr_{x}", 3151318 }    // Outstanding Service Ribbon
            };

            // 数组格式的字段名（不带索引）
            var arrayRibbons = new Dictionary<string, int>
            {
                { "Car_", 3240301 },   // Combat Action Ribbon
                { "Mur_", 3211305 },   // Meritorious Unit Ribbon
                { "Ior_", 3150914 },   // Infantry Officer Ribbon
                { "Sor_", 3151920 },   // Staff Officer Ribbon
                { "Dsr_", 3190409 },   // Distinguished Service Ribbon
                { "Wcr_", 3242303 },   // Weapons Commendation Ribbon
                { "Vur_", 3212201 },   // Valorous Unit Ribbon
                { "Lmr_", 3241213 },   // Legion of Merit Ribbon
                { "Csr_", 3190318 },   // Combat Service Ribbon
                { "Msr_", 3131318 },   // Meritorious Service Ribbon
                { "Gcr_", 3070318 },   // Good Conduct Ribbon
                { "Acr_", 3010318 },   // Achievement Ribbon
                { "Ccr_", 3030318 },   // Commendation Ribbon
                { "Hsr_", 3081318 },   // Heroic Service Ribbon
                { "Dhr_", 3040818 },   // Distinguished Honor Ribbon
                { "Phr_", 3161808 },   // Purple Heart Ribbon
                { "Esr_", 3051318 },   // Exceptional Service Ribbon
                { "Osr_", 3151318 }    // Outstanding Service Ribbon
            };

            // 首先尝试索引格式
            foreach (var ribbon in ribbons)
            {
                if (data.ContainsKey(ribbon.Key) && int.TryParse(data[ribbon.Key], out int count) && count > 0)
                {
                    awards.Add((ribbon.Value, count));
                }
            }

            // 如果没有找到索引格式的数据，尝试数组格式
            if (!awards.Any())
            {
                int playerIndex = int.Parse(x);
                foreach (var ribbon in arrayRibbons)
                {
                    if (data.ContainsKey(ribbon.Key))
                    {
                        var values = data[ribbon.Key].Split('\t');
                        if (playerIndex < values.Length && int.TryParse(values[playerIndex], out int count) && count > 0)
                        {
                            awards.Add((ribbon.Value, count));
                        }
                    }
                }
            }
        }

        private void ProcessBF2Medals(Dictionary<string, string> data, string x, List<(int, int)> awards)
        {
            // BF2 奖章处理
            var medals = new Dictionary<string, int>
            {
                { $"erg_{x}", 2051907 },   // Expert Rifleman Badge Gold
                { $"ers_{x}", 2051919 },   // Expert Rifleman Badge Silver
                { $"erb_{x}", 2051902 },   // Expert Rifleman Badge Bronze
                { $"ph_{x}", 2191608 },    // Purple Heart
                { $"Msm_{x}", 2191319 },   // Meritorious Service Medal
                { $"Cam_{x}", 2190303 },   // Combat Action Medal
                { $"Acm_{x}", 2190309 },   // Air Combat Medal
                { $"Arm_{x}", 2190318 },   // Armored Service Medal
                { $"Hcm_{x}", 2190308 },   // Helicopter Combat Medal
                { $"gcm_{x}", 2190703 },   // Good Conduct Medal
                { $"Cim_{x}", 2020903 },   // Combat Infantry Medal
                { $"Mim_{x}", 2020913 },   // Marksman Infantry Medal
                { $"Sim_{x}", 2020919 },   // Sharpshooter Infantry Medal
                { $"Mvm_{x}", 2021322 },   // Medal of Valor
                { $"Dsm_{x}", 2020419 },   // Distinguished Service Medal
                { $"Ncm_{x}", 2021403 },   // Navy Cross Medal
                { $"Gsm_{x}", 2020719 },   // Gold Star Medal
                { $"pmm_{x}", 2021613 }    // Presidential Medal of Merit
            };

            foreach (var medal in medals)
            {
                if (data.ContainsKey(medal.Key))
                {
                    awards.Add((medal.Value, 1));
                }
            }
        }

        private void ProcessSFBadges(Dictionary<string, string> data, string x, List<(int, int)> awards)
        {
            // SF 徽章处理
            var sfBadges = new Dictionary<string, int>
            {
                { $"X1Acb_{x}", 1261119 },   // SF Assault Combat Badge
                { $"X1Atcb_{x}", 1261120 },  // SF Anti-Tank Combat Badge
                { $"X1Sncb_{x}", 1261109 },  // SF Sniper Combat Badge
                { $"X1Socb_{x}", 1261115 },  // SF Support Combat Badge
                { $"X1Sucb_{x}", 1261121 },  // SF Supply Combat Badge
                { $"X1Ecb_{x}", 1261105 },   // SF Engineer Combat Badge
                { $"X1Mcb_{x}", 1261113 },   // SF Medic Combat Badge
                { $"X1fbb_{x}", 1260602 },   // SF Flashbang Badge
                { $"X1ghb_{x}", 1260708 },   // SF Grappling Hook Badge
                { $"X1zlb_{x}", 1262612 }    // SF Zipline Badge
            };

            foreach (var badge in sfBadges)
            {
                if (data.ContainsKey(badge.Key) && int.TryParse(data[badge.Key], out int count) && count > 0)
                {
                    awards.Add((badge.Value, count));
                }
            }
        }

        private void ProcessSFRibbons(Dictionary<string, string> data, string x, List<(int, int)> awards)
        {
            // SF 丝带处理
            var sfRibbons = new Dictionary<string, int>
            {
                { $"X1Nss_{x}", 3261919 },   // SF Night Shift Service
                { $"X1Sas_{x}", 3261901 },   // SF Special Air Service
                { $"X1Rsz_{x}", 3261819 },   // SF Rebel Service Ribbon
                { $"X1Msf_{x}", 3261319 },   // SF Meritorious Service
                { $"X1Reb_{x}", 3261805 },   // SF Rebel Service
                { $"X1Ins_{x}", 3260914 },   // SF Insurgent Service
                { $"X1Csr_{x}", 3260318 },   // SF Combat Service Ribbon
                { $"X1Arr_{x}", 3260118 },   // SF Aerial Service Ribbon
                { $"X1Aer_{x}", 3260105 },   // SF Air Excellence Ribbon
                { $"X1Hsr_{x}", 3260803 }    // SF Helicopter Service Ribbon
            };

            foreach (var ribbon in sfRibbons)
            {
                if (data.ContainsKey(ribbon.Key))
                {
                    awards.Add((ribbon.Value, 1));
                }
            }
        }

        private void ProcessSFMedals(Dictionary<string, string> data, string x, List<(int, int)> awards)
        {
            // SF 奖章处理
            var sfMedals = new Dictionary<string, int>
            {
                { $"X1Nsm_{x}", 2261913 },   // SF Night Shift Medal
                { $"X1Ssm_{x}", 2261919 },   // SF Special Service Medal
                { $"X1Spm_{x}", 2261613 },   // SF Special Purpose Medal
                { $"X1Mcm_{x}", 2261303 },   // SF Meritorious Conduct Medal
                { $"X1Rbm_{x}", 2261802 },   // SF Rebel Service Medal
                { $"X1Inm_{x}", 2260914 }    // SF Insurgent Medal
            };

            foreach (var medal in sfMedals)
            {
                if (data.ContainsKey(medal.Key))
                {
                    awards.Add((medal.Value, 1));
                }
            }
        }

        private async Task SaveAwardsAsync(int playerId, List<(int AwardId, int Count)> awards)
        {
            try
            {
                foreach (var (awardId, count) in awards)
                {
                    // 检查是否已存在该奖章
                    var existingAward = await _context.Awards
                        .FirstOrDefaultAsync(a => a.PlayerId == playerId && a.AwardId == awardId); // 使用 PlayerId 和 AwardId 进行查找

                    if (existingAward != null)
                    {
                        // 更新现有奖章的数量
                        existingAward.Level = (byte)count;
                        existingAward.Earned = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // 更新获得时间
                    }
                    else
                    {
                        // 创建新的奖章记录
                        var newAward = new Award
                        {
                            // Id 将由数据库自增生成
                            PlayerId = playerId,
                            AwardId = awardId,
                            Level = (byte)count,
                            When = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                            First = (byte)(existingAward == null ? 1 : 0)
                        };
                        _context.Awards.Add(newAward);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"保存玩家 {playerId} 的奖章时发生错误");
                throw;
            }
        }
    }
}