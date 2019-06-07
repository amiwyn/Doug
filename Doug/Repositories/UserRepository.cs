using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string userId);
    }
    public class UserRepository : IUserRepository
    {
        private DougContext db;

        public UserRepository(DougContext dougContext)
        {
            this.db = dougContext;
        }

        public void AddUser(string userId)
        {
            if (!db.Users.Any(user => user.Id == userId)) {

                var user = new User()
                {
                    Id = userId,
                    Credits = 10
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}
