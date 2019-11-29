using Doug.Items;
using Doug.Items.WeaponType;
using Doug.Models;
using Doug.Models.Coffee;
using Doug.Models.Monsters;
using Doug.Models.Slurs;
using Doug.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Doug
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
        public DbSet<Party> Parties { get; set; }
        public DbSet<Recipe> Recipes { get; set; }  
        public DbSet<Item> Items { get; set; }
        public DbSet<DropTable> Droptables { get; set; }
        public DbSet<Monster> Monsters { get; set; }  

        public DougContext(DbContextOptions<DougContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consumable>();
            modelBuilder.Entity<Food>();
            modelBuilder.Entity<SpecialFood>();
            modelBuilder.Entity<Ticket>();
            modelBuilder.Entity<Lootbox>();
            modelBuilder.Entity<EquipmentItem>();
            modelBuilder.Entity<Weapon>();
            modelBuilder.Entity<SkillBook>();

            modelBuilder.Entity<Axe>();
            modelBuilder.Entity<Bow>();
            modelBuilder.Entity<Claws>();
            modelBuilder.Entity<Dagger>();
            modelBuilder.Entity<GreatSword>();
            modelBuilder.Entity<Gun>();
            modelBuilder.Entity<Shield>();
            modelBuilder.Entity<Staff>();
            modelBuilder.Entity<Sword>();

            modelBuilder.Entity<InventoryItem>()
                .HasKey(u => new { u.UserId, u.InventoryPosition });

            modelBuilder.Entity<InventoryItem>()
                .HasOne(u => u.User)
                .WithMany(u => u.InventoryItems)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<LootItem>()
                .HasKey(e => new { e.Id, e.DropTableId });

            modelBuilder.Entity<MonsterAttacker>()
                .HasKey(m => new { m.SpawnedMonsterId, m.UserId });

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
