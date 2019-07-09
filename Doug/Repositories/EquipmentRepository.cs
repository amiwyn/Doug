using System.Linq;
using Doug.Items;
using Doug.Models;
using Microsoft.EntityFrameworkCore;

namespace Doug.Repositories
{
    public interface IEquipmentRepository
    {
        void EquipItem(string userId, EquipmentItem item);
        EquipmentItem UnequipItem(string userId, EquipmentSlot slot);
    }

    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly DougContext _db;
        private readonly IItemFactory _itemFactory;

        public EquipmentRepository(DougContext dougContext, IItemFactory itemFactory)
        {
            _db = dougContext;
            _itemFactory = itemFactory;
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

            _db.SaveChanges();

            return equipment;
        }
    }
}
