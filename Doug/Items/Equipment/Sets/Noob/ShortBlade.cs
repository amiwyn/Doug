namespace Doug.Items.Equipment.Sets.Noob
{
    public class ShortBlade : Weapon
    {
        public const string ItemId = "short_blade";

        public ShortBlade()
        {
            Id = ItemId;
            Name = "Short Blade";
            Description = "A durable sword. Light and Fast.";
            Rarity = Rarity.Common;
            Icon = ":noob_sword1:";
            Slot = EquipmentSlot.RightHand;
            Price = 65;
            LevelRequirement = 1;

            Attack = 16;
        }
    }
}
