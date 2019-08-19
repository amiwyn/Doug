using Doug.Items.WeaponType;

namespace Doug.Items.Equipment.Sets.Noob
{
    public class WoodenShield : Shield
    {
        public const string ItemId = "wooden_shield";

        public WoodenShield()
        {
            Id = ItemId;
            Name = "Wooden Shield";
            Description = "A shield made of wood.";
            Rarity = Rarity.Common;
            Icon = ":noob_shield:";
            Price = 105;
            LevelRequirement = 5;

            Stats.Defense = 8;
            Stats.Resistance = 4;
        }
    }
}
