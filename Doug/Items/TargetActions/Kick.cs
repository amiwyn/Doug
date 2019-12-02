using Doug.Effects;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.TargetActions
{
    public class Kick : ItemTargetAction
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISlackWebApi _slack;

        public Kick(IEventDispatcher eventDispatcher, ISlackWebApi slack, IInventoryRepository inventoryRepository) : base(inventoryRepository)
        {
            _eventDispatcher = eventDispatcher;
            _slack = slack;
        }

        public override string Activate(int itemPos, User user, User target, string channel)
        {
            if (!_eventDispatcher.OnKick(target, user, channel))
            {
                return string.Empty;
            }

            _slack.KickUser(target.Id, channel).Wait();

            return base.Activate(itemPos, user, target, channel);
        }
    }
}
