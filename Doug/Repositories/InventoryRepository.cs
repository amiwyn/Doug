﻿using System.Collections.Generic;
using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IInventoryRepository
    {
        void AddItem(User user, string itemId);
        void AddItems(User user, IEnumerable<string> items);
        void RemoveItem(User user, int inventoryPosition);
        void AddItemToUsers(List<User> users, string itemId);
    }

    public class InventoryRepository : IInventoryRepository
    {
        private readonly DougContext _db;

        public InventoryRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void AddItem(User user, string itemId)
        {
            AddItemToUser(user, itemId);

            _db.SaveChanges();
        }

        public void AddItems(User user, IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                AddItemToUser(user, item);
            }

            _db.SaveChanges();
        }

        private void AddItemToUser(User user, string itemId)
        {
            var itemStack = new Stack<InventoryItem>(user.InventoryItems.Where(itm => itm.ItemId == itemId).Reverse());

            InventoryItem item;
            while (itemStack.TryPop(out item) && item.Quantity >= item.Item.MaxStack) { }

            if (item != null && item.Quantity < item.Item.MaxStack)
            {
                item.Quantity++;
            }
            else
            {
                var slot = FindNextFreeSlot(user.InventoryItems);
                user.InventoryItems.Add(new InventoryItem(user.Id, itemId) { InventoryPosition = slot, Quantity = 1 });
            }
        }

        private int FindNextFreeSlot(List<InventoryItem> items)
        {
            var slots = items.Select(itm => itm.InventoryPosition).ToList();

            if (!slots.Any())
            {
                return 0;
            }

            var maxPos = slots.Max();
            var freeSlots = Enumerable.Range(0, maxPos).Except(slots).ToList();

            return freeSlots.Any() ? freeSlots.First() : maxPos + 1;
        }

        public void RemoveItem(User user, int inventoryPosition)
        {
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

        public void AddItemToUsers(List<User> users, string itemId)
        {
            users.ForEach(user => AddItemToUser(user, itemId));
            _db.SaveChanges();
        }
    }
}
