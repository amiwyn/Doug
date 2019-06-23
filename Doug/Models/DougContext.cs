using Doug.Items.Equipment;
using Microsoft.EntityFrameworkCore;

namespace Doug.Models
{
    public class DougContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Slur> Slurs { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Roster> Roster { get; set; }
        public DbSet<RecentFlame> RecentSlurs { get; set; }
        public DbSet<GambleChallenge> GambleChallenges { get; set; }

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

            modelBuilder.Entity<AwakeningOrb>();
            modelBuilder.Entity<LuckyDice>();
            modelBuilder.Entity<BurglarBoots>();
            modelBuilder.Entity<GreedyGloves>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
