using Doug.Items;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IEquipmentRepository
    {
        void EquipItem(User user, EquipmentItem item);
        EquipmentItem UnequipItem(User user, EquipmentSlot slot);
        void DeleteEquippedItem(User user, EquipmentSlot slot);
    }

    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly DougContext _db;

        public EquipmentRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void EquipItem(User user, EquipmentItem item)
        {
            user.Loadout.Equip(item);

            _db.SaveChanges();
        }

        public EquipmentItem UnequipItem(User user, EquipmentSlot slot)
        {
            var equipment = user.Loadout.GetEquipmentAt(slot);

            user.Loadout.UnEquip(slot);

            _db.SaveChanges();

            return equipment;
        }

        public void DeleteEquippedItem(User user, EquipmentSlot slot)
        {
            user.Loadout.UnEquip(slot);

            _db.SaveChanges();
        }
    }
}
