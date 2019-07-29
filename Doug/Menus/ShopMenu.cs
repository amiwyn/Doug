using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;
using Doug.Services;
using Doug.Services.MenuServices;

namespace Doug.Menus
{
    public class ShopMenu
    {
        private readonly IGovernmentService _governmentService;
        public List<Block> Blocks { get; set; }

        public ShopMenu(Shop shop, User user, IItemFactory itemFactory, IGovernmentService governmentService)
        {
            var items = shop.ShopItems.Select(itm => itemFactory.CreateItem(itm.ItemId));

            _governmentService = governmentService;
            Blocks = new List<Block>
            {
                ShopHeader(),
                new Section(new MarkdownText($"*{shop.Name}* \n {shop.Description}")),
                new Divider()
            };

            Blocks.AddRange(items.Aggregate(new List<Block>(), (list, item) => list.Concat(ShopItemSection(item, shop.Id)).ToList()));
            Blocks.Add(new Divider());

            Blocks.Add(new Context(new List<string> { string.Format(DougMessages.Balance, user.Credits) }));
        }

        private Block ShopHeader()
        {
            var generalStoreButton = new Button(DougMessages.GeneralStore, ShopMenuService.GeneralStoreId, $"{Actions.ShopSwitch.ToString()}:0");
            var peasantShop = new Button(DougMessages.PeasantShop, ShopMenuService.PeasantShopId, $"{Actions.ShopSwitch.ToString()}:1");
            var armoryShopButton = new Button(DougMessages.ArmoryShop, ShopMenuService.ArmoryShopId, $"{Actions.ShopSwitch.ToString()}:2");
            var rogueShopButton = new Button(DougMessages.RogueShop, ShopMenuService.RogueShop, $"{Actions.ShopSwitch.ToString()}:3");
            var magicShopButton = new Button(DougMessages.MagicShop, ShopMenuService.MagicShop, $"{Actions.ShopSwitch.ToString()}:4");

            return new ActionList(new List<Accessory> { generalStoreButton, peasantShop, armoryShopButton, rogueShopButton, magicShopButton }, Actions.ShopSwitch.ToString());
        }

        private List<Block> ShopItemSection(Item item, string shopId)
        {
            var textBlock = new MarkdownText($"{item.Icon} *{item.Name}*");

            var buttonBlock = new Button(string.Format(DougMessages.BuyFor, _governmentService.GetPriceWithTaxes(item)), item.Id, Actions.Buy.ToString());

            var shopItem = new List<Block> { new Section(textBlock, buttonBlock, $"{shopId}:{item.Id}") };

            if (item is EquipmentItem equipmentItem)
            {
                var attributes = equipmentItem.GetDisplayAttributeList();
                shopItem.Add(new Context(attributes));
            }

            return shopItem;
        }
    }
}
