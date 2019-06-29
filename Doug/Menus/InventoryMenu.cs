using System.Collections.Generic;
using System.Linq;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;

namespace Doug.Menus
{
    public class InventoryMenu
    {
        public List<Block> Blocks { get; set; }

        public InventoryMenu(IEnumerable<InventoryItem> items)
        {
            Blocks = new List<Block>
            {
                InventoryHeader(),
                new Divider()
            };

            Blocks.AddRange(items.Aggregate(new List<Block>(), (list, item) => list.Concat(ItemSection(item)).ToList()));
        }

        private Block InventoryHeader()
        {
            var inventoryButton = new PrimaryButton(DougMessages.Inventory, "inventory", Actions.InventorySwitch.ToString());
            var equipmentButton = new Button(DougMessages.Equipment, "equipment", Actions.EquipmentSwitch.ToString());

            return new ActionList(new List<Accessory> { inventoryButton, equipmentButton });
        }

        private List<Block> ItemSection(InventoryItem item)
        {
            var textBlock = new MarkdownText($"{item.Item.Icon} *{item.Item.Name}* \n {item.Item.Description}");
            var itemOptions = ItemActionsAccessory(item.InventoryPosition, item.Item.Price / 2);

            return new List<Block>
            {
                new Section(textBlock, itemOptions),
                new Context(new List<string> {string.Format(DougMessages.Quantity, item.Quantity)}),
                new Divider()
            };
        }

        private Accessory ItemActionsAccessory(int position, int sellValue)
        {
            var options = new List<Option>
            {
                new Option(DougMessages.Use, $"{InventoryActions.Use}:{position}"),
                new Option(DougMessages.Equip, $"{InventoryActions.Equip}:{position}"),
                new Option(DougMessages.Give, $"{InventoryActions.Give}:{position}"),
                new Option(DougMessages.Target, $"{InventoryActions.Target}:{position}"),
                new Option(string.Format(DougMessages.Sell, sellValue), $"{InventoryActions.Sell}:{position}")
            };

            return new Overflow(options, Actions.Inventory.ToString());
        }
    }
}
