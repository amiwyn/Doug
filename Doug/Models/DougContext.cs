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
        public DbSet<Secret> Secrets { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Roster> Roster { get; set; }
        public DbSet<RecentFlame> RecentSlurs { get; set; }
        public DbSet<GambleChallenge> GambleChallenges { get; set; }

        public DougContext(DbContextOptions<DougContext> options) : base(options)
        {
        }
    }
}
