using Doug.Monsters;
using Microsoft.EntityFrameworkCore;

namespace Doug.Models
{
    public class DougContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Slur> Slurs { get; set; }
        public DbSet<CoffeeBreak> CoffeeBreak { get; set; }
        public DbSet<Roster> Roster { get; set; }
        public DbSet<RecentFlame> RecentSlurs { get; set; }
        public DbSet<GambleChallenge> GambleChallenges { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UserEffect> UserEffect { get; set; }
        public DbSet<Government> Government { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<SpawnedMonster> SpawnedMonsters { get; set; }

        public DougContext(DbContextOptions<DougContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryItem>()
                .HasKey(u => new { u.UserId, u.InventoryPosition });

            modelBuilder.Entity<InventoryItem>()
                .HasOne(u => u.User)
                .WithMany(u => u.InventoryItems)
                .HasForeignKey(u => u.UserId);


            modelBuilder.Entity<Loadout>()
                .ToTable("Users")
                .HasBaseType((string)null);

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasOne(o => o.Loadout).WithOne()
                .HasForeignKey<User>(o => o.Id);


            modelBuilder.Entity<UserEffect>()
                .HasKey(e => new { e.UserId, e.EffectId });

            modelBuilder.Entity<UserEffect>()
                .HasOne(u => u.User)
                .WithMany(u => u.Effects)
                .HasForeignKey(u => u.UserId);


            modelBuilder.Entity<ShopItem>()
                .HasKey(s => new { s.ShopId, s.ItemId });

            modelBuilder.Entity<ShopItem>()
                .HasOne(s => s.Shop)
                .WithMany(s => s.ShopItems)
                .HasForeignKey(s => s.ShopId);


            modelBuilder.Entity<MonsterAttacker>()
                .HasKey(m => new { m.UserId, m.SpawnedMonsterId });

            modelBuilder.Entity<MonsterAttacker>()
                .HasOne(m => m.Monster)
                .WithMany(m => m.MonsterAttackers)
                .HasForeignKey(m => m.SpawnedMonsterId);

            modelBuilder.Entity<MonsterAttacker>()
                .HasOne(m => m.User).WithOne();


            modelBuilder.Entity<RegionMonster>()
                .HasKey(m => new { m.ChannelId, m.MonsterId });

            modelBuilder.Entity<RegionMonster>()
                .HasOne(m => m.Channel)
                .WithMany(m => m.Monsters)
                .HasForeignKey(m => m.ChannelId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
