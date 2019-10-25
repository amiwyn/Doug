using System.Collections.Generic;
using System.Linq;
using Doug.Items.Equipment.Sets.Noob;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Items.Lootboxes
{
    public class PeasantBox : ConsumableItem
    {
        public const string ItemId = "peasant_box";

        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISlackWebApi _slack;
        private readonly IItemFactory _itemFactory;

        public PeasantBox(IInventoryRepository inventoryRepository, ISlackWebApi slack, IItemFactory itemFactory) : base(inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
            _slack = slack;
            _itemFactory = itemFactory;

            Id = ItemId;
            Name = "Peasant Box";
            Description = "Contains the farmer's set";
            Rarity = Rarity.Rare;
            Icon = ":mystery_box:";
            Price = 200;
            IsTradable = false;
            IsSellable = false;
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var items = new List<Item>
            {
                _itemFactory.CreateItem(FarmersArmor.ItemId),
                _itemFactory.CreateItem(FarmersBoots.ItemId),
                _itemFactory.CreateItem(FarmersGloves.ItemId),
                _itemFactory.CreateItem(WoodenShield.ItemId),
                _itemFactory.CreateItem(ShortSword.ItemId),
            };

            _inventoryRepository.AddItems(user, items);

            user.LoadItems(_itemFactory);
            user.InventoryItems.Sort((item1, item2) => item1.InventoryPosition.CompareTo(item2.InventoryPosition));

            var itemNames = string.Join(", ", items.Select(item => item.GetDisplayName()));
            _slack.SendEphemeralMessage(string.Format(DougMessages.YouObtained, $"{itemNames}"), user.Id, channel).Wait();

            return string.Empty;
        }
    }
}
