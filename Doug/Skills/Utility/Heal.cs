using System.Threading.Tasks;
using Doug.Items.WeaponType;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills.Utility
{
    public class Heal : Skill
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;

        public Heal(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService) : base(statsRepository)
        {
            Name = "Heal";
            EnergyCost = 52;
            Cooldown = 50;
            RequiredWeapon = typeof(Staff);

            _slack = slack;
            _userService = userService;
        }

        public override async Task<DougResponse> Activate(User user, ICombatable target, string channel)
        {
            if (!CanActivateSkill(user, out var response))
            {
                return response;
            }

            var userToBeHealed = user;
            if (target != null && target is User targetUser)
            {
                userToBeHealed = targetUser;
            }

            var healAmount = user.Level * 5 + 2 * user.TotalIntelligence();

            userToBeHealed.Health += healAmount; 
            StatsRepository.UpdateHealth(userToBeHealed.Id, userToBeHealed.Health);

            var message = string.Format(DougMessages.UserHealed, _userService.Mention(user), _userService.Mention(userToBeHealed), healAmount);
            await _slack.BroadcastMessage(message, channel);

            return new DougResponse();
        }
    }
}
