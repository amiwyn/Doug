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
        List<User> GetUsers(List<string> users);
        User GetUser(string userId);
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

        public List<User> GetUsers(List<string> users)
        {
            var loadedUsers = _db.Users
                .Where(user => users.Any(usr => usr == user.Id))
                .IncludeFilter(usr => usr.Effects.Where(effect => effect.EndTime >= DateTime.UtcNow))
                .IncludeFilter(usr => usr.InventoryItems)
                .IncludeFilter(usr => usr.Loadout)
                .ToList();

            loadedUsers.ForEach(usr => usr.LoadItems(_itemFactory));
            loadedUsers.ForEach(usr => usr.LoadEffects(_effectFactory));

            return loadedUsers;
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
    }
}
