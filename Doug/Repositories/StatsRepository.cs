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
        void LevelUpUsers(List<string> userIds);
        void AttributeStatPoint(string userId, string stat);
        void FreeStatPoint(string userId, string stat);
        void KillUser(string userId);
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

        public void LevelUpUsers(List<string> userIds)
        {
            var users = _db.Users.Where(usr => userIds.Contains(usr.Id)).ToList();

            users.ForEach(usr => usr.LevelUp());

            _db.SaveChanges();
        }

        public void AttributeStatPoint(string userId, string stat)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            EditStatPoint(user, stat, 1);

            _db.SaveChanges();
        }

        public void FreeStatPoint(string userId, string stat)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            EditStatPoint(user, stat, -1);

            _db.SaveChanges();
        }

        private void EditStatPoint(User user, string stat, int modifier)
        {
            switch (stat)
            {
                case Stats.Luck:
                    user.Luck += modifier;
                    break;
                case Stats.Agility:
                    user.Agility += modifier;
                    break;
                case Stats.Charisma:
                    user.Charisma += modifier;
                    break;
                case Stats.Constitution:
                    user.Constitution += modifier;
                    break;
                case Stats.Stamina:
                    user.Stamina += modifier;
                    break;
            }
        }

        public void KillUser(string userId)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Dies();

            _db.SaveChanges();
        }
    }
}
