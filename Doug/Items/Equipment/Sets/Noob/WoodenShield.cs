namespace Doug.Items.Equipment.Sets.Noob
{
    public class WoodenShield : EquipmentItem
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

            Stats.Defense = 8;
            Stats.Resistance = 4;
        }
    }
}
