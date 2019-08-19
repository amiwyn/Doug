namespace Doug.Items.Equipment.Sets.Tier1.Cloth
{
    public class ClothArmor : EquipmentItem
    {
        public const string ItemId = "cloth_armor";

        public ClothArmor()
        {
            Id = ItemId;
            Name = "Cloth Armor";
            Description = "An armor made of cloth.";
            Rarity = Rarity.Common;
            Icon = ":cloth_armor:";
            Slot = EquipmentSlot.Body;
            Price = 387;
            LevelRequirement = 10;
            IntelligenceRequirement = 15;

            Stats.Defense = 10;
            Stats.Resistance = 7;
            Stats.Energy = 40;
            Stats.Intelligence = 3;
            Stats.EnergyRegen = 2;

            Stats.Health = 1;
            Stats.Energy = 1;
            Stats.MaxAttack = 1;
            Stats.MinAttack = 1;
            Stats.AttackSpeed = 1;
            Stats.Hitrate = 1;
            Stats.Dodge = 1;
            Stats.Defense = 1;
            Stats.Resistance = 1;
            Stats.HealthRegen = 1;
            Stats.EnergyRegen = 1;
            Stats.Luck = 1;
            Stats.Agility = 1;
            Stats.Strength = 1;
            Stats.Constitution = 1;
            Stats.Intelligence = 1;
        }
    }
}
