using Doug.Repositories;
using Doug.Services;
using Doug.Skills;
using Doug.Slack;

namespace Doug.Items.SkillBooks
{
    public class LacerateBook : SkillBook
    {
        public const string ItemId = "lacerate";

        public LacerateBook(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService,
            ICombatService combatService, IEventDispatcher eventDispatcher)
        {
            Id = ItemId;
            Name = "Lacerate";
            Description = "A swift and crippling attack. Cost 20 mana to cast. Deal damage based on agility";
            Rarity = Rarity.Common;
            Icon = ":skillbook:";
            Price = 1200;
            LevelRequirement = 10;
            AgilityRequirement = 15;

            Skill = new Lacerate(statsRepository, slack, userService, combatService, eventDispatcher);
        }
    }
}
