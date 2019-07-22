namespace Doug.Items.Equipment.Sets.StartingWeapons
{
    public class SmallAxe : Weapon
    {
        public const string ItemId = "small_axe";

        public SmallAxe()
        {
            Id = ItemId;
            Name = "Small Axe";
            Description = "You can cut a tree?";
            Rarity = Rarity.Common;
            Icon = ":axe1:";
            Slot = EquipmentSlot.RightHand;
            Price = 255;
            LevelRequirement = 10;

            Attack = 46;
        }
    }
}
