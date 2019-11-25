using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Items.WeaponType;

namespace Doug.Models.User
{
    public class Loadout
    {
        public string Id { get; set; }

        public string HeadId { get; set; }
        public EquipmentItem Head { get; set; }

        public string BodyId { get; set; }
        public EquipmentItem Body { get; set; }

        public string BootsId { get; set; }
        public EquipmentItem Boots { get; set; }

        public string GlovesId { get; set; }
        public EquipmentItem Gloves { get; set; }

        public string LeftHandId { get; set; }
        public EquipmentItem LeftHand { get; set; }

        public string RightHandId { get; set; }
        public EquipmentItem RightHand { get; set; }

        public string NeckId { get; set; }
        public EquipmentItem Neck { get; set; }

        public string LeftRingId { get; set; }
        public EquipmentItem LeftRing { get; set; }

        public string RightRingId { get; set; }
        public EquipmentItem RightRing { get; set; }

        public string SkillbookId { get; set; }
        public SkillBook Skillbook { get; set; }

        public void CreateEffects(IEquipmentEffectFactory equipmentEffectFactory)
        {
            Head?.CreateEffect(equipmentEffectFactory);
            Body?.CreateEffect(equipmentEffectFactory);
            Boots?.CreateEffect(equipmentEffectFactory);
            Gloves?.CreateEffect(equipmentEffectFactory);
            LeftHand?.CreateEffect(equipmentEffectFactory);
            RightHand?.CreateEffect(equipmentEffectFactory);
            Neck?.CreateEffect(equipmentEffectFactory);
            LeftRing?.CreateEffect(equipmentEffectFactory);
            RightRing?.CreateEffect(equipmentEffectFactory);
        }

        public int Sum(Func<EquipmentItem, int> stat)
        {
            return stat(Head ?? new EquipmentItem())
                   + stat(Body ?? new EquipmentItem())
                   + stat(Boots ?? new EquipmentItem())
                   + stat(Gloves ?? new EquipmentItem())
                   + stat(LeftHand ?? new EquipmentItem())
                   + stat(RightHand ?? new EquipmentItem())
                   + stat(Neck ?? new EquipmentItem())
                   + stat(LeftRing ?? new EquipmentItem())
                   + stat(RightRing ?? new EquipmentItem());
        }

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

            SetEquipmentAt(item.Slot, item);
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
            var equipment = GetEquipmentAt(slot);

            if (equipment == null)
            {
                return null;
            }

            SetEquipmentAt(slot, null);
            return equipment;
        }

        public EquipmentItem GetEquipmentAt(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Head: return Head;
                case EquipmentSlot.Body: return Body;
                case EquipmentSlot.Boots: return Boots;
                case EquipmentSlot.Gloves: return Gloves;
                case EquipmentSlot.LeftHand: return LeftHand;
                case EquipmentSlot.RightHand: return RightHand;
                case EquipmentSlot.Neck: return Neck;
                case EquipmentSlot.LeftRing: return LeftRing;
                case EquipmentSlot.RightRing: return RightRing;
                case EquipmentSlot.Skill: return Skillbook;
                default:
                    return default;
            }
        }

        public void SetEquipmentAt(EquipmentSlot slot, EquipmentItem value)
        {
            switch (slot)
            {
                case EquipmentSlot.Head:
                    Head = value;
                    HeadId = value?.Id;
                    break;
                case EquipmentSlot.Body:
                    Body = value;
                    BodyId = value?.Id;
                    break;
                case EquipmentSlot.Boots: 
                    Boots = value;
                    BootsId = value?.Id;
                    break;
                case EquipmentSlot.Gloves:
                    Gloves = value;
                    GlovesId = value?.Id;
                    break;
                case EquipmentSlot.LeftHand: 
                    LeftHand = value;
                    LeftHandId = value?.Id;
                    break;
                case EquipmentSlot.RightHand: 
                    RightHand = value;
                    RightHandId = value?.Id;
                    break;
                case EquipmentSlot.Neck:
                    Neck = value;
                    NeckId = value?.Id;
                    break;
                case EquipmentSlot.LeftRing:
                    LeftRing = value;
                    LeftRingId = value?.Id;
                    break;  
                case EquipmentSlot.RightRing:
                    RightRing = value;
                    RightRingId = value?.Id;
                    break;
                case EquipmentSlot.Skill:
                    Skillbook = (SkillBook)value;
                    SkillbookId = value?.Id;
                    break;
            }
        }

        public IEnumerable<string> GetDisplayEquipmentList()
        {
            return GetEquipmentList().Select(item => item.GetDisplayName());
        }

        public bool IsEmpty()
        {
            return HeadId == null &&
                   BodyId == null &&
                   BootsId == null &&
                   GlovesId == null &&
                   LeftHandId == null &&
                   RightHandId == null &&
                   NeckId == null &&
                   LeftRingId == null &&
                   RightRingId == null &&
                   SkillbookId == null;
        }

        public bool HasWeaponType(Type type)
        {
            return RightHand != null && RightHand.GetType().IsSubclassOf(type) ||
                   LeftHand != null && LeftHand.GetType().IsSubclassOf(type);
        }

        public List<EquipmentItem> GetEquipmentList()
        {
            return new List<EquipmentItem>
            {
                Head,
                Body,
                Boots,
                Gloves,
                LeftHand,
                RightHand,
                Neck,
                LeftRing,
                RightRing
            };
        }
    }
}
