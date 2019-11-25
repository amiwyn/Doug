using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Models.Coffee;
using Doug.Models.User;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Doug.Repositories
{
    public interface ICoffeeRepository
    {
        CoffeeBreak GetCoffeeBreak();
        void AddToRoster(string userId);
        void RemoveFromRoster(string userId);
        void SkipUser(string userId);
        void ConfirmUserReady(string userId);
        ICollection<User> GetReadyParticipants();
        ICollection<User> GetMissingParticipants();
        void ResetRoster();
        void EndCoffeeBreak();
        void StartCoffeeBreak();
        string GetRemindJob();
        void SetRemindJob(string jobId);
    }

    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly DougContext _db;
        private readonly IEquipmentEffectFactory _equipmentEffectFactory;

        public CoffeeRepository(DougContext dougContext, IEquipmentEffectFactory equipmentEffectFactory)
        {
            _db = dougContext;
            _equipmentEffectFactory = equipmentEffectFactory;
        }

        public CoffeeBreak GetCoffeeBreak()
        {
            return _db.CoffeeBreak.Single();
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

        public ICollection<User> GetMissingParticipants()
        {
            var userIds = _db.Roster.Where(user => !user.IsSkipping && !user.IsReady).Select(user => user.Id).ToList();

            var users = _db.Users.Where(usr => userIds.Contains(usr.Id))
                .Include(usr => usr.Effects)
                .Include(usr => usr.InventoryItems)
                .ThenInclude(itm => itm.Item)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Head)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Body)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Boots)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Gloves)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.LeftHand)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.RightHand)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Neck)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.LeftRing)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.RightRing)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Skillbook)
                .ToList();

            users.ForEach(user => user.Effects = user.Effects.Where(effect => effect.EndTime >= DateTime.UtcNow).ToList());

            users.ForEach(user => user.LoadItems(_equipmentEffectFactory));
            return users;
        }

        public ICollection<User> GetReadyParticipants()
        {
            var userIds = _db.Roster.Where(user => !user.IsSkipping && user.IsReady).Select(user => user.Id).ToList();

            var users = _db.Users.Where(usr => userIds.Contains(usr.Id))
                .Include(usr => usr.Effects)
                .Include(usr => usr.InventoryItems)
                .ThenInclude(itm => itm.Item)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Head)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Body)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Boots)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Gloves)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.LeftHand)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.RightHand)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Neck)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.LeftRing)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.RightRing)
                .Include(usr => usr.Loadout)
                .ThenInclude(itm => itm.Skillbook)
                .ToList();

            users.ForEach(user => user.Effects = user.Effects.Where(effect => effect.EndTime >= DateTime.UtcNow).ToList());

            users.ForEach(user => user.LoadItems(_equipmentEffectFactory));
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

        public void EndCoffeeBreak()
        {
            var channel = _db.CoffeeBreak.Single();
            channel.IsCoffeeBreak = false;

            _db.SaveChanges();
        }

        public void StartCoffeeBreak()
        {
            var channel = _db.CoffeeBreak.Single();
            var coffee = _db.CoffeeBreak.Single();
            channel.IsCoffeeBreak = true;
            coffee.LastCoffee = DateTime.UtcNow;

            _db.SaveChanges();
        }

        public string GetRemindJob()
        {
            return _db.CoffeeBreak.Single().CoffeeRemindJobId;
        }

        public void SetRemindJob(string jobId)
        {
            _db.CoffeeBreak.Single().CoffeeRemindJobId = jobId;
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
