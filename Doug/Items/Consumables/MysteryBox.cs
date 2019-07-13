using System.Collections.Generic;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items.Consumables
{
    public class MysteryBox : ConsumableItem
    {
        private readonly Dictionary<LootItem, double> _dropTable;

        private readonly IRandomService _randomService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IItemFactory _itemFactory;

        public MysteryBox(IInventoryRepository inventoryRepository, IRandomService randomService, ISlackWebApi slack, IUserService userService, IItemFactory itemFactory) : base(inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
            _randomService = randomService;
            _slack = slack;
            _userService = userService;
            _itemFactory = itemFactory;

            Id = ItemFactory.MysteryBox;
            Name = "Mystery Box";
            Description = "A mysterious box. Who knows what you might get if you open it.";
            Rarity = Rarity.Rare;
            Icon = ":mystery_box:";
            Price = 100;

            _dropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(ItemFactory.Apple, 5), 0.1 },
                { new LootItem(ItemFactory.CoffeeCup, 5), 0.1 },
                { new LootItem(ItemFactory.Bread, 5), 0.1 },
                { new LootItem(ItemFactory.McdoFries, 2), 0.05 },
                { new LootItem(ItemFactory.KickTicket, 3), 0.05 },
                { new LootItem(ItemFactory.InviteTicket, 3), 0.1 },
                { new LootItem(ItemFactory.HolyWater, 1), 0.05 },
                { new LootItem(ItemFactory.SuicidePill, 1), 0.05 },
                { new LootItem(ItemFactory.Cigarette, 1), 0.05 },
                { new LootItem(ItemFactory.BachelorsDegree, 1), 0.05 },

                { new LootItem(ItemFactory.StraightEdge, 1), 0.01 },
                { new LootItem(ItemFactory.CloakOfSpikes, 1), 0.01 },
                { new LootItem(ItemFactory.AwakeningOrb, 1), 0.01 },
                { new LootItem(ItemFactory.BurglarBoots, 1), 0.01 },
                { new LootItem(ItemFactory.GreedyGloves, 1), 0.01 },

                { new LootItem(ItemFactory.AgilityReset, 1), 0.05 },
                { new LootItem(ItemFactory.StrengthReset, 1), 0.05 },
                { new LootItem(ItemFactory.LuckReset, 1), 0.05 },
                { new LootItem(ItemFactory.ConstitutionReset, 1), 0.05 },
                { new LootItem(ItemFactory.StaminaReset, 1), 0.05 }
            };
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var loot = _randomService.RandomFromWeightedTable(_dropTable);

            _inventoryRepository.AddMultipleItems(user, loot.Id, loot.Quantity);

            var itemName = _itemFactory.CreateItem(loot.Id).Name;
            _slack.BroadcastMessage(string.Format(DougMessages.LootboxAnnouncement, _userService.Mention(user), Name, $"{loot.Quantity}x *{itemName}*"), channel);

            return string.Empty;
        }
    }
}
