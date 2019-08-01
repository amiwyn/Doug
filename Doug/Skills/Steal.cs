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
            EnergyCost = 1;
            Cooldown = 30;

            _statsRepository = statsRepository;
            _slack = slack;
            _userService = userService;
            _channelRepository = channelRepository;
            _eventDispatcher = eventDispatcher;
            _randomService = randomService;
            _creditsRepository = creditsRepository;
        }

        public override DougResponse Activate(User user, ICombatable target, string channel)
        {
            if (!CanActivateSkill(user, out var response))
            {
                return response;
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

                response = StealFromUser(user, targetUser, channel).Result;
            }

            return response;
        }

        private async Task<DougResponse> StealFromUser(User user, User target, string channel)
        {
            _statsRepository.SetSkillCooldown(user.Id, TimeSpan.FromSeconds(Cooldown));

            var message = "";
            var sneakResponse = new DougResponse();

            var userChance = _eventDispatcher.OnStealingChance(user, user.BaseStealSuccessRate());
            var targetChance = _eventDispatcher.OnGettingStolenChance(target, target.BaseOpponentStealSuccessRate());

            var rollSuccessful = _randomService.RollAgainstOpponent(userChance, targetChance);
            var detected = !_randomService.RollAgainstOpponent(user.BaseDetectionAvoidance(), target.BaseDetectionChance());

            var amount = _eventDispatcher.OnStealingAmount(user, user.BaseStealAmount());

            if (target.Credits - amount < 0)
            {
                amount = target.Credits;
            }
            
            if (rollSuccessful)
            {
                _creditsRepository.RemoveCredits(target.Id, amount);
                _creditsRepository.AddCredits(user.Id, amount);

                message = string.Format(DougMessages.StealCreditsCaught, _userService.Mention(user), amount, _userService.Mention(target));
                sneakResponse = new DougResponse(string.Format(DougMessages.StealCredits, amount, _userService.Mention(target)));
            }
            else
            {
                message = string.Format(DougMessages.StealFailCaught, _userService.Mention(user), _userService.Mention(target));
                sneakResponse = new DougResponse(string.Format(DougMessages.StealFail, _userService.Mention(target)));
            }

            if (detected)
            {
                await _slack.BroadcastMessage(message, channel);
            }

            return sneakResponse;
        }
    }
}