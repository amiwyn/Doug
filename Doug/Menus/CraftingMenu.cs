using System.Collections.Generic;
using System.Linq;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;

namespace Doug.Menus
{
    public class CraftingMenu
    {
        public List<Block> Blocks { get; set; }

        public CraftingMenu(IEnumerable<InventoryItem> items)
        {
            var options = items.Select(item => new Option($"{item.Item.Icon} {item.Item.Name}", item.InventoryPosition.ToString())).ToList();

            var accessory = new MultiSelect(options, new PlainText(DougMessages.CraftingSelect), Actions.Craft.ToString());
            var text = new MarkdownText(DougMessages.CraftingDialog);

            Blocks = new List<Block>
            {
                CraftingHeader(),
                new Divider(),
                new Section(text, accessory)
            };
        }
        
        private Block CraftingHeader()
        {
            var inventoryButton = new Button(DougMessages.Inventory, "inventory", Actions.InventorySwitch.ToString());
            var equipmentButton = new Button(DougMessages.Equipment, "equipment", Actions.EquipmentSwitch.ToString());
            var craftingButton = new PrimaryButton(DougMessages.Crafting, "crafting", Actions.CraftingSwitch.ToString());

            return new ActionList(new List<Accessory> { inventoryButton, equipmentButton, craftingButton });
        }
    }
}
