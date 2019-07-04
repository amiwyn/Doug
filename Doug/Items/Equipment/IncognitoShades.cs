using Doug.models;

namespace Doug.Items.Equipment
{
    public class IncognitoShades : EquipmentItem
    {
        public IncognitoShades()
        {
            Id = ItemFactory.IncognitoShades;
            Name = "Incognito Shades";
            Description = "A pair of cool-looking sunglasses. Doug won't be able to recognize you while you wear these.";
            Rarity = Rarity.Unique;
            Icon = ":dark_sunglasses:";
            Slot = EquipmentSlot.Head;
            Price = 1337;

            Charisma = 1;
        }

        public override string OnMention(string userId)
        {
            return ":sunglasses:";
        }
    }
}
