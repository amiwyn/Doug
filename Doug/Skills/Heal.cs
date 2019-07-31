using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills
{
    public class Heal : Skill
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;

        public Heal(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService) : base(statsRepository)
        {
            _slack = slack;
            _userService = userService;
            EnergyCost = 8;
        }

        public override DougResponse Activate(User user, ICombatable target, string channel)
        {
            if (!user.HasEnoughEnergyForCost(EnergyCost))
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }
            StatsRepository.UpdateEnergy(user.Id, EnergyCost);

            var userToBeHealed = user;
            if (target != null && target is User targetUser)
            {
                userToBeHealed = targetUser;
            }

            var healAmount = 30; // TODO : add scaling

            userToBeHealed.Health += healAmount; 
            StatsRepository.UpdateHealth(userToBeHealed.Id, userToBeHealed.Health);

            var message = string.Format(DougMessages.UserHealed, _userService.Mention(user), _userService.Mention(userToBeHealed), healAmount);
            _slack.BroadcastMessage(message, channel);

            return new DougResponse();
        }
    }
}
