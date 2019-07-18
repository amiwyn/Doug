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

        public int Health => Equipment.Sum(equip => equip.Value.Health);
        public int Energy => Equipment.Sum(equip => equip.Value.Energy);
        public int Luck => Equipment.Sum(equip => equip.Value.Luck);
        public int Agility => Equipment.Sum(equip => equip.Value.Agility);
        public int Strength => Equipment.Sum(equip => equip.Value.Strength);
        public int Constitution => Equipment.Sum(equip => equip.Value.Constitution);
        public int Intelligence => Equipment.Sum(equip => equip.Value.Intelligence);
        public int Attack => Equipment.Sum(equip => equip.Value.Attack);
        public int Defense => Equipment.Sum(equip => equip.Value.Defense);
        public int Dodge => Equipment.Sum(equip => equip.Value.Dodge);
        public int Hitrate => Equipment.Sum(equip => equip.Value.Hitrate);
        public double AttackSpeed => Equipment.Sum(equip => equip.Value.AttackSpeed);

        public List<EquipmentItem> Equip(EquipmentItem item)
        {
            var unequippedItems = new List<EquipmentItem>();
            var equipment = GetEquipmentAt(item.Slot);

            if (equipment != null)
            {
                unequippedItems.Add(UnEquip(item.Slot));
            }

            if (item is Weapon weapon && weapon.IsDualWield)
            {
                unequippedItems.Add(UnEquip(EquipmentSlot.LeftHand));
            }

            Equipment.Add(item.Slot, item);
            SetLoadoutStrings();
            return unequippedItems;
        }

        public EquipmentItem UnEquip(EquipmentSlot slot)
        {
            var equipment = Equipment.GetValueOrDefault(slot);

            if (equipment == null)
            {
                return null;
            }

            Equipment.Remove(slot);
            SetLoadoutStrings();
            return equipment;
        }

        private void SetLoadoutStrings()
        {
            Head = GetEquipmentAt(EquipmentSlot.Head)?.Id;
            Body = GetEquipmentAt(EquipmentSlot.Body)?.Id;
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

        public IEnumerable<string> GetDisplayEquipmentList()
        {
            return Equipment.Select(equipment => $"{equipment.Value.Icon} *{equipment.Value.Name}*");
        }

        public bool IsEmpty()
        {
            return Equipment.Count == 0;
        }

        public DamageType GetDamageType()
        {
            if (GetEquipmentAt(EquipmentSlot.RightHand) is Weapon weapon)
            {
                return weapon.DamageType;
            }
            return DamageType.Physical;
        }
    }
}
