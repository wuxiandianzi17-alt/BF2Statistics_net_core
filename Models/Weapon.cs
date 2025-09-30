using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BF2Statistics.Models
{
    [Table("weapons")]
    public class Weapon
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // 武器使用时间 (time0-time8)
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

        // 特殊武器时间
        [Column("knifetime")]
        public int KnifeTime { get; set; } = 0;
        [Column("c4time")]
        public int C4Time { get; set; } = 0;
        [Column("handgrenadetime")]
        public int HandGrenadeTime { get; set; } = 0;
        [Column("claymoretime")]
        public int ClaymoreTime { get; set; } = 0;
        [Column("shockpadtime")]
        public int ShockpadTime { get; set; } = 0;
        [Column("atminetime")]
        public int AtmineTime { get; set; } = 0;
        [Column("tacticaltime")]
        public int TacticalTime { get; set; } = 0;
        [Column("grapplinghooktime")]
        public int GrapplinghookTime { get; set; } = 0;
        [Column("ziplinetime")]
        public int ZiplineTime { get; set; } = 0;

        // 武器击杀数 (kills0-kills8)
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
        [Column("kills7")]
        public int Kills7 { get; set; } = 0;
        [Column("kills8")]
        public int Kills8 { get; set; } = 0;

        // 特殊武器击杀数
        [Column("knifekills")]
        public int KnifeKills { get; set; } = 0;
        [Column("c4kills")]
        public int C4Kills { get; set; } = 0;
        [Column("handgrenadekills")]
        public int HandGrenadeKills { get; set; } = 0;
        [Column("claymorekills")]
        public int ClaymoreKills { get; set; } = 0;
        [Column("shockpadkills")]
        public int ShockpadKills { get; set; } = 0;
        [Column("atminekills")]
        public int AtmineKills { get; set; } = 0;
        [Column("tacticalkills")]
        public int TacticalKills { get; set; } = 0;
        [Column("grapplinghookkills")]
        public int GrapplinghookKills { get; set; } = 0;
        [Column("ziplinekills")]
        public int ZiplineKills { get; set; } = 0;

        // 武器死亡数 (deaths0-deaths8)
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
        [Column("deaths7")]
        public int Deaths7 { get; set; } = 0;
        [Column("deaths8")]
        public int Deaths8 { get; set; } = 0;

        // 特殊武器死亡数
        [Column("knifedeaths")]
        public int KnifeDeaths { get; set; } = 0;
        [Column("c4deaths")]
        public int C4Deaths { get; set; } = 0;
        [Column("handgrenadedeaths")]
        public int HandGrenadeDeaths { get; set; } = 0;
        [Column("claymoredeaths")]
        public int ClaymoreDeaths { get; set; } = 0;
        [Column("shockpaddeaths")]
        public int ShockpadDeaths { get; set; } = 0;
        [Column("atminedeaths")]
        public int AtmineDeaths { get; set; } = 0;
        [Column("ziplinedeaths")]
        public int ZiplineDeaths { get; set; } = 0;
        [Column("grapplinghookdeaths")]
        public int GrapplinghookDeaths { get; set; } = 0;
        [Column("tacticaldeaths")]
        public int TacticalDeaths { get; set; } = 0;

        // 部署数量
        [Column("tacticaldeployed")]
        public int TacticalDeployed { get; set; } = 0;
        [Column("grapplinghookdeployed")]
        public int GrapplinghookDeployed { get; set; } = 0;
        [Column("ziplinedeployed")]
        public int ZiplineDeployed { get; set; } = 0;

        // 武器发射数 (fired0-fired8)
        [Column("fired0")]
        public int Fired0 { get; set; } = 0;
        [Column("fired1")]
        public int Fired1 { get; set; } = 0;
        [Column("fired2")]
        public int Fired2 { get; set; } = 0;
        [Column("fired3")]
        public int Fired3 { get; set; } = 0;
        [Column("fired4")]
        public int Fired4 { get; set; } = 0;
        [Column("fired5")]
        public int Fired5 { get; set; } = 0;
        [Column("fired6")]
        public int Fired6 { get; set; } = 0;
        [Column("fired7")]
        public int Fired7 { get; set; } = 0;
        [Column("fired8")]
        public int Fired8 { get; set; } = 0;

        // 特殊武器发射数
        [Column("knifefired")]
        public int KnifeFired { get; set; } = 0;
        [Column("c4fired")]
        public int C4Fired { get; set; } = 0;
        [Column("claymorefired")]
        public int ClaymoreFired { get; set; } = 0;
        [Column("handgrenadefired")]
        public int HandGrenadeFired { get; set; } = 0;
        [Column("shockpadfired")]
        public int ShockpadFired { get; set; } = 0;
        [Column("atminefired")]
        public int AtmineFired { get; set; } = 0;
        [Column("tacticalfired")]
        public int TacticalFired { get; set; } = 0;
        [Column("grapplinghookfired")]
        public int GrapplinghookFired { get; set; } = 0;
        [Column("ziplinefired")]
        public int ZiplineFired { get; set; } = 0;

        // 武器命中数 (hit0-hit8)
        [Column("hit0")]
        public int Hit0 { get; set; } = 0;
        [Column("hit1")]
        public int Hit1 { get; set; } = 0;
        [Column("hit2")]
        public int Hit2 { get; set; } = 0;
        [Column("hit3")]
        public int Hit3 { get; set; } = 0;
        [Column("hit4")]
        public int Hit4 { get; set; } = 0;
        [Column("hit5")]
        public int Hit5 { get; set; } = 0;
        [Column("hit6")]
        public int Hit6 { get; set; } = 0;
        [Column("hit7")]
        public int Hit7 { get; set; } = 0;
        [Column("hit8")]
        public int Hit8 { get; set; } = 0;

        // 特殊武器命中数
        [Column("knifehit")]
        public int KnifeHit { get; set; } = 0;
        [Column("c4hit")]
        public int C4Hit { get; set; } = 0;
        [Column("claymorehit")]
        public int ClaymoreHit { get; set; } = 0;
        [Column("handgrenadehit")]
        public int HandGrenadeHit { get; set; } = 0;
        [Column("shockpadhit")]
        public int ShockpadHit { get; set; } = 0;
        [Column("atminehit")]
        public int AtmineHit { get; set; } = 0;
        [Column("tacticalhit")]
        public int TacticalHit { get; set; } = 0;
        [Column("grapplinghookhit")]
        public int GrapplinghookHit { get; set; } = 0;
        [Column("ziplinehit")]
        public int ZiplineHit { get; set; } = 0;

        // 兼容性属性 - 总计
        [NotMapped]
        public int Time { get; set; }
        
        [NotMapped]
        public int Kills { get; set; }
        
        [NotMapped]
        public int Deaths { get; set; }
        
        [NotMapped]
        public int Fired { get; set; }
        
        [NotMapped]
        public int Hit { get; set; }
        
        [NotMapped]
        public float Damage { get; set; } = 0;

        [NotMapped]
        public float Accuracy { get; set; }
    }
}