using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Commands
{
    public interface ICombatCommands
    {
        DougResponse Steal(Command command);
        Task<DougResponse> Attack(Command command);
    }

    public class CombatCommands : ICombatCommands
    {
        private const int StealEnergyCost = 1;
        private const int AttackEnergyCost = 1;
        private const int KillExperienceGain = 100;
        private static readonly DougResponse NoResponse = new DougResponse();

        private readonly IItemEventDispatcher _itemEventDispatcher;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IRandomService _randomService;
        private readonly IUserService _userService;
        private readonly IChannelRepository _channelRepository;

        public CombatCommands(IItemEventDispatcher itemEventDispatcher, IUserRepository userRepository, ISlackWebApi slack, IStatsRepository statsRepository, IRandomService randomService, IUserService userService, IChannelRepository channelRepository)
        {
            _itemEventDispatcher = itemEventDispatcher;
            _userRepository = userRepository;
            _slack = slack;
            _statsRepository = statsRepository;
            _randomService = randomService;
            _userService = userService;
            _channelRepository = channelRepository;
        }

        public DougResponse Steal(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());

            var energy = user.Energy - StealEnergyCost;

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            _statsRepository.UpdateEnergy(command.UserId, energy);

            var userChance = _itemEventDispatcher.OnStealingChance(user, user.BaseStealSuccessRate());
            var targetChance = _itemEventDispatcher.OnGettingStolenChance(target, target.BaseOpponentStealSuccessRate());

            var rollSuccessful = _randomService.RollAgainstOpponent(userChance, targetChance);

            var amount = _itemEventDispatcher.OnStealingAmount(user, user.BaseStealAmount());

            if (target.Credits - amount < 0)
            {
                amount = target.Credits;
            }

            if (rollSuccessful)
            {
                _userRepository.RemoveCredits(target.Id, amount);
                _userRepository.AddCredits(command.UserId, amount);

                var message = string.Format(DougMessages.StealCredits, _userService.Mention(user), amount, _userService.Mention(target));
                _slack.BroadcastMessage(message, command.ChannelId);
            }
            else
            {
                var message = string.Format(DougMessages.StealFail, _userService.Mention(user), _userService.Mention(target));
                _slack.BroadcastMessage(message, command.ChannelId);
            }

            return NoResponse;
        }

        public async Task<DougResponse> Attack(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());
            var energy = user.Energy - AttackEnergyCost;
            var damage = user.TotalAttack();

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            var channelType = _channelRepository.GetChannelType(command.ChannelId);

            if (channelType != ChannelType.Pvp)
            {
                return new DougResponse(DougMessages.NotInRightChannel);
            }

            _statsRepository.UpdateEnergy(command.UserId, energy);

            var message = string.Format(DougMessages.UserAttackedTarget, _userService.Mention(user), _userService.Mention(target), damage);
            await _slack.BroadcastMessage(message, command.ChannelId);

            await _userService.RemoveHealth(target, damage, command.ChannelId);

            if (target.IsDead())
            {
                await _userService.AddExperience(user, KillExperienceGain, command.ChannelId);
            }

            return NoResponse;
        }
    }
}
