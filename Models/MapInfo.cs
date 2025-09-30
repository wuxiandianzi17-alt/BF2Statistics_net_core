using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("mapinfo")]
    public class MapInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [StringLength(45)]
        public string Name { get; set; } = "";

        [Column("score")]
        public int Score { get; set; } = 0;

        [Column("time")]
        public int Time { get; set; } = 0;

        [Column("kills")]
        public int Kills { get; set; } = 0;

        [Column("deaths")]
        public int Deaths { get; set; } = 0;

        [Column("captures")]
        public int Captures { get; set; } = 0;

        [Column("assists")]
        public int Assists { get; set; } = 0;

        [Column("rounds")]
        public int Rounds { get; set; } = 0;

        [Column("times")]
        public int Times { get; set; } = 0;
    }
}