using System.Linq;
using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Commands
{
    public interface IInventoryCommands
    {
        DougResponse Use(Command command);
        DougResponse Give(Command command);
        DougResponse Equip(Command command);
    }

    public class InventoryCommands : IInventoryCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly IStatsRepository _statsRepository;
        private readonly ISlackWebApi _slack;
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryCommands(IUserRepository userRepository, ISlackWebApi slack, IStatsRepository statsRepository, IInventoryRepository inventoryRepository)
        {
            _userRepository = userRepository;
            _slack = slack;
            _statsRepository = statsRepository;
            _inventoryRepository = inventoryRepository;
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

            var response = inventoryItem.Item.Use(position, user, _inventoryRepository, _statsRepository);

            return new DougResponse(response);
        }

        public DougResponse Give(Command command)
        {
            var target = command.GetTargetUserId();
            var user = _userRepository.GetUser(command.UserId);
            var position = int.Parse(command.GetArgumentAt(1));
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == position);

            if (inventoryItem == null)
            {
                return new DougResponse(string.Format(DougMessages.NoItemInSlot, position));
            }

            _inventoryRepository.RemoveItem(user.Id, position);

            _inventoryRepository.AddItem(target, inventoryItem.ItemId);

            var message = string.Format(DougMessages.UserGaveItem, Utils.UserMention(user.Id), inventoryItem.Item.Name, Utils.UserMention(target));
            _slack.SendMessage(message, command.ChannelId);

            return new DougResponse();
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
            var equipment = user.Loadout.GetEquipmentAt(equipmentItem.Slot);

            if (equipment != null)
            {
                var item = _inventoryRepository.UnequipItem(user.Id, equipmentItem.Slot);
                _inventoryRepository.AddItem(user.Id, item.Id);
            }

            _inventoryRepository.EquipItem(user.Id, equipmentItem);
            _inventoryRepository.RemoveItem(user.Id, position);

            return new DougResponse(string.Format(DougMessages.EquippedItem, inventoryItem.Item.Name));
        }
    }
}
