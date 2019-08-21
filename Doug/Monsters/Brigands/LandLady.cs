using System.Collections.Generic;
using Doug.Items.Consumables;
using Doug.Items.Misc.Drops;
using Doug.Models;

namespace Doug.Monsters.Brigands
{
    public class LandLady : Monster
    {
        public const string MonsterId = "landlady";

        public LandLady()
        {
            Id = MonsterId;
            Name = "Vanier Land Lady";
            Description = "T'a pas payé ton loyer, viens voir maman pour la fessé";
            Image = "https://files.slack.com/files-pri/TAZF9C67N-FMBCJMUD6/image.png";
            Level = 30;
            ExperienceValue = 3000;

            MaxHealth = Health = 1979;
            MinAttack = 252;
            MaxAttack = 300;
            Hitrate = 15;
            Dodge = 55;
            Defense = 80;
            Resistance = 20;

            DropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(new CoffeeCup(), 1), 0.25 },
                { new LootItem(new IronIngot(), 1), 0.05 },
                { new LootItem(new AgilityReset(), 1), 0.01 },
                { new LootItem(new ConstitutionReset(), 1), 0.01 },
                { new LootItem(new IntelligenceReset(), 1), 0.01 },
                { new LootItem(new LuckReset(), 1), 0.01 },
                { new LootItem(new StrengthReset(), 1), 0.01 }
            
            };
        }
    }
}
