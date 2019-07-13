namespace Doug.Items.Equipment
{
    public class StraightEdge : EquipmentItem
    {
        public StraightEdge()
        {
            Id = ItemFactory.StraightEdge;
            Name = "Straight Edge";
            Description = "No thanks, I'm straight edge.";
            Rarity = Rarity.Common;
            Icon = ":straight_edge:";
            Slot = EquipmentSlot.RightHand;
            Price = 1200;
            LevelRequirement = 20;

            Attack = 58;
            Constitution = 5;
        }
    }
}
