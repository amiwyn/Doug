using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    public interface ICoffeeRepository
    {
        void AddToRoster(string userId);
        void RemoveFromRoster(string userId);
        void SkipUser(string userId);
        void ConfirmUserReady(string userId);
        ICollection<string> GetReadyParticipants();
        ICollection<string> GetMissingParticipants();
        void ResetRoster();
        bool IsCoffeeBreak();
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

        public ICollection<string> GetReadyParticipants()
        {
            return _db.Roster.Where(user => !user.IsSkipping && user.IsReady).Select(user => user.Id).ToList();
        }

        public bool IsCoffeeBreak()
        {
            return _db.Channel.Single().IsCoffee;
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
