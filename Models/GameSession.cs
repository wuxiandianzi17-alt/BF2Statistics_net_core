using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("game_sessions")]
    public class GameSession
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("server_ip")]
        [StringLength(15)]
        public string ServerIp { get; set; } = "";

        [Column("game_port")]
        public int GamePort { get; set; }

        [Column("query_port")]
        public int QueryPort { get; set; }

        [Required]
        [Column("map_name")]
        [StringLength(50)]
        public string MapName { get; set; } = "";

        [Column("map_id")]
        public int MapId { get; set; }

        [Column("map_start")]
        public double MapStart { get; set; }

        [Column("map_end")]
        public double MapEnd { get; set; }

        [Column("winner")]
        public int Winner { get; set; }

        [Column("game_mode")]
        public int GameMode { get; set; }

        [Column("mod_id")]
        public int ModId { get; set; }

        [Column("version")]
        [StringLength(10)]
        public string Version { get; set; } = "";

        [Column("pc")]
        public int PlayerCount { get; set; }

        [Column("ra1")]
        public int RoundsArmy1 { get; set; }

        [Column("rs1")]
        public int ScoreArmy1 { get; set; }

        [Column("ra2")]
        public int RoundsArmy2 { get; set; }

        [Column("rs2")]
        public int ScoreArmy2 { get; set; }

        [Column("rst2")]
        public int ScoreTeam2 { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 计算游戏时长（秒）
        [NotMapped]
        public double GameDuration => MapEnd - MapStart;

        // 获取地图开始时间
        [NotMapped]
        public DateTime MapStartTime => DateTimeOffset.FromUnixTimeSeconds((long)MapStart).DateTime;

        // 获取地图结束时间
        [NotMapped]
        public DateTime MapEndTime => DateTimeOffset.FromUnixTimeSeconds((long)MapEnd).DateTime;
    }
}