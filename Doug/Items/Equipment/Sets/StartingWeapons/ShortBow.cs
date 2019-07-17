namespace Doug.Items.Equipment.Sets.StartingWeapons
{
    public class ShortBow : Weapon
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
            IsDualWield = true;

            Attack = 56;
        }
    }
}
