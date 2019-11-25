using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Effects;
using Doug.Items;
using Doug.Models.User;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Doug.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string userId);
        List<User> GetUsers();
        User GetUser(string userId);
        void RegenerateUsers();
    }

    public class UserRepository : IUserRepository
    {
        private readonly DougContext _db;
        private readonly IEquipmentEffectFactory _equipmentEffectFactory;
        private readonly IEffectFactory _effectFactory;

        public UserRepository(DougContext dougContext, IEquipmentEffectFactory equipmentEffectFactory, IEffectFactory effectFactory)
        {
            _db = dougContext;
            _equipmentEffectFactory = equipmentEffectFactory;
            _effectFactory = effectFactory;
        }

        public void AddUser(string userId)
        {
            if (!_db.Users.Any(user => user.Id == userId))
            {

                var user = new User
                {
                    Id = userId,
                    Credits = 10,
                    Loadout = new Loadout { Id = userId }
                };

                _db.Users.Add(user);
                _db.SaveChanges();
            }
        }

        public User GetUser(string userId)
        {
            var user = _db.Users
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
                .Single(usr => usr.Id == userId);

            user.Effects = user.Effects.Where(effect => effect.EndTime >= DateTime.UtcNow).ToList();

            user.LoadItems(_equipmentEffectFactory);
            user.LoadEffects(_effectFactory);

            return user;
        }

        public void RegenerateUsers()
        {
            var users = GetUsers();

            users.ForEach(user => user.RegenerateHealthAndEnergy());

            _db.SaveChanges();
        }

        public List<User> GetUsers()
        {
            var users = _db.Users
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

            users.ForEach(usr => usr.LoadItems(_equipmentEffectFactory));
            users.ForEach(usr => usr.LoadEffects(_effectFactory));

            return users;
        }
    }
}
