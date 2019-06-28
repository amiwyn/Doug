using System.Collections.Generic;
using System.Linq;
using Doug.Models;

namespace Doug.Menus
{
    public class InventoryMenu
    {
        public List<BlockMessage> Blocks { get; set; }

        public InventoryMenu(IEnumerable<InventoryItem> items)
        {
            Blocks = new List<BlockMessage>
            {
                BlockMessage.Divider()
            };

            Blocks.AddRange(items.Aggregate(new List<BlockMessage>(), (list, item) => list.Concat(ItemSection(item)).ToList()));
        }

        private static List<BlockMessage> ItemSection(InventoryItem item)
        {
            var blocks = new List<BlockMessage>();

            var textBlock = TextBlock.MarkdownTextBlock($"{item.Item.Icon} *{item.Item.Name}* - ({item.InventoryPosition}) \n {item.Item.Description}");
            var itemOptions = ItemActionsAccessory(item.InventoryPosition, item.Item.Price / 2);

            blocks.Add(new BlockMessage { Type = "section", Text = textBlock, Accessory = itemOptions });
            blocks.Add(BlockMessage.Context(new List<TextBlock> { TextBlock.MarkdownTextBlock(string.Format(DougMessages.Quantity, item.Quantity)) }));
            blocks.Add(BlockMessage.Divider());

            return blocks;
        }

        private static Accessory ItemActionsAccessory(int position, int sellValue)
        {
            var options = new List<OptionBlock>
            {
                new OptionBlock(DougMessages.Use, $"use:{position}"),
                new OptionBlock(DougMessages.Equip, $"equip:{position}"),
                new OptionBlock(string.Format(DougMessages.Sell, sellValue), $"sell:{position}")
            };

            return Accessory.OptionList(options, "inventory");
        }
    }
}
