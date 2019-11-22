using Doug.Effects;
using Doug.Models.Combat;
using Doug.Models.User;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items
{
    public interface ITargetActionFactory
    {
        TargetAction CreateTargetAction(string targetActionId);
    }

    public class TargetActionFactory : ITargetActionFactory
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISlackWebApi _slack;
        private readonly ICombatService _combatService;

        public TargetActionFactory(IEventDispatcher eventDispatcher, ISlackWebApi slack, ICombatService combatService)
        {
            _eventDispatcher = eventDispatcher;
            _slack = slack;
            _combatService = combatService;
        }

        public TargetAction CreateTargetAction(string targetActionId)
        {
            switch (targetActionId)
            {
                case "kick": return Kick;
                case "antirogue": return AntiRogue;
                default: return (_, __, ___, ____) => DougMessages.ItemCantBeUsed;
            }
        }

        private string Kick(int itemPos, User user, User target, string channel)
        {
            if (!_eventDispatcher.OnKick(target, user, channel))
            {
                return string.Empty;
            }

            _slack.KickUser(target.Id, channel).Wait();

            return string.Empty;
        }

        private string AntiRogue(int itemPos, User user, User target, string channel)
        {
            var damage = target.Level * target.Agility;
            var attack = new MagicAttack(user, damage);
            target.ReceiveAttack(attack, _eventDispatcher);

            _combatService.DealDamage(user, attack, target, channel).Wait();

            return string.Empty;
        }
    }
}
