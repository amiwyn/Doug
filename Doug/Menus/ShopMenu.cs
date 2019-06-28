using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;

namespace Doug.Menus
{
    public class ShopMenu
    {
        public List<Block> Blocks { get; set; }

        public ShopMenu(IEnumerable<Item> items, User user)
        {
            Blocks = new List<Block>
            {
                new Section(new MarkdownText(DougMessages.ShopSpeech)),
                new Divider()
            };

            Blocks.AddRange(items.Select(ShopItemSection));
            Blocks.Add(new Divider());

            Blocks.Add(new Context(new List<string> { string.Format(DougMessages.Balance, user.Credits) }));
        }

        private static Block ShopItemSection(Item item)
        {
            var textBlock = new MarkdownText($"{item.Icon} *{item.Name}* \n {item.Description}");

            var buttonBlock = new Button(string.Format(DougMessages.BuyFor, item.Price), item.Id, "buy");
            return new Section(textBlock, buttonBlock);
        }
    }
}
