using Doug.Items.WeaponType;

namespace Doug.Items.Equipment.Sets.Tier1.Plate
{
    public class SmallAxe : Axe
    {
        public const string ItemId = "small_axe";

        public SmallAxe()
        {
            Id = ItemId;
            Name = "Small Axe";
            Description = "You can cut a tree?";
            Rarity = Rarity.Common;
            Icon = ":axe1:";
            Price = 255;
            LevelRequirement = 10;
            StrengthRequirement = 15;

            Stats.MinAttack = 44;
            Stats.MaxAttack = 58;
        }
    }
}
