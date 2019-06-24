using System.Collections.Generic;
using System.Linq;
using Doug.Items;

namespace Doug.Models
{
    public class TextBlock
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public bool? Emoji { get; set; }

        public static TextBlock MarkdownTextBlock(string text)
        {
            return new TextBlock
            {
                Type = "mrkdwn",
                Text = text,
                Emoji = null
            };
        }

        public static TextBlock PlainTextBlock(string text)
        {
            return new TextBlock()
            {
                Type = "plain_text",
                Text = text,
                Emoji = true
            };
        }
    }

    public class Accessory
    {
        public string Type { get; set; }
        public TextBlock Text { get; set; }
        public string Value { get; set; }
        public string ActionId { get; set; }

        public static Accessory Button(string text, string value, string action)
        {
            return new Accessory {Type = "button", Text = TextBlock.PlainTextBlock(text), Value = value, ActionId = action};
        }
    }

    public class BlockMessage
    {
        public string Type { get; set; }
        public TextBlock Text { get; set; }
        public Accessory Accessory { get; set; }

        public static List<BlockMessage> ShopMessage(IEnumerable<Item> items)
        {
            var blocks = new List<BlockMessage>
            {
                TextSection(DougMessages.ShopSpeech),
                Divider()
            };

            blocks.AddRange(items.Select(ShopItemSection));
            blocks.Add(Divider());

            return blocks;
        }
        private static BlockMessage TextSection(string text)
        {
            var textBlock = TextBlock.MarkdownTextBlock(text);
            return new BlockMessage { Type = "section", Text = textBlock };
        }

        private static BlockMessage Divider()
        {
            return new BlockMessage {Type = "divider"};
        }

        private static BlockMessage ShopItemSection(Item item)
        {
            var textBlock = TextBlock.MarkdownTextBlock($"{item.Icon} *{item.Name}* \n {item.Description}");

            var buttonBlock = Accessory.Button(string.Format(DougMessages.BuyFor, item.Price), item.Id, "buy");
            return new BlockMessage { Type = "section", Text = textBlock, Accessory = buttonBlock };
        }
    }
}
