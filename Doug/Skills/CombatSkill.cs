using System.Linq;
using Doug.Items;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Skills
{
    public abstract class CombatSkill : Skill
    {
        private readonly IChannelRepository _channelRepository;
        private readonly ISlackWebApi _slack;
        private readonly IEventDispatcher _eventDispatcher;

        protected CombatSkill(IStatsRepository statsRepository, IChannelRepository channelRepository, ISlackWebApi slack, IEventDispatcher eventDispatcher) : base(statsRepository)
        {
            _channelRepository = channelRepository;
            _slack = slack;
            _eventDispatcher = eventDispatcher;
        }

        protected override bool CanActivateSkill(User user, ICombatable target, string channel, out DougResponse response)
        {
            if (target is User targetUser)
            {
                _eventDispatcher.OnAttacking(user, target, 0);

                var channelType = _channelRepository.GetChannelType(channel);
                if (channelType != ChannelType.Pvp)
                {
                    response = new DougResponse(DougMessages.NotInRightChannel);
                    return false;
                }

                var usersInChannel = _slack.GetUsersInChannel(channel).Result;
                if (usersInChannel.All(usr => usr != targetUser.Id))
                {
                    response = new DougResponse(DougMessages.UserIsNotInPvp);
                    return false;
                }
            }

            return base.CanActivateSkill(user, target, channel, out response);
        }
    }
}
