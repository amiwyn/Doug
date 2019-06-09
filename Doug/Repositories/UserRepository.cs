using Doug.Models;
using Doug.Slack;
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
        void RemoveCredits(string userId, int amount);
        void AddCredits(string userId, int amount);
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

        public ICollection<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        public void RemoveCredits(string userId, int amount)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            if (amount < 0)
            {
                throw new ArgumentException(DougMessages.InvalidAmount);
            }

            if (user.Credits - amount < 0)
            {
                throw new ArgumentException(string.Format(DougMessages.NotEnoughCredits, amount, user.Credits));
            }

            user.Credits -= amount;
            _db.SaveChanges();
        }
    }
}
