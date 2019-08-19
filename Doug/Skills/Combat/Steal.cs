using System;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills.Combat
{
    public class Steal : CombatSkill
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IStatsRepository _statsRepository;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IRandomService _randomService;
        private readonly ICreditsRepository _creditsRepository;

        public Steal(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService, IChannelRepository channelRepository, IEventDispatcher eventDispatcher, IRandomService randomService, ICreditsRepository creditsRepository) : base(statsRepository, channelRepository, slack)
        {
            Name = "Steal";
            EnergyCost = 1;
            Cooldown = 30;

            _statsRepository = statsRepository;
            _slack = slack;
            _userService = userService;
            _eventDispatcher = eventDispatcher;
            _randomService = randomService;
            _creditsRepository = creditsRepository;
        }

        public override async Task<DougResponse> Activate(User user, ICombatable target, string channel)
        {
            if (!CanActivateSkill(user, target, channel, out var response))
            {
                return response;
            }

            if (target == null)
            {
                return new DougResponse();
            }

            if (target is User targetUser)
            {
                response = await StealFromUser(user, targetUser, channel);
            }

            return response;
        }

        private async Task<DougResponse> StealFromUser(User user, User target, string channel)
        {
            _statsRepository.SetSkillCooldown(user.Id, TimeSpan.FromSeconds(Cooldown));

            var userChance = _eventDispatcher.OnStealingChance(user, user.BaseStealSuccessRate());
            var targetChance = _eventDispatcher.OnGettingStolenChance(target, target.BaseOpponentStealSuccessRate());

            var rollSuccessful = _randomService.RollAgainstOpponent(userChance, targetChance);
            var detected = !_randomService.RollAgainstOpponent(user.BaseDetectionAvoidance(), target.BaseDetectionChance());

            var amount = _eventDispatcher.OnStealingAmount(user, user.BaseStealAmount());

            if (target.Credits - amount < 0)
            {
                amount = target.Credits;
            }

            string message;
            DougResponse sneakResponse;

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