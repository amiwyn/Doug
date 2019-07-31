using System;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills
{
    public class Steal : Skill
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IChannelRepository _channelRepository;
        private readonly IStatsRepository _statsRepository;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IRandomService _randomService;
        private readonly ICreditsRepository _creditsRepository;

        public Steal(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService, IChannelRepository channelRepository, IEventDispatcher eventDispatcher, IRandomService randomService, ICreditsRepository creditsRepository) : base(statsRepository)
        {
            _statsRepository = statsRepository;
            _slack = slack;
            _userService = userService;
            _channelRepository = channelRepository;
            _eventDispatcher = eventDispatcher;
            _randomService = randomService;
            _creditsRepository = creditsRepository;
            EnergyCost = 5;
        }

        public override DougResponse Activate(User user, ICombatable target, string channel)
        {
            if (user.IsStealOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateStealCooldownRemaining()));
            }

            if (target == null)
            {
                return new DougResponse();
            }

            if (target is User targetUser)
            {
                var channelType = _channelRepository.GetChannelType(channel);
                if (channelType != ChannelType.Common && channelType != ChannelType.Pvp)
                {
                    return new DougResponse(DougMessages.NotInRightChannel);
                }

                var usersInChannel = _slack.GetUsersInChannel(channel).Result;
                if (usersInChannel.All(usr => usr != targetUser.Id))
                {
                    return new DougResponse(DougMessages.UserIsNotInPvp);
                }

                var energy = user.Energy - EnergyCost;

                if (energy < 0)
                {
                    return new DougResponse(DougMessages.NotEnoughEnergy);
                }

                StealFromUser(user, targetUser, energy, channel).Wait();
            }

            return new DougResponse();
        }

        private async Task StealFromUser(User user, User target, int energyCost, string channel)
        {
            _statsRepository.UpdateEnergy(user.Id, energyCost);
            _statsRepository.SetSkillCooldown(user.Id, TimeSpan.FromSeconds(30)); // TODO uhdiuyqwdb

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
                _creditsRepository.RemoveCredits(target.Id, amount);
                _creditsRepository.AddCredits(user.Id, amount);

                var message = string.Format(DougMessages.StealCredits, _userService.Mention(user), amount, _userService.Mention(target));
                await _slack.BroadcastMessage(message, channel);
            }
            else
            {
                var message = string.Format(DougMessages.StealFail, _userService.Mention(user), _userService.Mention(target));
                await _slack.BroadcastMessage(message, channel);
            }
        }
    }
}