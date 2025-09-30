using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("unlocks")]
    public class Unlock
    {
        [Key]
        [Column("id")]
        public int PlayerId { get; set; } // 这是玩家ID，作为复合主键的一部分

        [Key]
        [Column("kit")]
        public int UnlockId { get; set; } // 这是解锁ID（kit），作为复合主键的一部分

        [Column("state")]
        public int State { get; set; } = 0; // 0=not unlocked, 1=selected/unlocked

        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; } = null!;
    }
}