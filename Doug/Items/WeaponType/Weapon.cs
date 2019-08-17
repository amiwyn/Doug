using System.Collections.Generic;
using System.Linq;

namespace Doug.Items.WeaponType
{
    public abstract class Weapon : EquipmentItem
    {
        public bool IsDualWield { get; set; }

        protected Weapon()
        {
            Slot = EquipmentSlot.RightHand;
            Stats.AttackSpeed = 0;
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
