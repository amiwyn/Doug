using Doug.Models;
using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Microsoft.EntityFrameworkCore;

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
        private readonly IItemFactory _itemFactory;

        public CoffeeRepository(DougContext dougContext, IItemFactory itemFactory)
        {
            _db = dougContext;
            _itemFactory = itemFactory;
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

            var users = _db.Users.Where(usr => userIds.Contains(usr.Id))
                .Include(usr => usr.InventoryItems)
                .Include(usr => usr.Loadout)
                .ToList();

            users.ForEach(user => user.LoadItems(_itemFactory));
            return users;
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
            var channel = _db.Channel.Single();
            channel.IsCoffee = false;

            _db.SaveChanges();
        }

        public void StartCoffeeBreak()
        {
            var channel = _db.Channel.Single();
            channel.IsCoffee = true;

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
