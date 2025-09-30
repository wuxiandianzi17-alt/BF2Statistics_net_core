using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("army")]
    public class Army
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

        [Column("time7")]
        public int Time7 { get; set; } = 0;

        [Column("time8")]
        public int Time8 { get; set; } = 0;

        [Column("win0")]
        public int Win0 { get; set; } = 0;

        [Column("win1")]
        public int Win1 { get; set; } = 0;

        [Column("win2")]
        public int Win2 { get; set; } = 0;

        [Column("win3")]
        public int Win3 { get; set; } = 0;

        [Column("win4")]
        public int Win4 { get; set; } = 0;

        [Column("win5")]
        public int Win5 { get; set; } = 0;

        [Column("win6")]
        public int Win6 { get; set; } = 0;

        [Column("win7")]
        public int Win7 { get; set; } = 0;

        [Column("win8")]
        public int Win8 { get; set; } = 0;

        [Column("loss0")]
        public int Loss0 { get; set; } = 0;

        [Column("loss1")]
        public int Loss1 { get; set; } = 0;

        [Column("loss2")]
        public int Loss2 { get; set; } = 0;

        [Column("loss3")]
        public int Loss3 { get; set; } = 0;

        [Column("loss4")]
        public int Loss4 { get; set; } = 0;

        [Column("loss5")]
        public int Loss5 { get; set; } = 0;

        [Column("loss6")]
        public int Loss6 { get; set; } = 0;

        [Column("loss7")]
        public int Loss7 { get; set; } = 0;

        [Column("loss8")]
        public int Loss8 { get; set; } = 0;

        [Column("score0")]
        public int Score0 { get; set; } = 0;

        [Column("score1")]
        public int Score1 { get; set; } = 0;

        [Column("score2")]
        public int Score2 { get; set; } = 0;

        [Column("score3")]
        public int Score3 { get; set; } = 0;

        [Column("score4")]
        public int Score4 { get; set; } = 0;

        [Column("score5")]
        public int Score5 { get; set; } = 0;

        [Column("score6")]
        public int Score6 { get; set; } = 0;

        [Column("score7")]
        public int Score7 { get; set; } = 0;

        [Column("score8")]
        public int Score8 { get; set; } = 0;

        [Column("best0")]
        public int Best0 { get; set; } = 0;

        [Column("best1")]
        public int Best1 { get; set; } = 0;

        [Column("best2")]
        public int Best2 { get; set; } = 0;

        [Column("best3")]
        public int Best3 { get; set; } = 0;

        [Column("best4")]
        public int Best4 { get; set; } = 0;

        [Column("best5")]
        public int Best5 { get; set; } = 0;

        [Column("best6")]
        public int Best6 { get; set; } = 0;

        [Column("best7")]
        public int Best7 { get; set; } = 0;

        [Column("best8")]
        public int Best8 { get; set; } = 0;

        [Column("worst0")]
        public int Worst0 { get; set; } = 0;

        [Column("worst1")]
        public int Worst1 { get; set; } = 0;

        [Column("worst2")]
        public int Worst2 { get; set; } = 0;

        [Column("worst3")]
        public int Worst3 { get; set; } = 0;

        [Column("worst4")]
        public int Worst4 { get; set; } = 0;

        [Column("worst5")]
        public int Worst5 { get; set; } = 0;

        [Column("worst6")]
        public int Worst6 { get; set; } = 0;

        [Column("worst7")]
        public int Worst7 { get; set; } = 0;

        [Column("worst8")]
        public int Worst8 { get; set; } = 0;

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

        [Column("cmdtime")]
        public int CmdTime { get; set; } = 0;

        [Column("sqltime")]
        public int SqlTime { get; set; } = 0;

        [Column("sqmtime")]
        public int SqmTime { get; set; } = 0;

        [Column("lwtime")]
        public int LwTime { get; set; } = 0;

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

        [Column("rndscore")]
        public int RndScore { get; set; } = 0;

        [Column("bested")]
        public int Bested { get; set; } = 0;

        [Column("worsted")]
        public int Worsted { get; set; } = 0;

        [Column("brnd0")]
        public int Brnd0 { get; set; } = 0;

        [Column("brnd1")]
        public int Brnd1 { get; set; } = 0;

        [Column("brnd2")]
        public int Brnd2 { get; set; } = 0;

        [Column("brnd3")]
        public int Brnd3 { get; set; } = 0;

        [Column("brnd4")]
        public int Brnd4 { get; set; } = 0;

        [Column("brnd5")]
        public int Brnd5 { get; set; } = 0;

        [Column("brnd6")]
        public int Brnd6 { get; set; } = 0;

        [Column("brnd7")]
        public int Brnd7 { get; set; } = 0;

        [Column("brnd8")]
        public int Brnd8 { get; set; } = 0;
    }
}