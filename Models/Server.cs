using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("servers")]
    public class Server
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ip")]
        [StringLength(15)]
        public string Ip { get; set; } = "";

        [Required]
        [Column("prefix")]
        [StringLength(30)]
        public string Prefix { get; set; } = "";

        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Column("port")]
        public int Port { get; set; } = 0;

        [Column("queryport")]
        public int QueryPort { get; set; } = 0;

        [Column("rcon_port")]
        public int RconPort { get; set; } = 4711;

        [Column("rcon_password")]
        [StringLength(50)]
        public string? RconPassword { get; set; }

        [Column("lastupdate")]
        public DateTime LastUpdate { get; set; } = DateTime.MinValue;
    }
}