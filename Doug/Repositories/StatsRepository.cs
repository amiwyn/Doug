using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Models;
using Doug.Monsters;

namespace Doug.Repositories
{
    public interface IStatsRepository
    {
        void UpdateEnergy(string userId, int energy);
        void UpdateHealth(string userId, int health);
        void AddExperienceToUsers(List<string> userIds, long experience);
        void AddMonsterExperienceToUsers(List<string> userIds, Monster monster);
        void LevelUpUsers(List<string> userIds);
        void AttributeStatPoint(string userId, string stat);
        void FreeStatPoint(string userId, string stat);
        void KillUser(string userId);
        void SetAttackCooldown(string userId, TimeSpan cooldown);
        void SetSkillCooldown(string userId, TimeSpan cooldown);
        void FireSkill(string userId, TimeSpan cooldown, int newEnergy);
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

        public void AddExperienceToUsers(List<string> userIds, long experience)
        {
            var users = _db.Users.Where(usr => userIds.Contains(usr.Id)).ToList();

            users.ForEach(usr => usr.Experience += experience);

            _db.SaveChanges();
        }

        public void AddMonsterExperienceToUsers(List<string> userIds, Monster monster)
        {
            var users = _db.Users.Where(usr => userIds.Contains(usr.Id)).ToList();

            users.ForEach(usr => usr.ReceiveExpFromMonster(monster, userIds.Count));

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

            if (stat == Stats.Constitution)
            {
                user.Health += 15;
            }

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
                    user.Luck += user.Luck + modifier < 1 ? 0 : modifier;
                    break;
                case Stats.Agility:
                    user.Agility += user.Agility + modifier < 1 ? 0 : modifier;
                    break;
                case Stats.Strength:
                    user.Strength += user.Strength + modifier < 1 ? 0 : modifier;
                    break;
                case Stats.Constitution:
                    user.Constitution += user.Constitution + modifier < 1 ? 0 : modifier;
                    break;
                case Stats.Intelligence:
                    user.Intelligence += user.Intelligence + modifier < 1 ? 0 : modifier;
                    break;
            }
        }

        public void KillUser(string userId)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Dies();

            _db.SaveChanges();
        }

        public void SetAttackCooldown(string userId, TimeSpan cooldown)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.AttackCooldown = DateTime.UtcNow + cooldown;

            _db.SaveChanges();
        }

        public void SetSkillCooldown(string userId, TimeSpan cooldown)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.SkillCooldown = DateTime.UtcNow + cooldown;

            _db.SaveChanges();
        }

        public void FireSkill(string userId, TimeSpan cooldown, int newEnergy)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Energy = newEnergy;
            user.SkillCooldown = DateTime.UtcNow + cooldown;

            _db.SaveChanges();
        }
    }
}
