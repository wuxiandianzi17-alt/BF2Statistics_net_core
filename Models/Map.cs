using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("maps")]
    public class Map
    {
        // 复合主键的一部分：玩家ID
        [Column("id")]
        public int Id { get; set; }

        // 复合主键的一部分：地图ID
        [Column("mapid")]
        public int MapId { get; set; }

        [Column("time")]
        public int Time { get; set; } = 0;

        [Column("win")]
        public int Win { get; set; } = 0;

        [Column("loss")]
        public int Loss { get; set; } = 0;

        [Column("best")]
        public int Best { get; set; } = 0;

        [Column("worst")]
        public int Worst { get; set; } = 0;
    }
}