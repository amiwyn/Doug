using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface ICombatService
    {
        Task<DougResponse> Steal(User user, User target, string channel);
        Task<DougResponse> Attack(User user, User target, string channel);
    }

    public class CombatService : ICombatService
    {
        private const int StealEnergyCost = 1;
        private const int AttackEnergyCost = 1;
        private const int KillExperienceGain = 100;

        private readonly IEventDispatcher _eventDispatcher;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IRandomService _randomService;
        private readonly IUserService _userService;
        private readonly IChannelRepository _channelRepository;

        public CombatService(IEventDispatcher eventDispatcher, IUserRepository userRepository, ISlackWebApi slack, IStatsRepository statsRepository, IRandomService randomService, IUserService userService, IChannelRepository channelRepository)
        {
            _eventDispatcher = eventDispatcher;
            _userRepository = userRepository;
            _slack = slack;
            _statsRepository = statsRepository;
            _randomService = randomService;
            _userService = userService;
            _channelRepository = channelRepository;
        }

        public async Task<DougResponse> Steal(User user, User target, string channel)
        {
            var userIsActive = _userService.IsUserActive(user.Id);
            var targetIsActive = _userService.IsUserActive(target.Id);

            if (user.IsStealOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateStealCooldownRemaining()));
            }

            var channelType = _channelRepository.GetChannelType(channel);

            if (channelType != ChannelType.Common && channelType != ChannelType.Pvp)
            {
                return new DougResponse(DougMessages.NotInRightChannel);
            }

            var energy = user.Energy - StealEnergyCost;

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            if (!await targetIsActive)
            {
                return new DougResponse(DougMessages.UserMustBeActive);
            }

            if (!await userIsActive)
            {
                return new DougResponse(DougMessages.YouMustBeActive);
            }

            _statsRepository.UpdateEnergy(user.Id, energy);
            _userRepository.SetStealCooldown(user.Id, user.GetStealCooldown());

            var userChance = _eventDispatcher.OnStealingChance(user, user.BaseStealSuccessRate());
            var targetChance = _eventDispatcher.OnGettingStolenChance(target, target.BaseOpponentStealSuccessRate());

            var rollSuccessful = _randomService.RollAgainstOpponent(userChance, targetChance);

            var amount = _eventDispatcher.OnStealingAmount(user, user.BaseStealAmount());

            if (target.Credits - amount < 0)
            {
                amount = target.Credits;
            }

            if (rollSuccessful)
            {
                _userRepository.RemoveCredits(target.Id, amount);
                _userRepository.AddCredits(user.Id, amount);

                var message = string.Format(DougMessages.StealCredits, _userService.Mention(user), amount, _userService.Mention(target));
                await _slack.BroadcastMessage(message, channel);
            }
            else
            {
                var message = string.Format(DougMessages.StealFail, _userService.Mention(user), _userService.Mention(target));
                await _slack.BroadcastMessage(message, channel);
            }

            return new DougResponse();
        }

        public async Task<DougResponse> Attack(User user, User target, string channel)
        {
            var energy = user.Energy - AttackEnergyCost;

            if (user.IsAttackOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateAttackCooldownRemaining()));
            }

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            var channelType = _channelRepository.GetChannelType(channel);

            //if (channelType != ChannelType.Pvp)
            //{
            //    return new DougResponse(DougMessages.NotInRightChannel);
            //}

            var flaggedUsers = await _slack.GetUsersInChannel(channel);

            if (flaggedUsers.All(usr => usr != target.Id))
            {
                return new DougResponse(DougMessages.UserIsNotInPvp);
            }

            _statsRepository.UpdateEnergy(user.Id, energy);
            _userRepository.SetAttackCooldown(user.Id, user.GetAttackCooldown());

            if (_eventDispatcher.OnAttackedInvincibility(user, target))
            {
                return new DougResponse(DougMessages.UserIsInvincible);
            }

            await DealDamage(user, target, channel);

            return new DougResponse();
        }

        private async Task DealDamage(User user, User target, string channel)
        {
            var attackStatus = user.AttackUser(target, out var damageDealt, _eventDispatcher);

            var message = attackStatus.ToMessage(_userService.Mention(user), _userService.Mention(target), damageDealt);

            await _slack.BroadcastMessage(message, channel);

            if (target.IsDead() && await _userService.HandleDeath(target, channel))
            {
                _eventDispatcher.OnDeathByUser(target, user);
                await _userService.AddExperience(user, KillExperienceGain, channel);
            }

            _statsRepository.UpdateHealth(target.Id, target.Health);
        }
    }
}
