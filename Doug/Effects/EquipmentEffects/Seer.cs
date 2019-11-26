using Doug.Models;
using Doug.Models.User;
using Doug.Services;
using Doug.Slack;

namespace Doug.Effects.EquipmentEffects
{
    public class Seer : EquipmentEffect
    {
        private readonly IUserService _userService;
        private readonly ISlackWebApi _slack;
        public const string EffectId = "seer";

        public Seer(IUserService userService, ISlackWebApi slack)
        {
            _userService = userService;
            _slack = slack;
            Id = EffectId;
            Name = "Seer";
        }

        public override string OnGettingFlamed(Command command, string slur)
        {
            var message = string.Format(DougMessages.UserFlamedYou, _userService.Mention(new User() { Id = command.UserId }));

            _slack.SendEphemeralMessage(message, command.GetTargetUserId(), command.ChannelId);

            return base.OnGettingFlamed(command, slur);
        }
    }
}
