using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("kits")]
    public class Kit
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("time0")]
        public int Time0 { get; set; } = 0;

        [Column("time1")]
        public int Time1 { get; set; } = 0;

        [Column("time2")]
        public int Time2 { get; set; } = 0;

        [Column("time3")]
        public int Time3 { get; set; } = 0;

        [Column("time4")]
        public int Time4 { get; set; } = 0;

        [Column("time5")]
        public int Time5 { get; set; } = 0;

        [Column("time6")]
        public int Time6 { get; set; } = 0;

        [Column("kills0")]
        public int Kills0 { get; set; } = 0;

        [Column("kills1")]
        public int Kills1 { get; set; } = 0;

        [Column("kills2")]
        public int Kills2 { get; set; } = 0;

        [Column("kills3")]
        public int Kills3 { get; set; } = 0;

        [Column("kills4")]
        public int Kills4 { get; set; } = 0;

        [Column("kills5")]
        public int Kills5 { get; set; } = 0;

        [Column("kills6")]
        public int Kills6 { get; set; } = 0;

        [Column("deaths0")]
        public int Deaths0 { get; set; } = 0;

        [Column("deaths1")]
        public int Deaths1 { get; set; } = 0;

        [Column("deaths2")]
        public int Deaths2 { get; set; } = 0;

        [Column("deaths3")]
        public int Deaths3 { get; set; } = 0;

        [Column("deaths4")]
        public int Deaths4 { get; set; } = 0;

        [Column("deaths5")]
        public int Deaths5 { get; set; } = 0;

        [Column("deaths6")]
        public int Deaths6 { get; set; } = 0;
    }
}