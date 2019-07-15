namespace Doug.Items.Equipment.Sets.StartingWeapons
{
    public class WoodenStaff : Weapon
    {
        public const string ItemId = "wood_staff";

        public WoodenStaff()
        {
            Id = ItemId;
            Name = "Wooden Staff";
            Description = "A magical staff, still it looks like a piece of wood.";
            Rarity = Rarity.Common;
            Icon = ":staff1:";
            Slot = EquipmentSlot.RightHand;
            Price = 215;
            LevelRequirement = 10;
            IsDualWield = true;
            DamageType = DamageType.Magic;

            Attack = 24;
        }
    }
}
