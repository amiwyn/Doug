using Doug.Items.WeaponType;

namespace Doug.Items.Equipment.Sets.Tier1.Plate
{
    public class PlateShield : Shield
    {
        public const string ItemId = "plate_shield";

        public PlateShield()
        {
            Id = ItemId;
            Name = "Plate Shield";
            Description = "A shield made of metal.";
            Rarity = Rarity.Common;
            Icon = ":shield_2:";
            Price = 475;
            LevelRequirement = 15;

            Stats.Defense = 12;
            Stats.Resistance = 5;
        }
    }
}
