using System.Linq;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Commands
{
    public interface IInventoryCommands
    {
        DougResponse Use(Command command);
    }

    public class InventoryCommands : IInventoryCommands
    {
        private readonly IUserRepository _userRepository;

        public InventoryCommands(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
