using System.Collections.Generic;
using System.Linq;
using Doug.Items.Consumables;
using Doug.Items.Consumables.Resets;
using Doug.Items.Equipment;
using Doug.Items.Misc;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.Items.Lootboxes
{
    public class MysteryBox : ConsumableItem
    {
        public const string ItemId = "mystery_box";

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

            Id = ItemId;
            Name = "Mystery Box";
            Description = "A mysterious box. Who knows what you might get if you open it.";
            Rarity = Rarity.Rare;
            Icon = ":mystery_box:";
            Price = 100;

            _dropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(Apple.ItemId, 5), 0.1 },
                { new LootItem(CoffeeCup.ItemId, 5), 0.1 },
                { new LootItem(Bread.ItemId, 5), 0.1 },
                { new LootItem(McdoFries.ItemId, 2), 0.05 },
                { new LootItem(KickTicket.ItemId, 3), 0.05 },
                { new LootItem(InviteTicket.ItemId, 3), 0.1 },
                { new LootItem(HolyWater.ItemId, 1), 0.05 },
                { new LootItem(SuicidePill.ItemId, 1), 0.05 },
                { new LootItem(Cigarette.ItemId, 1), 0.05 },
                { new LootItem(BachelorsDegree.ItemId, 1), 0.05 },

                { new LootItem(StraightEdge.ItemId, 1), 0.01 },
                { new LootItem(CloakOfSpikes.ItemId, 1), 0.01 },
                { new LootItem(AwakeningOrb.ItemId, 1), 0.01 },
                { new LootItem(BurglarBoots.ItemId, 1), 0.01 },
                { new LootItem(GreedyGloves.ItemId, 1), 0.01 },

                { new LootItem(AgilityReset.ItemId, 1), 0.05 },
                { new LootItem(StrengthReset.ItemId, 1), 0.05 },
                { new LootItem(LuckReset.ItemId, 1), 0.05 },
                { new LootItem(ConstitutionReset.ItemId, 1), 0.05 },
                { new LootItem(IntelligenceReset.ItemId, 1), 0.05 }
            };
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var loot = _randomService.RandomFromWeightedTable(_dropTable);

            var item = _itemFactory.CreateItem(loot.Id);

            _inventoryRepository.AddItems(user, Enumerable.Repeat(item, loot.Quantity));

            user.LoadItems(_itemFactory);
            user.InventoryItems.Sort((item1, item2) => item1.InventoryPosition.CompareTo(item2.InventoryPosition));

            _slack.BroadcastMessage(string.Format(DougMessages.LootboxAnnouncement, _userService.Mention(user), Name, $"{loot.Quantity}x *{item.Name}*"), channel).Wait();

            return string.Empty;
        }
    }
}
