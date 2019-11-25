using System.Collections.Generic;
using System.Linq;
using Doug.Items;

namespace Doug.Repositories
{
    public interface IItemRepository
    {
        Item GetItem(string itemId);
        List<Item> GetItems(List<string> items);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly DougContext _db;

        public ItemRepository(DougContext db)
        {
            _db = db;
        }

        public Item GetItem(string itemId)
        {
            return _db.Items.Single(item => item.Id == itemId);
        }

        public List<Item> GetItems(List<string> items)
        {
            return _db.Items.Where(item => items.Contains(item.Id)).ToList();
        }
    }
}
