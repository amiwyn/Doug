using Doug.Repositories;
using Doug.Services;
using Doug.Skills;
using Doug.Slack;

namespace Doug.Items.SkillBooks
{
    public class FireballBook : SkillBook
    {
        public const string ItemId = "fireball";

        public FireballBook(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService,
            ICombatService combatService, IEventDispatcher eventDispatcher)
        {
            Id = ItemId;
            Name = "Fireball";
            Description = "A blazing ball of fire. Cost 10 mana to cast.";
            Rarity = Rarity.Common;
            Icon = ":skillbook:";
            Price = 1200;
            LevelRequirement = 10;
            IntelligenceRequirement = 15;

            Skill = new Fireball(statsRepository, slack, userService, combatService, eventDispatcher);
        }
    }
}
