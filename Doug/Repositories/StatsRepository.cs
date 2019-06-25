using System.Collections.Generic;
using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IStatsRepository
    {
        void UpdateEnergy(string userId, int energy);
        void UpdateHealth(string userId, int health);
        void AddExperience(string userId, long experience);
        void AddExperienceToUsers(List<string> userIds, long experience);
    }

    public class StatsRepository : IStatsRepository
    {
        private readonly DougContext _db;

        public StatsRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void UpdateEnergy(string userId, int energy)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);
            user.Energy = energy;
            _db.SaveChanges();
        }

        public void UpdateHealth(string userId, int health)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);
            user.Health = health;
            _db.SaveChanges();
        }

        public void AddExperience(string userId, long experience)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);
            user.Experience += experience;
            _db.SaveChanges();
        }

        public void AddExperienceToUsers(List<string> userIds, long experience)
        {
            var users = _db.Users.Where(usr => userIds.Contains(usr.Id)).ToList();

            users.ForEach(usr => usr.Experience += experience);

            _db.SaveChanges();
        }
    }
}
