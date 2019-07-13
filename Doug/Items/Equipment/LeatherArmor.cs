namespace Doug.Items.Equipment
{
    public class LeatherArmor : EquipmentItem
    {
        public LeatherArmor()
        {
            Id = ItemFactory.LeatherArmor;
            Name = "Leather Armor";
            Description = "An armor made of leather. Protects against physical damage.";
            Rarity = Rarity.Common;
            Icon = ":armor1:";
            Slot = EquipmentSlot.Body;
            Price = 85;
            LevelRequirement = 1;

            Defense = 15;
        }
    }
}
