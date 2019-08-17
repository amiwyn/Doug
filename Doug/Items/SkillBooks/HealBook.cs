using Doug.Repositories;
using Doug.Services;
using Doug.Skills.Utility;
using Doug.Slack;

namespace Doug.Items.SkillBooks
{
    public class HealBook : SkillBook
    {
        public const string ItemId = "heal";

        public HealBook(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService)
        {
            Skill = new Heal(statsRepository, slack, userService);

            Id = ItemId;
            Name = "Heal";
            Description = $"Heal the target. Health gained depends on the caster's level. Cost {Skill.EnergyCost} mana to cast.";
            Rarity = Rarity.Common;
            Icon = ":int_skillbook:";
            Price = 1200;
            LevelRequirement = 10;
            IntelligenceRequirement = 15;
        }
    }
}
