using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("awards")]
    public class Award
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("pid")]
        public int PlayerId { get; set; }

        [Column("name")]
        [MaxLength(50)]
        public string PlayerName { get; set; } = "";

        [Column("awardid")]
        public int AwardId { get; set; }

        [Column("level")]
        public byte Level { get; set; } = 0;

        [Column("when")]
        public int When { get; set; } = 0;

        [Column("first")]
        public byte First { get; set; } = 0;

        [Column("earned")]
        public int Earned { get; set; } = 0;
    }
}