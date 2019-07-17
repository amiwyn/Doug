using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IInventoryRepository
    {
        void AddItem(User user, Item item);
        void AddItems(User user, IEnumerable<Item> items);
        void RemoveItem(User user, int inventoryPosition);
        void AddItemToUsers(List<User> users, Item item);
    }

    public class InventoryRepository : IInventoryRepository
    {
        private readonly DougContext _db;

        public InventoryRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void AddItem(User user, Item item)
        {
            AddItemToUser(user, item);

            _db.SaveChanges();
        }

        public void AddItems(User user, IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                AddItemToUser(user, item);
            }

            _db.SaveChanges();
        }

        private void AddItemToUser(User user, Item item)
        {
            var currentStacks = user.InventoryItems.Where(inv => inv.ItemId == item.Id).ToList();

            if (currentStacks.Any())
            {
                var freeStacks = currentStacks.Where(inv => inv.Quantity < inv.Item.MaxStack).ToList();

                if (freeStacks.Any())
                {
                    freeStacks.First().Quantity++;
                }
            }

            var slot = FindNextFreeSlot(user.InventoryItems);
            user.InventoryItems.Add(new InventoryItem(user.Id, item.Id) { InventoryPosition = slot, Quantity = 1, Item = item });
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

        public void AddItemToUsers(List<User> users, Item item)
        {
            users.ForEach(user => AddItemToUser(user, item));
            _db.SaveChanges();
        }
    }
}
