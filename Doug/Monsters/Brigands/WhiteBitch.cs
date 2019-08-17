using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models;

namespace Doug.Monsters.Brigands
{
    public class WhiteBitch : Monster
    {
        public const string MonsterId = "white_bitch";

        public WhiteBitch()
        {
            Id = MonsterId;
            Name = "St-Roch white bitch";
            Description = "Tellement de followers insta que j'invalide tes attaques :nail_care: ";
            Image = "https://pbs.twimg.com/media/DKBNrJEVAAAwLy4.jpg";
            Level = 18;
            ExperienceValue = 1250;

            MaxHealth = Health = 450;
            MinAttack = 102;
            MaxAttack = 160;
            Hitrate = 16;
            Dodge = 100;
            Defense = 42;
            Resistance = 10;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(new CoffeeCup(), 1), 0.25 },
                { new LootItem(new IronIngot(), 1), 0.05 },
                { new LootItem(new BikerCocaine(), 1), 0.15 }
            };
        }
    }
}
