using Doug.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Doug.Items;

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
    }

    public class UserRepository : IUserRepository
    {
        private readonly DougContext _db;
        private readonly IItemFactory _itemFactory;

        public UserRepository(DougContext dougContext, IItemFactory itemFactory)
        {
            _db = dougContext;
            _itemFactory = itemFactory;
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
                .Include(usr => usr.InventoryItems)
                .Include(usr => usr.Loadout)
                .Single(usr => usr.Id == userId);

            user.LoadItems(_itemFactory);

            return user;
        }

        public List<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        public void RemoveCredits(string userId, int amount)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Credits -= amount;
            _db.SaveChanges();
        }
    }
}
