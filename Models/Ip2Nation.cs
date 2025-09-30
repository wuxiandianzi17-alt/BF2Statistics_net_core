using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("ip2nation")]
    public class Ip2Nation
    {
        [Key]
        [Column("ip")]
        public int Ip { get; set; }

        [Column("country")]
        [StringLength(2)]
        public string Country { get; set; } = "";
    }
}