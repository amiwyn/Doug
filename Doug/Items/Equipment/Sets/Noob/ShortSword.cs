using Doug.Items.WeaponType;

namespace Doug.Items.Equipment.Sets.Noob
{
    public class ShortSword : Sword
    {
        public const string ItemId = "short_sword";

        public ShortSword()
        {
            Id = ItemId;
            Name = "Short Sword";
            Description = "A durable sword. Light and Fast.";
            Rarity = Rarity.Common;
            Icon = ":noob_sword2:";
            Slot = EquipmentSlot.RightHand;
            Price = 105;
            LevelRequirement = 5;

            Stats.MinAttack = 28;
            Stats.MaxAttack = 36;
        }
    }
}
