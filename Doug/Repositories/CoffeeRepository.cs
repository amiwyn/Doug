using Doug.Models;
using System.Collections.Generic;
using System.Linq;

namespace Doug.Repositories
{
    public interface ICoffeeRepository
    {
        void AddToRoster(string userId);
        void RemoveFromRoster(string userId);
        void SkipUser(string userId);
        void ConfirmUserReady(string userId);
        ICollection<User> GetReadyParticipants();
        ICollection<string> GetMissingParticipants();
        void ResetRoster();
        bool IsCoffeeBreak();
        void EndCoffeeBreak();
        void StartCoffeeBreak();
    }

    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly DougContext _db;

        public CoffeeRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void AddToRoster(string userId)
        {
            if (!_db.Roster.Any(user => user.Id == userId))
            {
                _db.Roster.Add(new Roster() { Id = userId });
                _db.SaveChanges();
            }
        }

        public void ConfirmUserReady(string userId)
        {
            var user = _db.Roster.SingleOrDefault(usr => usr.Id == userId);
            if (user != null)
            {
                user.IsReady = true;
                _db.SaveChanges();
            }
        }

        public ICollection<string> GetMissingParticipants()
        {
            return _db.Roster.Where(user => !user.IsSkipping && !user.IsReady).Select(user => user.Id).ToList();
        }

        public ICollection<User> GetReadyParticipants()
        {
            var userIds = _db.Roster.Where(user => !user.IsSkipping && user.IsReady).Select(user => user.Id).ToList();

            return _db.Users.Where(usr => userIds.Contains(usr.Id)).ToList();
        }

        public void RemoveFromRoster(string userId)
        {
            var user = _db.Roster.SingleOrDefault(usr => usr.Id == userId);
            if (user != null)
            {
                _db.Roster.Remove(user);
                _db.SaveChanges();
            }
        }

        public void ResetRoster()
        {
            var participants = _db.Roster.ToList();
            participants.ForEach(participant =>
            {
                participant.IsReady = false;
                participant.IsSkipping = false;
            });
            _db.SaveChanges();
        }

        public bool IsCoffeeBreak()
        {
            return _db.Channel.Single().IsCoffee;
        }

        public void EndCoffeeBreak()
        {
            _db.Channel.Single().IsCoffee = false;
            _db.SaveChanges();
        }

        public void StartCoffeeBreak()
        {
            _db.Channel.Single().IsCoffee = true;
            _db.SaveChanges();
        }

        public void SkipUser(string userId)
        {
            var user = _db.Roster.SingleOrDefault(usr => usr.Id == userId);
            if (user != null)
            {
                user.IsSkipping = true;
                _db.SaveChanges();
            }
        }
    }
}
