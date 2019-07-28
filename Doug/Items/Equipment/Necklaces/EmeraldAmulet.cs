namespace Doug.Items.Equipment.Necklaces
{
    public class EmeraldAmulet : EquipmentItem
    {
        public const string ItemId = "emerald_amulet";

        public EmeraldAmulet()
        {
            Id = ItemId;
            Name = "Emerald Amulet";
            Description = "A shiny amulet";
            Rarity = Rarity.Common;
            Icon = ":necklace_1:";
            Slot = EquipmentSlot.Neck;
            Price = 3200;
            LevelRequirement = 10;

            Stats.Constitution = 4;
            Stats.Energy = 100;
            Stats.Health = 100;
        }
    }
}
