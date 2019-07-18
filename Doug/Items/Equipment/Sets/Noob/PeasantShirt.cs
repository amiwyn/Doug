namespace Doug.Items.Equipment.Sets.Noob
{
    public class PeasantShirt : EquipmentItem
    {
        public const string ItemId = "peasant_shirt";

        public PeasantShirt()
        {
            Id = ItemId;
            Name = "Peasant Shirt";
            Description = "An armor made of leather. Protects against physical damage.";
            Rarity = Rarity.Common;
            Icon = ":noob_armor:";
            Slot = EquipmentSlot.Body;
            Price = 65;
            LevelRequirement = 1;

            Defense = 6;
            Resistance = 3;
        }
    }
}
