namespace Doug.Items.Equipment.Sets.Tier2.Fighter
{
    public class FighterHelm : EquipmentItem
    {
        public const string ItemId = "fighter_helm";

        public FighterHelm()
        {
            Id = ItemId;
            Name = "Fighter Helm";
            Description = "Attention aux bicyles";
            Rarity = Rarity.Common;
            Icon = ":fighter_helm:";
            Slot = EquipmentSlot.Head;
            Price = 712;
            LevelRequirement = 20;
            StrengthRequirement = 20;

            Stats.Defense = 6;
            Stats.Agility = 1;
            Stats.Constitution = 2;
        }
    }
}