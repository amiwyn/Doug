using System.Collections.Generic;
using System.Linq;

namespace Doug.Repositories
{
    public interface ICreditsRepository
    {
        void RemoveCredits(string userId, int amount);
        void AddCredits(string userId, int amount);
        void AddCreditsToUsers(List<string> users, int amount);
    }

    public class CreditsRepository : ICreditsRepository
    {
        private readonly DougContext _db;

        public CreditsRepository(DougContext db)
        {
            _db = db;
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

        public void RemoveCredits(string userId, int amount)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);

            user.Credits -= amount;
            _db.SaveChanges();
        }
    }
}
