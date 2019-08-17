using Doug.Repositories;
using Doug.Services;
using Doug.Skills.Combat;
using Doug.Slack;

namespace Doug.Items.SkillBooks
{
    public class StealBook : SkillBook
    {
        public const string ItemId = "steal";

        public StealBook(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService,
            IChannelRepository channelRepository, IEventDispatcher eventDispatcher,
            IRandomService randomService, ICreditsRepository creditsRepository)
        {
            Skill = new Steal(statsRepository, slack, userService, channelRepository, eventDispatcher, randomService, creditsRepository);

            Id = ItemId;
            Name = "Steal";
            Description = $"Steal rupees from the target. Cost {Skill.EnergyCost} mana to cast.";
            Rarity = Rarity.Common;
            Icon = ":agi_skillbook:";
            Price = 1200;
            LevelRequirement = 10;
            AgilityRequirement = 15;
        }
    }
}
