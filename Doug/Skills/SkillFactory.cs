using Doug.Effects;
using Doug.Repositories;
using Doug.Services;
using Doug.Skills.Combat;
using Doug.Skills.Utility;
using Doug.Slack;

namespace Doug.Skills
{
    public interface ISkillFactory
    {
        Skill CreateSkill(string skillId);
    }

    public class SkillFactory : ISkillFactory
    {
        private readonly IUserService _userService;
        private readonly IStatsRepository _statsRepository;
        private readonly ISlackWebApi _slack;
        private readonly ICombatService _combatService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IChannelRepository _channelRepository;
        private readonly IRandomService _randomService;
        private readonly ICreditsRepository _creditsRepository;

        public SkillFactory(IUserService userService, IStatsRepository statsRepository, ISlackWebApi slack, ICombatService combatService, IEventDispatcher eventDispatcher, IChannelRepository channelRepository, IRandomService randomService, ICreditsRepository creditsRepository)
        {
            _userService = userService;
            _statsRepository = statsRepository;
            _slack = slack;
            _combatService = combatService;
            _eventDispatcher = eventDispatcher;
            _channelRepository = channelRepository;
            _randomService = randomService;
            _creditsRepository = creditsRepository;
        }

        public Skill CreateSkill(string skillId)
        {
            switch (skillId)
            {
                case Fireball.SkillId: return new Fireball(_statsRepository, _slack, _userService, _combatService, _eventDispatcher, _channelRepository);
                case Lacerate.SkillId: return new Lacerate(_statsRepository, _slack, _userService, _combatService, _eventDispatcher, _channelRepository);
                case MightyStrike.SkillId: return new MightyStrike(_statsRepository, _slack, _userService, _combatService, _eventDispatcher, _channelRepository);
                case Steal.SkillId: return new Steal(_statsRepository, _slack, _userService, _channelRepository, _eventDispatcher, _randomService, _creditsRepository);
                case Heal.SkillId: return new Heal(_statsRepository, _slack, _userService);
                default: return new Skill(_statsRepository);
            }
        }
    }
}
