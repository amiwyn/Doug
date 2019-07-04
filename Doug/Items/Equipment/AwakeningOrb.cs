using Doug.Models;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items.Equipment
{
    public class AwakeningOrb : EquipmentItem
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;

        public AwakeningOrb(ISlackWebApi slack, IUserService userService)
        {
            _slack = slack;
            _userService = userService;

            Id = ItemFactory.AwakeningOrb;
            Name = "Orb of Awakening";
            Description = "When equipped, this strange orb will notify you privately who flamed you. You must be active to receive the notification.";
            Rarity = Rarity.Rare;
            Icon = ":crystal_ball:";
            Slot = EquipmentSlot.LeftHand;
            Price = 2100;
        }

        public override string OnGettingFlamed(Command command, string slur)
        {
            var message = string.Format(DougMessages.UserFlamedYou, _userService.Mention(new User() { Id =command.UserId}));

            _slack.SendEphemeralMessage(message, command.GetTargetUserId(), command.ChannelId);

            return base.OnGettingFlamed(command, slur);
        }
    }
}
