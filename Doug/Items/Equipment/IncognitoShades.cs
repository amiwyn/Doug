namespace Doug.Items.Equipment
{
    public class IncognitoShades : EquipmentItem
    {
        public const string ItemId = "incognito_shades";

        public IncognitoShades()
        {
            Id = ItemId;
            Name = "Incognito Shades";
            Description = "Really sick shades, you feel like wearing them even at night. +4 Luck, and you'll look too cool for Doug to recognize you.";
            Rarity = Rarity.Rare;
            Icon = ":dark_sunglasses:";
            Slot = EquipmentSlot.Neck;
            Price = 1990;
            LevelRequirement = 5;

            Stats.Luck = 4;
        }

        public override string OnMention(string mention)
        {
            return ":sunglasses:";
        }
    }
}
