using System.Collections.Generic;
using System.Linq;
using Doug.Models.Combat;

namespace Doug.Items
{
    public abstract class Weapon : EquipmentItem
    {
        public bool IsDualWield { get; set; }
        public DamageType DamageType { get; set; }

        protected Weapon()
        {
            Stats.AttackSpeed = 100;
        }

        public override IEnumerable<string> GetDisplayAttributeList()
        {
            var itemSlot = Slot == EquipmentSlot.RightHand ? DougMessages.ItemRightHand : DougMessages.ItemLeftHand;

            return Stats.ToStringList()
                .Prepend(IsDualWield ? DougMessages.ItemDualWield : itemSlot)
                .Prepend(LevelRequirement == 0 ? string.Empty : string.Format(DougMessages.ItemLevel, LevelRequirement));
        }
    }
}
