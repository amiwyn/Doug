using Doug.Items;
using Doug.Models;
using Doug.Slack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string userId);
        ICollection<User> GetUsers();
        User GetUser(string userId);
        void RemoveCredits(string userId, int amount);
        void AddCredits(string userId, int amount);
        void AddItem(string userid, int itemId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DougContext _db;

        public UserRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void AddCredits(string userId, int amount)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Credits += amount;

            _db.SaveChanges();
        }

        public void AddItem(string userId, int itemId)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);
            var item = _db.Items.Single(itm => itm.Id == itemId);
            user.UserItems.Add(new UserItem { User = user, Item = item });
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
            return _db.Users
                .Include(user => user.UserItems)
                .ThenInclude(userItem => userItem.Item)
                .Single(user => user.Id == userId);
        }

        public ICollection<User> GetUsers()
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
