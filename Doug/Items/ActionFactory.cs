using System.Linq;
using Doug.Effects;
using Doug.Items.ItemActions;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items
{
    public interface IActionFactory
    {
        Action CreateAction(string actionId);
        string Eat(int itemPos, User user, int health, int energy, string itemName);
        string EatEffect(int itemPos, User user, string effectId, int duration);
        string Transport(int itemPos, User user, string channel);
        string OpenLootBox(int itemPos, User user, DropTable dropTable, string channel, string lootboxName);
    }

    public class ActionFactory : IActionFactory
    {
        private readonly IEffectRepository _effectRepository;
        private readonly IUserService _userService;
        private readonly IStatsRepository _statsRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISlackWebApi _slack;
        private readonly IRandomService _randomService;
        private readonly IItemRepository _itemRepository;

        public ActionFactory(IEffectRepository effectRepository, IUserService userService, IStatsRepository statsRepository, IInventoryRepository inventoryRepository, ISlackWebApi slack, IRandomService randomService, IItemRepository itemRepository)
        {
            _effectRepository = effectRepository;
            _userService = userService;
            _statsRepository = statsRepository;
            _inventoryRepository = inventoryRepository;
            _slack = slack;
            _randomService = randomService;
            _itemRepository = itemRepository;
        }

        public Action CreateAction(string actionId)
        {
            switch (actionId)
            {
                case "cleanse": return new Cleanse(_effectRepository, _inventoryRepository).Activate;
                case "suicide": return new Suicide(_inventoryRepository, _userService).Activate;
                case "reset": return new Reset(_inventoryRepository, _statsRepository).Activate;
                default: return (_, __, ___) => DougMessages.ItemCantBeUsed;
            }
        }

        public string Eat(int itemPos, User user, int health, int energy, string itemName)
        {
            _inventoryRepository.RemoveItem(user, itemPos);

            var recoveryText = string.Empty;

            if (health != 0)
            {
                user.Health += health;
                _statsRepository.UpdateHealth(user.Id, user.Health);
                recoveryText += $"*{health}* Health";
            }
            else if (energy != 0)
            {
                user.Energy += energy;
                _statsRepository.UpdateEnergy(user.Id, user.Energy);
                recoveryText += recoveryText == string.Empty ? $"*{energy}* Energy" : $" and *{energy}* Energy";
            }
            else
            {
                return string.Format(DougMessages.RecoverItemNothing, itemName);
            }

            return string.Format(DougMessages.RecoverItem, itemName, recoveryText);
        }

        public string EatEffect(int itemPos, User user, string effectId, int duration)
        {
            _effectRepository.AddEffect(user, effectId, duration);

            return string.Format(DougMessages.AddedEffect, EffectFactory.GetEffectName(effectId), duration);
        }

        public string Transport(int itemPos, User user, string channel)
        {
            _inventoryRepository.RemoveItem(user, itemPos);

            _slack.InviteUser(user.Id, channel);

            return string.Empty;
        }

        public string OpenLootBox(int itemPos, User user, DropTable dropTable, string channel, string lootboxName)
        {
            _inventoryRepository.RemoveItem(user, itemPos);

            var loot = _randomService.RandomFromWeightedTable(dropTable);
            var item = _itemRepository.GetItem(loot.Id);

            _inventoryRepository.AddItems(user, Enumerable.Repeat(item, loot.Quantity));
            user.InventoryItems.Sort((item1, item2) => item1.InventoryPosition.CompareTo(item2.InventoryPosition));

            _slack.BroadcastMessage(string.Format(DougMessages.LootboxAnnouncement, _userService.Mention(user), lootboxName, $"{loot.Quantity}x {item.GetDisplayName()}"), channel).Wait();

            return string.Empty;
        }
    }
}
