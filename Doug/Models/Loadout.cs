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

        public Loadout(string head, string body, string legs, string boots, string gloves, string leftHand, string rightHand, string neck)
        {
            Equipment = new Dictionary<EquipmentSlot, EquipmentItem>();

            AddEquipment(head);
            AddEquipment(body);
            AddEquipment(legs);
            AddEquipment(boots);
            AddEquipment(gloves);
            AddEquipment(leftHand);
            AddEquipment(rightHand);
            AddEquipment(neck);
        }

        private void AddEquipment(string itemId)
        {
            if (!string.IsNullOrEmpty(itemId))
            {
                var item = (EquipmentItem) ItemFactory.CreateItem(itemId);
                Equipment.Add(item.Slot, item);
            }
        }

        public int Luck => Equipment.Sum(equip => equip.Value.Luck);
        public int Agility => Equipment.Sum(equip => equip.Value.Agility);
        public int Charisma => Equipment.Sum(equip => equip.Value.Charisma);
        public int Constitution => Equipment.Sum(equip => equip.Value.Constitution);
        public int Stamina => Equipment.Sum(equip => equip.Value.Stamina);

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
