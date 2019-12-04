using System.Threading.Tasks;
using System.Linq;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;
using Doug.Services;
using System;

namespace Doug.Skills
{
    public abstract class BuffSkill : Skill
    {
        public string BuffId;
        public string BuffName;
        public int Duration;

        private readonly ISlackWebApi _slack;
        private readonly IEffectRepository _effectRepository;
        private readonly IUserService _userService;

        protected BuffSkill(IStatsRepository statsRepository, IEffectRepository effectRepository, ISlackWebApi slack, IUserService userService) : base(statsRepository)
        {
            _slack = slack;
            _effectRepository = effectRepository;
            _userService = userService;
        }

        protected override bool CanActivateSkill(User user, ICombatable target, string channel, out DougResponse response)
        {
            var totalCooldown = Cooldown * (1 - user.CooldownReduction());
            if (target is User targetUser)
            {
                var usersInChannel = _slack.GetUsersInChannel(channel).Result;
                if (usersInChannel.All(usr => usr != targetUser.Id))
                {
                    response = new DougResponse(DougMessages.UserNotInChannel);
                    return false;
                }
            }

            if (user.IsSkillOnCooldown())
            {
                response = new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateStealCooldownRemaining()));
                return false;
            }

            if (!user.HasEnoughEnergyForCost(EnergyCost))
            {
                response = new DougResponse(DougMessages.NotEnoughEnergy);
                return false;
            }

            user.Energy -= EnergyCost;
            StatsRepository.FireSkill(user.Id, TimeSpan.FromSeconds(totalCooldown), user.Energy);
            response = new DougResponse();
            return true;
        }

        public override async Task<DougResponse> Activate(User user, ICombatable target, string channel)
        {
            if (!CanActivateSkill(user, target, channel, out var response))
            {
                return response;
            }

            var userToBeBuffed = user;
            if (target != null && target is User targetUser)
            {
                userToBeBuffed = targetUser;
            }

            _effectRepository.AddEffect(userToBeBuffed, BuffId, Duration);

            var message = string.Format(DougMessages.UserBuffed, _userService.Mention(user), BuffName, _userService.Mention(userToBeBuffed), Duration);
            await _slack.BroadcastMessage(message, channel);

            return new DougResponse();
        }
    }
}
