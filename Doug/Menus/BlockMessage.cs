using System.Collections.Generic;

namespace Doug.Menus
{
    public class BlockMessage
    {
        public string Type { get; set; }
        public TextBlock Text { get; set; }
        public Accessory Accessory { get; set; }
        public List<TextBlock> Elements { get; set; }

        public static BlockMessage TextSection(string text)
        {
            var textBlock = TextBlock.MarkdownTextBlock(text);
            return new BlockMessage { Type = "section", Text = textBlock };
        }

        public static BlockMessage Divider()
        {
            return new BlockMessage {Type = "divider"};
        }

        public static BlockMessage Context(string text)
        {
            var elements = new List<TextBlock> {TextBlock.MarkdownTextBlock(text)};
            return new BlockMessage { Type = "context", Elements = elements };
        }
    }
}
