using Doug.Repositories;
using Doug.Services;
using Doug.Skills;
using Doug.Slack;

namespace Doug.Items.SkillBooks
{
    public class HealBook : SkillBook
    {
        public const string ItemId = "heal";

        public HealBook(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService)
        {
            Id = ItemId;
            Name = "Heal";
            Description = "Heal the target by a flat *30* Health. Cost 8 mana to cast.";
            Rarity = Rarity.Common;
            Icon = ":spellbook:";
            Price = 1200;
            LevelRequirement = 10;
            IntelligenceRequirement = 15;

            Skill = new Heal(statsRepository, slack, userService);
        }
    }
}
