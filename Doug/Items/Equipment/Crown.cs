namespace Doug.Items.Equipment
{
    public class Crown : EquipmentItem
    {
        public const string ItemId = "crown";

        public Crown()
        {
            Id = ItemId;
            Name = "Crown";
            Description = "Ye be a royals. Enjoy your power trip while ya head still stands on ya shoulders.";
            Rarity = Rarity.Legendary;
            Icon = ":emperor_crown:";
            Slot = EquipmentSlot.Head;
            Price = 90969469;
            LevelRequirement = 1;
            IsSellable = false;
            IsTradable = false;

            Stats.Strength = 5;
            Stats.Luck = 5;
            Stats.Agility = 5;
            Stats.Constitution = 5;
            Stats.Intelligence = 5;
            Stats.Health = 100;
        }
    }
}
