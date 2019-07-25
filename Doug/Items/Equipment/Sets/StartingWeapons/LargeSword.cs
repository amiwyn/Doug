namespace Doug.Items.Equipment.Sets.StartingWeapons
{
    public class LargeSword : Weapon
    {
        public const string ItemId = "large_sword";

        public LargeSword()
        {
            Id = ItemId;
            Name = "Large Sword";
            Description = "Now that's a big sword!";
            Rarity = Rarity.Common;
            Icon = ":heavy_sword1:";
            Slot = EquipmentSlot.RightHand;
            Price = 315;
            LevelRequirement = 10;
            IsDualWield = true;

            Stats.MinAttack = 38;
            Stats.MaxAttack = 70;
            Stats.Strength = 2;
        }
    }
}
