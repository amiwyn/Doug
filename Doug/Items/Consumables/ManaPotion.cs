using Doug.Models;

namespace Doug.Items.Equipement
{
    public class ManaPotion : Item
    {
        public ManaPotion()
        {
            Name = "Energy Drink";
            Description = "Eww... this drink tastes bad but atleast give me some energy.";
            Rarity = Rarity.Common;
            Icon = ":tropical_drink:";
        }

        public override int OnConsuming(int mana)
        {
            return mana + 5;
        }
    }
}
