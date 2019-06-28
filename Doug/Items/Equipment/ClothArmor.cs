namespace Doug.Items.Equipment
{
    public class ClothArmor : EquipmentItem
    {
        public ClothArmor()
        {
            Id = ItemFactory.ClothArmor;
            Name = "Cloth Armor";
            Description = "An armor made of an unknown textile. Protects against physical damage.";
            Rarity = Rarity.Common;
            Icon = ":shirt:";
            Slot = EquipmentSlot.Body;
            Price = 85;
        }
    }
}
