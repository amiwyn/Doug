using Doug.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Doug.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string userId);
        List<User> GetUsers();
        User GetUser(string userId);
        void SaveUser(User user);
        void RemoveCredits(string userId, int amount);
        void AddCredits(string userId, int amount);
        void AddItem(string userId, string itemId);
        void RemoveItem(string userId, int inventoryPosition);
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

        public void AddItem(string userId, string itemId)
        {
            var user = _db.Users
                .Include(usr => usr.InventoryItems)
                .Single(usr => usr.Id == userId);

            var item = user.InventoryItems.FirstOrDefault(itm => itm.ItemId == itemId);

            if (item != null && item.Quantity < item.Item.MaxStack)
            {
                item.Quantity++;
            }
            else
            {
                var slot = FindNextFreeSlot(user.InventoryItems);
                user.InventoryItems.Add(new InventoryItem(userId, itemId) {InventoryPosition = slot, Quantity = 1});
            }

            _db.SaveChanges();
        }

        private int FindNextFreeSlot(List<InventoryItem> items)
        {
            var slots = items.Select(itm => itm.InventoryPosition).ToList();

            if (!slots.Any())
            {
                return 0;
            }

            var maxPos = slots.Any() ? slots.Max() : 0;
            var freeSlots = Enumerable.Range(0, maxPos).Except(slots).ToList();

            return freeSlots.Any() ? freeSlots.First() : maxPos + 1;
        } 

        public void RemoveItem(string userId, int inventoryPosition)
        {
            var user = _db.Users.Single(usr => usr.Id == userId);
            var item = user.InventoryItems.Single(itm => itm.InventoryPosition == inventoryPosition);

            if (item.Quantity <= 1)
            {
                user.InventoryItems.Remove(item);
            }
            else
            {
                item.Quantity--;
            }

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
                .Include(user => user.InventoryItems)
                .Single(user => user.Id == userId);
        }

        public void SaveUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
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
