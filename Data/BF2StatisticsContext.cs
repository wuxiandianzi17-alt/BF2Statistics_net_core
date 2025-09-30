using Microsoft.EntityFrameworkCore;
using BF2Statistics.Models;

namespace BF2Statistics.Data
{
    public class BF2StatisticsContext : DbContext
    {
        public BF2StatisticsContext(DbContextOptions<BF2StatisticsContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Award> Awards { get; set; } = null!;
        public DbSet<Army> Armies { get; set; } = null!;
        public DbSet<Weapon> Weapons { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Kit> Kits { get; set; } = null!;
        public DbSet<Map> Maps { get; set; } = null!;
        public DbSet<MapInfo> MapInfos { get; set; } = null!;
        public DbSet<Kill> Kills { get; set; } = null!;
        public DbSet<Unlock> Unlocks { get; set; } = null!;
        public DbSet<Ip2Nation> Ip2Nations { get; set; } = null!;
        public DbSet<Server> Servers { get; set; } = null!;
        public DbSet<GameSession> GameSessions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Player表配置
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasDefaultValue("");
                entity.Property(e => e.Country).HasDefaultValue("xx");
                entity.Property(e => e.Ip).HasDefaultValue("");
            });

            // Award表配置
            modelBuilder.Entity<Award>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Level).HasDefaultValue((byte)0);
                entity.Property(e => e.When).HasDefaultValue(0);
                entity.Property(e => e.First).HasDefaultValue((byte)0);
            });

            // Army表配置
            modelBuilder.Entity<Army>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Time0).HasDefaultValue(0);
                entity.Property(e => e.Time1).HasDefaultValue(0);
                entity.Property(e => e.Time2).HasDefaultValue(0);
                entity.Property(e => e.Wins).HasDefaultValue(0);
                entity.Property(e => e.Losses).HasDefaultValue(0);
                entity.Property(e => e.Score).HasDefaultValue(0);
                entity.Property(e => e.BestScore).HasDefaultValue(0);
                entity.Property(e => e.WorstScore).HasDefaultValue(0);
            });

            // Weapon表配置
            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Kills).HasDefaultValue(0);
                entity.Property(e => e.Deaths).HasDefaultValue(0);
                entity.Property(e => e.Fired).HasDefaultValue(0);
                entity.Property(e => e.Hit).HasDefaultValue(0);
            });

            // Vehicle表配置
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Time).HasDefaultValue(0);
                entity.Property(e => e.Kills).HasDefaultValue(0);
                entity.Property(e => e.Deaths).HasDefaultValue(0);
                entity.Property(e => e.RoadKills).HasDefaultValue(0);
            });

            // Kit表配置
            modelBuilder.Entity<Kit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Time0).HasDefaultValue(0);
                entity.Property(e => e.Time1).HasDefaultValue(0);
                entity.Property(e => e.Time2).HasDefaultValue(0);
            });

            // Map表配置
            modelBuilder.Entity<Map>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Time).HasDefaultValue(0);
                entity.Property(e => e.Wins).HasDefaultValue(0);
                entity.Property(e => e.Losses).HasDefaultValue(0);
            });

            // MapInfo表配置
            modelBuilder.Entity<MapInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(32);
            });

            // Kill表配置
            modelBuilder.Entity<Kill>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // Unlock表配置
            modelBuilder.Entity<Unlock>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.UnlockId }); // 复合主键
                entity.Property(e => e.State).HasDefaultValue(0);
            });

            // Ip2Nation表配置
            modelBuilder.Entity<Ip2Nation>(entity =>
            {
                entity.HasKey(e => e.Ip);
                entity.Property(e => e.Country).HasMaxLength(2);
            });

            // Server表配置
            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ip).HasDefaultValue("");
                entity.Property(e => e.Prefix).HasDefaultValue("");
                entity.Property(e => e.Port).HasDefaultValue(0);
                entity.Property(e => e.QueryPort).HasDefaultValue(0);
                entity.Property(e => e.RconPort).HasDefaultValue(4711);
                entity.Property(e => e.RconPassword).HasMaxLength(50);
                entity.Property(e => e.LastUpdate).HasDefaultValue(DateTime.MinValue);
                
                // 创建唯一索引 (ip, port)
                entity.HasIndex(e => new { e.Ip, e.Port }).IsUnique();
            });

            // GameSession表配置
            modelBuilder.Entity<GameSession>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ServerIp).HasDefaultValue("");
                entity.Property(e => e.MapName).HasDefaultValue("");
                entity.Property(e => e.Version).HasDefaultValue("bf2");
                entity.Property(e => e.CreatedAt).HasDefaultValue(DateTime.UtcNow);
                entity.Property(e => e.UpdatedAt).HasDefaultValue(DateTime.UtcNow);
                
                // 创建索引
                entity.HasIndex(e => e.ServerIp);
                entity.HasIndex(e => e.MapName);
                entity.HasIndex(e => e.MapStart);
                entity.HasIndex(e => e.CreatedAt);
            });
        }
    }
}