using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Models;
using Microsoft.EntityFrameworkCore;

namespace Doug.Repositories
{
    public interface IInventoryRepository
    {
        void AddItem(string userId, string itemId);
        void RemoveItem(string userId, int inventoryPosition);
        void EquipItem(string userId, EquipmentItem item);
        EquipmentItem UnequipItem(string userId, EquipmentSlot slot);
        void AddItemToUsers(List<string> users, string itemId);
    }

    public class InventoryRepository : IInventoryRepository
    {
        private readonly DougContext _db;
        private readonly IItemFactory _itemFactory;

        public InventoryRepository(DougContext dougContext, IItemFactory itemFactory)
        {
            _db = dougContext;
            _itemFactory = itemFactory;
        }

        public void AddItem(string userId, string itemId)
        {
            AddItemToUser(userId, itemId);

            _db.SaveChanges();
        }

        private void AddItemToUser(string userId, string itemId)
        {
            var user = _db.Users
                .Include(usr => usr.InventoryItems)
                .Include(usr => usr.Loadout)
                .Single(usr => usr.Id == userId);

            user.LoadItems(_itemFactory);

            var item = user.InventoryItems.FirstOrDefault(itm => itm.ItemId == itemId);

            if (item != null && item.Quantity < item.Item.MaxStack)
            {
                item.Quantity++;
            }
            else
            {
                var slot = FindNextFreeSlot(user.InventoryItems);
                user.InventoryItems.Add(new InventoryItem(userId, itemId) { InventoryPosition = slot, Quantity = 1 });
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

        public void EquipItem(string userId, EquipmentItem item)
        {
            var user = _db.Users
                .Include(usr => usr.Loadout)
                .Single(usr => usr.Id == userId);

            user.LoadItems(_itemFactory);

            user.Loadout.Equip(item);

            _db.SaveChanges();
        }

        public EquipmentItem UnequipItem(string userId, EquipmentSlot slot)
        {
            var user = _db.Users
                .Include(usr => usr.Loadout)
                .Single(usr => usr.Id == userId);

            user.LoadItems(_itemFactory);

            var equipment = user.Loadout.GetEquipmentAt(slot);

            user.Loadout.UnEquip(slot);

            return equipment;
        }

        public void AddItemToUsers(List<string> users, string itemId)
        {
            users.ForEach(user => AddItemToUser(user, itemId));
            _db.SaveChanges();
        }
    }
}
