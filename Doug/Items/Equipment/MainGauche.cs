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

        public override DougResponse OnStealingFailed(string response, string targetUserMention)
        {
            return "Someone failed to rob " + targetUserMention + ", but they were too quick to get caught... this time.";
        }

        // NYI
        //public override DougResponse OnDeath(string response, User user, User killer)
        //{
        //    return LoseItem();
        //}
    }
}
