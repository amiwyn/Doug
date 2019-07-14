namespace Doug.Items.Equipment
{
    public class BurglarBoots : EquipmentItem
    {
        public const string ItemId = "burglar_boots";

        public BurglarBoots()
        {
            Id = ItemId;
            Name = "Burglar Boots";
            Description = "The boots of a notorious burglar. Indubitably the best tool for a burglary. Increases the chances of success when stealing rupees.";
            Rarity = Rarity.Unique;
            Icon = ":burglar_boots:";
            Slot = EquipmentSlot.Boots;
            Price = 2625;
            LevelRequirement = 15;
        }

        public override double OnStealingChance(double chance)
        {
            return chance + 0.20;
        }
    }
}
