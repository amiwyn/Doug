namespace Doug.Items.Equipment.Sets.Noob
{
    public class FarmersArmor : EquipmentItem
    {
        public const string ItemId = "farmers_armor";

        public FarmersArmor()
        {
            Id = ItemId;
            Name = "Farmer's Armor";
            Description = "An armor made of leather. Protects against physical damage.";
            Rarity = Rarity.Common;
            Icon = ":noob_armor:";
            Slot = EquipmentSlot.Body;
            Price = 155;
            LevelRequirement = 5;

            Defense = 10;
        }
    }
}
