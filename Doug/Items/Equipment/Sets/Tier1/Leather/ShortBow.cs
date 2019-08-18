using Doug.Items.WeaponType;

namespace Doug.Items.Equipment.Sets.Tier1.Leather
{
    public class ShortBow : Bow
    {
        public const string ItemId = "short_bow";

        public ShortBow()
        {
            Id = ItemId;
            Name = "Short Bow";
            Description = "A bow. It shoots arrows.";
            Rarity = Rarity.Common;
            Icon = ":bow1:";
            Slot = EquipmentSlot.RightHand;
            Price = 233;
            LevelRequirement = 10;
            AgilityRequirement = 15;

            Stats.MinAttack = 40;
            Stats.MaxAttack = 60;
            Stats.AttackSpeed = 50;
            Stats.Hitrate = 10;
        }
    }
}
