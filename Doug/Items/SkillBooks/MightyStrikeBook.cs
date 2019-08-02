using Doug.Repositories;
using Doug.Services;
using Doug.Skills;
using Doug.Slack;

namespace Doug.Items.SkillBooks
{
    public class MightyStrikeBook : SkillBook
    {
        public const string ItemId = "mighty_strike";

        public MightyStrikeBook(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService,
            ICombatService combatService, IEventDispatcher eventDispatcher)
        {
            Skill = new MightyStrike(statsRepository, slack, userService, combatService, eventDispatcher);

            Id = ItemId;
            Name = "Mighty Strike";
            Description = $"_A powerful blow!_ Cost {Skill.EnergyCost} mana to cast. Deal damage based on strength";
            Rarity = Rarity.Common;
            Icon = ":str_skillbook:";
            Price = 1200;
            LevelRequirement = 10;
            StrengthRequirement = 15;
        }
    }
}
