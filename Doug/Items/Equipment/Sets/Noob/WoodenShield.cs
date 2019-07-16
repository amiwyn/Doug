namespace Doug.Items.Equipment.Sets.Noob
{
    public class WoodenShield : Weapon
    {
        public const string ItemId = "wooden_shield";

        public WoodenShield()
        {
            Id = ItemId;
            Name = "Wooden Shield";
            Description = "A shield made of wood.";
            Rarity = Rarity.Common;
            Icon = ":noob_shield:";
            Slot = EquipmentSlot.LeftHand;
            Price = 105;
            LevelRequirement = 5;

            Defense = 10;
        }
    }
}
