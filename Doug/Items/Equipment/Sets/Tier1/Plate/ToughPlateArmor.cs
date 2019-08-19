namespace Doug.Items.Equipment.Sets.Tier1.Plate
{
    public class ToughPlateArmor : EquipmentItem
    {
        public const string ItemId = "tough_armor";

        public ToughPlateArmor()
        {
            Id = ItemId;
            Name = "Tough Plate Armor";
            Description = "An armor made of thicc material.";
            Rarity = Rarity.Common;
            Icon = ":thick_armor:";
            Slot = EquipmentSlot.Body;
            Price = 669;
            LevelRequirement = 15;
            StrengthRequirement = 15;

            Stats.Defense = 20;
            Stats.Resistance = 8;
            Stats.HealthRegen = 1;
        }
    }
}