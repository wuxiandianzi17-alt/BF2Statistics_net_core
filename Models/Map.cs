using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("maps")]
    public class Map
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Column("time")]
        public int Time { get; set; } = 0;

        [Column("wins")]
        public int Wins { get; set; } = 0;

        [Column("losses")]
        public int Losses { get; set; } = 0;

        [Column("score")]
        public int Score { get; set; } = 0;

        [Column("bestscore")]
        public int BestScore { get; set; } = 0;

        [Column("worstscore")]
        public int WorstScore { get; set; } = 0;

        [Column("bested")]
        public int Bested { get; set; } = 0;

        [Column("worsted")]
        public int Worsted { get; set; } = 0;
    }
}