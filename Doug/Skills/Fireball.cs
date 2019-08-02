using Doug.Items;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Skills
{
    public class Fireball : Skill
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly ICombatService _combatService;
        private readonly IEventDispatcher _eventDispatcher;

        public Fireball(IStatsRepository statsRepository, ISlackWebApi slack, IUserService userService, ICombatService combatService, IEventDispatcher eventDispatcher) : base(statsRepository)
        {
            Name = "Fireball";
            EnergyCost = 10;
            Cooldown = 30;

            _slack = slack;
            _userService = userService;
            _combatService = combatService;
            _eventDispatcher = eventDispatcher;
        }

        public override DougResponse Activate(User user, ICombatable target, string channel)
        {
            if (!CanActivateSkill(user, out var response))
            {
                return response;
            }

            var message = string.Format(DougMessages.UserActivatedSkill, _userService.Mention(user), Name);
            _slack.BroadcastMessage(message, channel).Wait();

            var attack = new MagicAttack(user, user.TotalIntelligence());
            target.ReceiveAttack(attack, _eventDispatcher);
            _combatService.DealDamage(user, attack, target, channel).Wait();

            return new DougResponse();
        }
    }
}
