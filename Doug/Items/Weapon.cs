using System.Collections.Generic;
using System.Linq;

namespace Doug.Items
{
    public class Weapon : EquipmentItem
    {
        public bool IsDualWield { get; set; }
        public DamageType DamageType { get; set; }

        public override IEnumerable<string> GetDisplayAttributeList()
        {
            var itemSlot = Slot == EquipmentSlot.RightHand ? DougMessages.ItemRightHand : DougMessages.ItemLeftHand;

            return GetStatsAttributesList()
                .Prepend(IsDualWield ? DougMessages.ItemDualWield : itemSlot)
                .Prepend(DisplayAttribute(DougMessages.ItemLevel, LevelRequirement));
        }
    }
}
