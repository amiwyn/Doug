namespace Doug.Items.Equipment
{
    public class StraightEdge : EquipmentItem
    {
        public const string ItemId = "straight_edge";

        public StraightEdge()
        {
            Id = ItemId;
            Name = "Straight Edge";
            Description = "No thanks, I'm straight edge.";
            Rarity = Rarity.Common;
            Icon = ":straight_edge:";
            Slot = EquipmentSlot.RightHand;
            Price = 820;
            LevelRequirement = 1;

            Stats.MinAttack = 69;
            Stats.MaxAttack = 69;
            Stats.Constitution = 4;
        }
    }
}
