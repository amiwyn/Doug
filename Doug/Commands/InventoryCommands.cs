using System.Linq;
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
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IUserService _userService;

        public InventoryCommands(IUserRepository userRepository, ISlackWebApi slack, IInventoryRepository inventoryRepository, IEquipmentRepository equipmentRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _slack = slack;
            _inventoryRepository = inventoryRepository;
            _equipmentRepository = equipmentRepository;
            _userService = userService;
        }

        public DougResponse Use(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(0));
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position);

            if (inventoryItem == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            var response = inventoryItem.Item.Use(position, user, command.ChannelId);

            return new DougResponse(response);
        }

        public DougResponse Give(Command command)
        {
            var target = _userRepository.GetUser(command.GetTargetUserId());
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(1));
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position);

            if (inventoryItem == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            if (!inventoryItem.Item.IsTradable)
            {
               return new DougResponse(DougMessages.ItemNotTradable); 
            }

            _inventoryRepository.RemoveItem(user, position);

            _inventoryRepository.AddItem(target, inventoryItem.ItemId);

            var message = string.Format(DougMessages.UserGaveItem, _userService.Mention(user), inventoryItem.Item.Name, _userService.Mention(target));
            _slack.BroadcastMessage(message, command.ChannelId);

            return new DougResponse();
        }

        public DougResponse Target(Command command)
        {
            var target = _userRepository.GetUser(command.GetTargetUserId());
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(1));
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position);

            if (inventoryItem == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            var response = inventoryItem.Item.Target(position, user, target, command.ChannelId);

            return new DougResponse(response);
        }

        public DougResponse Equip(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(0));
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position);

            if (inventoryItem == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            if (!inventoryItem.Item.IsEquipable())
            {
                return new DougResponse(DougMessages.ItemNotEquipAble);
            }

            var equipmentItem = (EquipmentItem)inventoryItem.Item;

            if (equipmentItem.LevelRequirement > user.Level)
            {
                return new DougResponse(string.Format(DougMessages.LevelRequirementNotMet, equipmentItem.LevelRequirement));
            }

            var equipment = user.Loadout.GetEquipmentAt(equipmentItem.Slot);

            if (equipment != null)
            {
                var item = _equipmentRepository.UnequipItem(user.Id, equipmentItem.Slot);
                _inventoryRepository.AddItem(user, item.Id);
            }

            _equipmentRepository.EquipItem(user.Id, equipmentItem);
            _inventoryRepository.RemoveItem(user, position);

            return new DougResponse(string.Format(DougMessages.EquippedItem, inventoryItem.Item.Name));
        }

        public DougResponse UnEquip(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var slot = int.Parse(command.GetArgumentAt(0));
            var equipment = user.Loadout.GetEquipmentAt((EquipmentSlot) slot);

            if (equipment == null)
            {
                return new DougResponse(string.Format(DougMessages.NoEquipmentInSlot, slot));
            }

            var item = _equipmentRepository.UnequipItem(user.Id, equipment.Slot);

            _inventoryRepository.AddItem(user, item.Id);

            return new DougResponse(string.Format(DougMessages.UnequippedItem, equipment.Name));
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
