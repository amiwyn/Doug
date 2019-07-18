namespace Doug.Items.Equipment.Sets.StartingWeapons
{
    public class SmallClaw : Weapon
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
            IsDualWield = true;

            Attack = 56;
            AttackSpeed = 2;
        }
    }
}
