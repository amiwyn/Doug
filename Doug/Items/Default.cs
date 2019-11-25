using Doug.Items.WeaponType;

namespace Doug.Items
{
    public class Default : Weapon
    {
        public Default()
        {
            Id = "default";
            Name = "default_item";
            Description = "pablo, i said not to add that item yet!";
            Rarity = Rarity.Common;
            Icon = ":no_entry:";
        }
    }
}
