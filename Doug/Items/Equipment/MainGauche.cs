namespace Doug.Items.Equipment
{
    public class MainGauche : EquipmentItem
    {
        public MainGauche()
        {
            Id = ItemFactory.MainGauche;
            Name = "Main Gauche";
            Description = "A dented, dulled brass shank. You won't be caught while /stealing, but any player that kills you steals this item.";
            Rarity = Rarity.Unique;
            Icon = ":dagger_knife:";
            Slot = EquipmentSlot.LeftHand;
            Price = 1000;

            Attack = 9;
            Agility = 2;
        }

        public override DougResponse OnStealingFailed(User user, string targetUserMention)
        {
            return "You failed to rob " + targetUserMention + ", but at least you didn't get caught... this time.";
        }

        public override DougResponse OnDeath()
        {
            return LoseItem();
        }
    }
}
