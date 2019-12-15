using System.Threading.Tasks;
using Doug.Items;
using Doug.Menus;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Commands
{
    public interface IInventoryCommands
    {
        DougResponse Use(Command command);
        DougResponse Give(Command command);
        DougResponse Target(Command command);
        DougResponse Equip(Command command);
        DougResponse UnEquip(Command command);
        Task<DougResponse> Inventory(Command command);
    }

    public class InventoryCommands : IInventoryCommands
    {
        private readonly IInventoryService _inventoryService;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;

        public InventoryCommands(IInventoryService inventoryService, IUserRepository userRepository, ISlackWebApi slack)
        {
            _inventoryService = inventoryService;
            _userRepository = userRepository;
            _slack = slack;
        }

        public DougResponse Use(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(0));

            return new DougResponse(_inventoryService.Use(user, position, command.ChannelId));
        }

        public DougResponse Give(Command command)
        {
            var target = _userRepository.GetUser(command.GetTargetUserId());
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(1));

            return new DougResponse(_inventoryService.Give(user, target, position, command.ChannelId));
        }

        public DougResponse Target(Command command)
        {
            var target = _userRepository.GetUser(command.GetTargetUserId());
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(1));

            return new DougResponse(_inventoryService.Target(user, target, position, command.ChannelId));
        }

        public DougResponse Equip(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(0));

            return new DougResponse(_inventoryService.Equip(user, position));
        }

        public DougResponse UnEquip(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var slot = (EquipmentSlot)int.Parse(command.GetArgumentAt(0));

            return new DougResponse(_inventoryService.UnEquip(user, slot));
        }

        public async Task<DougResponse> Inventory(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            if (user.HasEmptyInventory())
            {
                return new DougResponse(DougMessages.EmptyInventory);
            }

            await _slack.SendEphemeralBlocks(new InventoryMenu(user.InventoryItems).Blocks, command.UserId, command.ChannelId);

            return new DougResponse();
        }
    }
}
