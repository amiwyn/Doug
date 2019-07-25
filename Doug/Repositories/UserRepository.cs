using System;
using Doug.Models;
using System.Collections.Generic;
using System.Linq;
using Doug.Effects;
using Doug.Items;
using Z.EntityFramework.Plus;

namespace Doug.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string userId);
        List<User> GetUsers();
        User GetUser(string userId);
        void RemoveCredits(string userId, int amount);
        void AddCredits(string userId, int amount);
        void AddCreditsToUsers(List<string> users, int amount);
        void SetAttackCooldown(string userId, TimeSpan cooldown);
        void SetStealCooldown(string userId, TimeSpan cooldown);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DougContext _db;
        private readonly IItemFactory _itemFactory;
        private readonly IEffectFactory _effectFactory;

        public UserRepository(DougContext dougContext, IItemFactory itemFactory, IEffectFactory effectFactory)
        {
            _db = dougContext;
            _itemFactory = itemFactory;
            _effectFactory = effectFactory;
        }

        public void AddCredits(string userId, int amount)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Credits += amount;

            _db.SaveChanges();
        }

        public void AddCreditsToUsers(List<string> userIds, int amount)
        {
            var users = _db.Users.Where(usr => userIds.Contains(usr.Id)).ToList();

            users.ForEach(usr => usr.Credits += amount);

            _db.SaveChanges();
        }

        public void SetAttackCooldown(string userId, TimeSpan cooldown)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.AttackCooldown = DateTime.UtcNow + cooldown;

            _db.SaveChanges();
        }

        public void SetStealCooldown(string userId, TimeSpan cooldown)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.StealCooldown = DateTime.UtcNow + cooldown;

            _db.SaveChanges();
        }

        public void AddUser(string userId)
        {
            if (!_db.Users.Any(user => user.Id == userId)) {

                var user = new User()
                {
                    Id = userId,
                    Credits = 10
                };

                _db.Users.Add(user);
                _db.SaveChanges();
            }
        }

        public User GetUser(string userId)
        {
            var user = _db.Users
                .IncludeFilter(usr => usr.Effects.Where(effect => effect.EndTime >= DateTime.UtcNow))
                .IncludeFilter(usr => usr.InventoryItems)
                .IncludeFilter(usr => usr.Loadout)
                .Single(usr => usr.Id == userId);

            user.LoadItems(_itemFactory);
            user.LoadEffects(_effectFactory);


            return user;
        }

        public List<User> GetUsers()
        {
            var users = _db.Users
                .IncludeFilter(usr => usr.Effects.Where(effect => effect.EndTime >= DateTime.UtcNow))
                .IncludeFilter(usr => usr.InventoryItems)
                .IncludeFilter(usr => usr.Loadout)
                .ToList();

            users.ForEach(usr => usr.LoadItems(_itemFactory));
            users.ForEach(usr => usr.LoadEffects(_effectFactory));

            return users;
        }

        public void RemoveCredits(string userId, int amount)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Credits -= amount;
            _db.SaveChanges();
        }
    }
}
