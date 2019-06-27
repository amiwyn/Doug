﻿using System.Collections.Generic;
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
        void AttributeStatPoint(string userId, string stat);
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

        public void AttributeStatPoint(string userId, string stat)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            switch (stat)
            {
                // TODO magic strings
                case "luck":
                    user.Luck++;
                    break;
                case "agility":
                    user.Agility++;
                    break;
                case "charisma":
                    user.Charisma++;
                    break;
                case "constitution":
                    user.Constitution++;
                    break;
                case "stamina":
                    user.Stamina++;
                    break;
            }

            _db.SaveChanges();
        }
    }
}
