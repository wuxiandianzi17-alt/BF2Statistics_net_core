using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("kills")]
    public class Kill
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("victim")]
        public int Victim { get; set; } = 0;

        [Column("attacker")]
        public int Attacker { get; set; } = 0;

        [Column("count")]
        public int Count { get; set; } = 0;
    }
}