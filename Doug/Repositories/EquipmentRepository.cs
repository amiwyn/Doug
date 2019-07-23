using System.Collections.Generic;
using Doug.Items;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IEquipmentRepository
    {
        List<EquipmentItem> EquipItem(User user, EquipmentItem item);
        EquipmentItem UnequipItem(User user, EquipmentSlot slot);
    }

    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly DougContext _db;

        public EquipmentRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public List<EquipmentItem> EquipItem(User user, EquipmentItem item)
        {
            var equipment = user.Equip(item);

            _db.SaveChanges();

            return equipment;
        }

        public EquipmentItem UnequipItem(User user, EquipmentSlot slot)
        {
            var equipment = user.Loadout.UnEquip(slot);

            _db.SaveChanges();

            return equipment;
        }
    }
}
