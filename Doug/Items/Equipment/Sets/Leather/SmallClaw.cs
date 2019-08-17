using Doug.Items.WeaponType;

namespace Doug.Items.Equipment.Sets.Leather
{
    public class SmallClaw : Claws
    {
        public const string ItemId = "small_claw";

        public SmallClaw()
        {
            Id = ItemId;
            Name = "Small Claws";
            Description = "Watch out for bears.";
            Rarity = Rarity.Common;
            Icon = ":claw1:";
            Slot = EquipmentSlot.RightHand;
            Price = 233;
            LevelRequirement = 10;

            Stats.MinAttack = 42;
            Stats.MaxAttack = 58;
            Stats.AttackSpeed = 80;
        }
    }
}
