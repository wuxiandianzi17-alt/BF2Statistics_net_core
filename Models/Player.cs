using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("player")]
    public class Player
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [StringLength(45)]
        public string Name { get; set; } = "";

        [Column("country")]
        [StringLength(2)]
        public string Country { get; set; } = "xx";

        [Column("time")]
        public int Time { get; set; } = 0;

        [Column("rounds")]
        public int Rounds { get; set; } = 0;

        [Column("ip")]
        [StringLength(15)]
        public string Ip { get; set; } = "";

        [Column("score")]
        public int Score { get; set; } = 0;

        [Column("cmdscore")]
        public int CmdScore { get; set; } = 0;

        [Column("skillscore")]
        public int SkillScore { get; set; } = 0;

        [Column("teamscore")]
        public int TeamScore { get; set; } = 0;

        [Column("kills")]
        public int Kills { get; set; } = 0;

        [Column("deaths")]
        public int Deaths { get; set; } = 0;

        [Column("captures")]
        public int Captures { get; set; } = 0;

        [Column("neutralizes")]
        public int Neutralizes { get; set; } = 0;

        [Column("captureassists")]
        public int CaptureAssists { get; set; } = 0;

        [Column("neutralizeassists")]
        public int NeutralizeAssists { get; set; } = 0;

        [Column("defends")]
        public int Defends { get; set; } = 0;

        [Column("damageassists")]
        public int DamageAssists { get; set; } = 0;

        [Column("heals")]
        public int Heals { get; set; } = 0;

        [Column("revives")]
        public int Revives { get; set; } = 0;

        [Column("ammos")]
        public int Ammos { get; set; } = 0;

        [Column("repairs")]
        public int Repairs { get; set; } = 0;

        [Column("targetassists")]
        public int TargetAssists { get; set; } = 0;

        [Column("driverspecials")]
        public int DriverSpecials { get; set; } = 0;

        [Column("driverassists")]
        public int DriverAssists { get; set; } = 0;

        [Column("passengerassists")]
        public int PassengerAssists { get; set; } = 0;

        [Column("teamkills")]
        public int TeamKills { get; set; } = 0;

        [Column("teamdamage")]
        public int TeamDamage { get; set; } = 0;

        [Column("teamvehicledamage")]
        public int TeamVehicleDamage { get; set; } = 0;

        [Column("suicides")]
        public int Suicides { get; set; } = 0;

        [Column("killstreak")]
        public int KillStreak { get; set; } = 0;

        [Column("deathstreak")]
        public int DeathStreak { get; set; } = 0;

        [Column("rank")]
        public byte Rank { get; set; } = 0;

        [Column("banned")]
        public int Banned { get; set; } = 0;

        [Column("kicked")]
        public int Kicked { get; set; } = 0;

        [Column("cmdtime")]
        public int CmdTime { get; set; } = 0;

        [Column("sqltime")]
        public int SqlTime { get; set; } = 0;

        [Column("sqmtime")]
        public int SqmTime { get; set; } = 0;

        [Column("lwtime")]
        public int LwTime { get; set; } = 0;

        [Column("wins")]
        public int Wins { get; set; } = 0;

        [Column("losses")]
        public int Losses { get; set; } = 0;

        [Column("availunlocks")]
        public byte AvailableUnlocks { get; set; } = 0;

        [Column("usedunlocks")]
        public byte UsedUnlocks { get; set; } = 0;

        [Column("joined")]
        public int Joined { get; set; } = 0;

        [Column("rndscore")]
        public int RndScore { get; set; } = 0;

        [Column("lastonline")]
        public int LastOnline { get; set; } = 0;

        [Column("chng")]
        public byte Chng { get; set; } = 0;

        [Column("decr")]
        public byte Decr { get; set; } = 0;

        [Column("mode0")]
        public int Mode0 { get; set; } = 0;

        [Column("mode1")]
        public int Mode1 { get; set; } = 0;

        [Column("mode2")]
        public int Mode2 { get; set; } = 0;

        [Column("permban")]
        public byte PermBan { get; set; } = 0;

        [Column("clantag")]
        [StringLength(10)]
        public string ClanTag { get; set; } = "";
    }
}