using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Models.Combat;

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

        public int Health => Equipment.Sum(equip => equip.Value.Stats.Health);
        public int Energy => Equipment.Sum(equip => equip.Value.Stats.Energy);
        public int Luck => Equipment.Sum(equip => equip.Value.Stats.Luck);
        public int Agility => Equipment.Sum(equip => equip.Value.Stats.Agility);
        public int Strength => Equipment.Sum(equip => equip.Value.Stats.Strength);
        public int Constitution => Equipment.Sum(equip => equip.Value.Stats.Constitution);
        public int Intelligence => Equipment.Sum(equip => equip.Value.Stats.Intelligence);
        public int MaxAttack => Equipment.Sum(equip => equip.Value.Stats.MaxAttack);
        public int MinAttack => Equipment.Sum(equip => equip.Value.Stats.MinAttack);
        public int Defense => Equipment.Sum(equip => equip.Value.Stats.Defense);
        public int Dodge => Equipment.Sum(equip => equip.Value.Stats.Dodge);
        public int Hitrate => Equipment.Sum(equip => equip.Value.Stats.Hitrate);
        public int AttackSpeed => Equipment.Sum(equip => equip.Value.Stats.AttackSpeed);
        public int Resistance => Equipment.Sum(equip => equip.Value.Stats.Resistance);

        public List<EquipmentItem> Equip(EquipmentItem item)
        {
            var unequippedItems = new List<EquipmentItem>();
            var equipment = GetEquipmentAt(item.Slot);

            if (equipment != null)
            {
                unequippedItems.Add(UnEquip(item.Slot));
            }

            if (item.IsHandSlot())
            {
                unequippedItems.AddRange(EquipWeapon(item));
            }

            Equipment.Add(item.Slot, item);
            SetLoadoutStrings();
            return unequippedItems;
        }

        private List<EquipmentItem> EquipWeapon(EquipmentItem item)
        {
            var unequippedItems = new List<EquipmentItem>();

            if (item.Slot == EquipmentSlot.LeftHand)
            {
                if (GetEquipmentAt(EquipmentSlot.RightHand) is Weapon rightHand && rightHand.IsDualWield)
                {
                    unequippedItems.Add(UnEquip(EquipmentSlot.RightHand));
                }
            }

            if (item is Weapon weapon && weapon.IsDualWield)
            {
                var leftHandWeapon = UnEquip(EquipmentSlot.LeftHand);
                if (leftHandWeapon != null)
                {
                    unequippedItems.Add(leftHandWeapon);
                }
            }

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
