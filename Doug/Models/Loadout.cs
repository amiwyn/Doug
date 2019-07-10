using System.Collections.Generic;
using System.Linq;
using Doug.Items;

namespace Doug.Models
{
    public class Loadout
    {
        public string Id { get; set; }
        public string Head { get; set; }
        public string Body { get; set; }
        public string Legs { get; set; }
        public string Boots { get; set; }
        public string Gloves { get; set; }
        public string LeftHand { get; set; }
        public string RightHand { get; set; }
        public string Neck { get; set; }

        public Dictionary<EquipmentSlot, EquipmentItem> Equipment { get; }

        public Loadout()
        {
            Equipment = new Dictionary<EquipmentSlot, EquipmentItem>();
        }

        public void CreateEquipment(IItemFactory itemFactory)
        {
            AddEquipment(Head, itemFactory);
            AddEquipment(Body, itemFactory);
            AddEquipment(Legs, itemFactory);
            AddEquipment(Boots, itemFactory);
            AddEquipment(Gloves, itemFactory);
            AddEquipment(LeftHand, itemFactory);
            AddEquipment(RightHand, itemFactory);
            AddEquipment(Neck, itemFactory);
        }

        private void AddEquipment(string itemId, IItemFactory itemFactory)
        {
            if (!string.IsNullOrEmpty(itemId))
            {
                var item = (EquipmentItem) itemFactory.CreateItem(itemId);
                Equipment.TryAdd(item.Slot, item);
            }
        }

        public void DeleteEquipment(EquipmentSlot slot)
        {
            Equipment.Remove(slot);
            SetLoadoutStrings();
        }

        public int Luck => Equipment.Sum(equip => equip.Value.Luck);
        public int Agility => Equipment.Sum(equip => equip.Value.Agility);
        public int Charisma => Equipment.Sum(equip => equip.Value.Charisma);
        public int Constitution => Equipment.Sum(equip => equip.Value.Constitution);
        public int Stamina => Equipment.Sum(equip => equip.Value.Stamina);
        public int Attack => Equipment.Sum(equip => equip.Value.Attack);

        public void Equip(EquipmentItem item)
        {
            Equipment.Add(item.Slot, item);
            SetLoadoutStrings();
        }

        public void UnEquip(EquipmentSlot slot)
        {
            var equipment = Equipment.GetValueOrDefault(slot);

            if (equipment == null)
            {
                return;
            }

            Equipment.Remove(slot);
            SetLoadoutStrings();
        }

        private void SetLoadoutStrings()
        {
            Head = GetEquipmentAt(EquipmentSlot.Head)?.Id;
            Body = GetEquipmentAt(EquipmentSlot.Body)?.Id;
            Legs = GetEquipmentAt(EquipmentSlot.Legs)?.Id;
            Boots = GetEquipmentAt(EquipmentSlot.Boots)?.Id;
            Gloves = GetEquipmentAt(EquipmentSlot.Gloves)?.Id;
            LeftHand = GetEquipmentAt(EquipmentSlot.LeftHand)?.Id;
            RightHand = GetEquipmentAt(EquipmentSlot.RightHand)?.Id;
            Neck = GetEquipmentAt(EquipmentSlot.Neck)?.Id;
        }

        public EquipmentItem GetEquipmentAt(EquipmentSlot slot)
        {
            return Equipment.GetValueOrDefault(slot);
        }
    }
}
