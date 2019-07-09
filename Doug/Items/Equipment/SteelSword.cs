namespace Doug.Items.Equipment
{
    public class SteelSword : EquipmentItem
    {
        public SteelSword()
        {
            Id = ItemFactory.SteelSword;
            Name = "Steel Sword";
            Description = "A durable sword. Light and Fast.";
            Rarity = Rarity.Common;
            Icon = ":dagger_knife:";
            Slot = EquipmentSlot.RightHand;
            Price = 65;
            LevelRequirement = 1;

            Attack = 18;
            Charisma = 2;
        }
    }
}
