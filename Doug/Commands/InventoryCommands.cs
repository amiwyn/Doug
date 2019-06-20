using System.Linq;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Commands
{
    public interface IInventoryCommands
    {
        DougResponse Use(Command command);
        DougResponse Give(Command command);
    }

    public class InventoryCommands : IInventoryCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;

        public InventoryCommands(IUserRepository userRepository, ISlackWebApi slack)
        {
            _userRepository = userRepository;
            _slack = slack;
        }

        public DougResponse Use(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(0));
            var item = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position);

            if (item == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            var response = item.Item.Use(position, user, _userRepository);

            return new DougResponse(response);
        }

        public DougResponse Give(Command command)
        {
            var target = command.GetTargetUserId();
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(1));
            var item = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position);

            if (item == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            _userRepository.RemoveItem(user.Id, position);

            _userRepository.AddItem(target, item.ItemId);

            var message = string.Format(DougMessages.UserGaveItem, Utils.UserMention(user.Id), item.Item.Name, Utils.UserMention(target));
            _slack.SendMessage(message, command.ChannelId);

            return new DougResponse();
        }
    }
}
