using Doug.Repositories;
using Doug.Services;
using Doug.Skills;
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
            Id = ItemId;
            Name = "Heal";
            Description = "Steal rupees from the target. Cost 5 energy to cast.";
            Rarity = Rarity.Common;
            Icon = ":skillbook:";
            Price = 1200;
            LevelRequirement = 10;
            AgilityRequirement = 15;

            Skill = new Steal(statsRepository, slack, userService, channelRepository, eventDispatcher, randomService, creditsRepository);
        }
    }
}
