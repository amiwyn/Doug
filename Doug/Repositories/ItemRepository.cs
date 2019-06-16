using Doug.Items;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IItemRepository
    {
        void AddItem(Item item);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly DougContext _db;

        public ItemRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void AddItem(Item item)
        {
            _db.Items.Add(item);
            _db.SaveChanges();
        }
    }
}
