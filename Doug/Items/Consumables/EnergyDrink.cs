using Doug.Models;

namespace Doug.Items.Equipement
{
    public class EnergyDrink : Item
    {
        public EnergyDrink()
        {
            Name = "Energy Drink";
            Description = "Eww... this drink tastes bad but atleast give me some energy.";
            Rarity = Rarity.Common;
            Icon = ":tropical_drink:";
        }
    }
}
