namespace Doug.Items.Equipment.Sets.StartingWeapons
{
    public class LightSword : EquipmentItem
    {
        public const string ItemId = "light_sword";

        public LightSword()
        {
            Id = ItemId;
            Name = "Light Sword";
            Description = "A sword made of lightweight material.";
            Rarity = Rarity.Common;
            Icon = ":sword1:";
            Slot = EquipmentSlot.RightHand;
            Price = 210;
            LevelRequirement = 10;

            Attack = 42;
        }
    }
}
