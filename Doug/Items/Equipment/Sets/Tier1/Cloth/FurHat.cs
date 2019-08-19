namespace Doug.Items.Equipment.Sets.Tier1.Cloth
{
    public class FurHat : EquipmentItem
    {
        public const string ItemId = "fur_hat";

        public FurHat()
        {
            Id = ItemId;
            Name = "Fur hat";
            Description = "You feel 120% fancier.";
            Rarity = Rarity.Common;
            Icon = ":fur_hat:";
            Slot = EquipmentSlot.Head;
            Price = 250;
            LevelRequirement = 10;

            Stats.Defense = 4;
            Stats.Energy = 20;
        }
    }
}
