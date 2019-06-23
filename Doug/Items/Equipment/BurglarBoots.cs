namespace Doug.Items.Equipment
{
    public class BurglarBoots : EquipmentItem
    {
        public BurglarBoots()
        {
            Name = "Burglar Boots";
            Description = "The boots of a notorious burglar. Indubitably the best tool for a burglary. Increases the chances of success when stealing rupees.";
            Rarity = Rarity.Unique;
            Icon = ":boot:";
            Slot = EquipmentSlot.Boots;
        }

        public override double OnStealingChance(double chance)
        {
            return chance + 0.20;
        }
    }
}
