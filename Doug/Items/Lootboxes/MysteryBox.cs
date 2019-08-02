using System.Collections.Generic;
using System.Linq;
using Doug.Items.Consumables;
using Doug.Items.Consumables.Resets;
using Doug.Items.Equipment;
using Doug.Items.Misc;
using Doug.Items.Misc.Drops;
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

        public MysteryBox()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            Id = ItemId;
            Name = "Mystery Box";
            Description = "A mysterious box. Who knows what you might get if you open it.";
            Rarity = Rarity.Rare;
            Icon = ":mystery_box:";
            Price = 100;
        }

        public MysteryBox(IInventoryRepository inventoryRepository, IRandomService randomService, ISlackWebApi slack, IUserService userService, IItemFactory itemFactory) : base(inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
            _randomService = randomService;
            _slack = slack;
            _userService = userService;
            _itemFactory = itemFactory;

            SetProperties();

            _dropTable = new Dictionary<LootItem, double>
            {
                { new LootItem(_itemFactory.CreateItem(Apple.ItemId), 5), 0.1 },
                { new LootItem(_itemFactory.CreateItem(CoffeeCup.ItemId), 5), 0.1 },
                { new LootItem(_itemFactory.CreateItem(Bread.ItemId), 5), 0.1 },
                { new LootItem(_itemFactory.CreateItem(McdoFries.ItemId), 2), 0.05 },
                { new LootItem(_itemFactory.CreateItem(KickTicket.ItemId), 3), 0.01 },
                { new LootItem(_itemFactory.CreateItem(InviteTicket.ItemId), 3), 0.1 },
                { new LootItem(_itemFactory.CreateItem(HolyWater.ItemId), 1), 0.05 },
                { new LootItem(_itemFactory.CreateItem(SuicidePill.ItemId), 1), 0.05 },
                { new LootItem(_itemFactory.CreateItem(Cigarette.ItemId), 1), 0.05 },
                { new LootItem(_itemFactory.CreateItem(PicklePickle.ItemId), 1), 0.05 },
                { new LootItem(_itemFactory.CreateItem(BachelorsDegree.ItemId), 1), 0.05 },
                { new LootItem(_itemFactory.CreateItem(IronIngot.ItemId), 1), 0.05 },

                { new LootItem(_itemFactory.CreateItem(StraightEdge.ItemId), 1), 0.01 },
                { new LootItem(_itemFactory.CreateItem(CloakOfSpikes.ItemId), 1), 0.01 },
                { new LootItem(_itemFactory.CreateItem(AwakeningOrb.ItemId), 1), 0.01 },
                { new LootItem(_itemFactory.CreateItem(BurglarBoots.ItemId), 1), 0.01 },
                { new LootItem(_itemFactory.CreateItem(GreedyGloves.ItemId), 1), 0.005 },
                { new LootItem(_itemFactory.CreateItem(LuckyCoin.ItemId), 1), 0.005 },

                { new LootItem(_itemFactory.CreateItem(AgilityReset.ItemId), 1), 0.02 },
                { new LootItem(_itemFactory.CreateItem(StrengthReset.ItemId), 1), 0.02 },
                { new LootItem(_itemFactory.CreateItem(LuckReset.ItemId), 1), 0.02 },
                { new LootItem(_itemFactory.CreateItem(ConstitutionReset.ItemId), 1), 0.02 },
                { new LootItem(_itemFactory.CreateItem(IntelligenceReset.ItemId), 1), 0.02 }
            };
        }

        public override string Use(int itemPos, User user, string channel)
        {
            base.Use(itemPos, user, channel);

            var loot = _randomService.RandomFromWeightedTable(_dropTable);

            var item = loot.Item;

            _inventoryRepository.AddItems(user, Enumerable.Repeat(item, loot.Quantity));

            user.LoadItems(_itemFactory);
            user.InventoryItems.Sort((item1, item2) => item1.InventoryPosition.CompareTo(item2.InventoryPosition));

            _slack.BroadcastMessage(string.Format(DougMessages.LootboxAnnouncement, _userService.Mention(user), Name, $"{loot.Quantity}x *{item.Name}*"), channel).Wait();

            return string.Empty;
        }
    }
}
