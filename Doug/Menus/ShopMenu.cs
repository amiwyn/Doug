using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Models;

namespace Doug.Menus
{
    public class ShopMenu
    {
        public List<BlockMessage> Blocks { get; set; }

        public ShopMenu(IEnumerable<Item> items, User user)
        {
            Blocks = new List<BlockMessage>
            {
                BlockMessage.TextSection(DougMessages.ShopSpeech),
                BlockMessage.Divider()
            };

            Blocks.AddRange(items.Select(ShopItemSection));
            Blocks.Add(BlockMessage.Divider());

            var creditsLeft = TextBlock.MarkdownTextBlock(string.Format(DougMessages.Balance, user.Credits));
            Blocks.Add(BlockMessage.Context(new List<TextBlock>() { creditsLeft }));
        }

        private static BlockMessage ShopItemSection(Item item)
        {
            var textBlock = TextBlock.MarkdownTextBlock($"{item.Icon} *{item.Name}* \n {item.Description}");

            var buttonBlock = Accessory.Button(string.Format(DougMessages.BuyFor, item.Price), item.Id, "buy");
            return new BlockMessage { Type = "section", Text = textBlock, Accessory = buttonBlock };
        }
    }
}
