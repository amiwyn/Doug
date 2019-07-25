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

            Stats.MinAttack = 42;
            Stats.MaxAttack = 58;
            Stats.AttackSpeed = 2;
        }
    }
}
