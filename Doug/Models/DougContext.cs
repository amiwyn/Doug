using Doug.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class DougContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Slur> Slurs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Secret> Secrets { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Roster> Roster { get; set; }
        public DbSet<RecentFlame> RecentSlurs { get; set; }
        public DbSet<GambleChallenge> GambleChallenges { get; set; }

        public DougContext(DbContextOptions<DougContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserItem>()
                .HasKey(u => new { u.UserId, u.ItemId });

            modelBuilder.Entity<UserItem>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserItems)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserItem>()
                .HasOne(i => i.Item)
                .WithMany(i => i.UserItems)
                .HasForeignKey(i => i.ItemId);

            modelBuilder.Entity<AwakeningOrb>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
