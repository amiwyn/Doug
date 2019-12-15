using System.Linq;
using Doug.Items;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IInventoryService
    {
        string Use(User user, int itemPosition, string channel);
        string Give(User user, User target, int itemPosition, string channel);
        string Target(User user, User target, int itemPosition, string channel);
        string Equip(User user, int itemPosition);
        string UnEquip(User user, EquipmentSlot slot);
    }

    public class InventoryService : IInventoryService
    {
        private readonly IActionFactory _actionFactory;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUserService _userService;
        private readonly ISlackWebApi _slack;
        private readonly ITargetActionFactory _targetFactory;
        private readonly IEquipmentRepository _equipmentRepository;

        public InventoryService(IActionFactory actionFactory, IInventoryRepository inventoryRepository, IUserService userService, ISlackWebApi slack, ITargetActionFactory targetFactory, IEquipmentRepository equipmentRepository)
        {
            _actionFactory = actionFactory;
            _inventoryRepository = inventoryRepository;
            _userService = userService;
            _slack = slack;
            _targetFactory = targetFactory;
            _equipmentRepository = equipmentRepository;
        }

        public string Use(User user, int itemPosition, string channel)
        {
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == itemPosition);

            if (inventoryItem == null)
            {
                return string.Format(DougMessages.NoItemInSlot, itemPosition);
            }

            return inventoryItem.Item.Use(_actionFactory, itemPosition, user, channel);
        }

        public string Give(User user, User target, int itemPosition, string channel)
        {
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == itemPosition);

            if (inventoryItem == null)
            {
                return string.Format(DougMessages.NoItemInSlot, itemPosition);
            }

            if (!inventoryItem.Item.IsTradable)
            {
                return DougMessages.ItemNotTradable;
            }

            _inventoryRepository.RemoveItem(user, itemPosition);

            _inventoryRepository.AddItem(target, inventoryItem.Item);

            var message = string.Format(DougMessages.UserGaveItem, _userService.Mention(user), inventoryItem.Item.GetDisplayName(), _userService.Mention(target));
            _slack.BroadcastMessage(message, channel);

            return string.Empty;
        }

        public string Target(User user, User target, int itemPosition, string channel)
        {
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == itemPosition);

            if (inventoryItem == null)
            {
                return string.Format(DougMessages.NoItemInSlot, itemPosition);
            }

            if (inventoryItem.Item.IsTargetable())
            {
                _slack.BroadcastMessage(string.Format(DougMessages.UsedItemOnTarget, _userService.Mention(user), inventoryItem.Item.GetDisplayName(), _userService.Mention(target)), channel);
            }

            return inventoryItem.Item.Target(_targetFactory, itemPosition, user, target, channel);
        }

        public string Equip(User user, int itemPosition)
        {
            var inventoryItem = user.InventoryItems.SingleOrDefault(itm => itm.InventoryPosition == itemPosition);

            if (inventoryItem == null)
            {
                return string.Format(DougMessages.NoItemInSlot, itemPosition);
            }

            if (!inventoryItem.Item.IsEquipable())
            {
                return DougMessages.ItemNotEquipAble;
            }

            var equipmentItem = (EquipmentItem)inventoryItem.Item;

            if (!user.CanEquip(equipmentItem))
            {
                return DougMessages.LevelRequirementNotMet;
            }

            var unequippedItems = _equipmentRepository.EquipItem(user, equipmentItem);

            _inventoryRepository.AddItems(user, unequippedItems.Select(item => item));
            _inventoryRepository.RemoveItem(user, itemPosition);

            return string.Format(DougMessages.EquippedItem, inventoryItem.Item.GetDisplayName());
        }

        public string UnEquip(User user, EquipmentSlot slot)
        {
            var equipment = user.Loadout.GetEquipmentAt(slot);

            if (equipment == null)
            {
                return string.Format(DougMessages.NoEquipmentInSlot, slot);
            }

            var item = _equipmentRepository.UnequipItem(user, equipment.Slot);

            _inventoryRepository.AddItem(user, item);

            return string.Format(DougMessages.UnequippedItem, equipment.GetDisplayName());
        }
    }
}
