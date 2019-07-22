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
            LevelRequirement = 20;

            MinAttack = 69;
            MaxAttack = 69;
            Constitution = 4;
        }
    }
}
