using System.Collections.Generic;
using Doug.Items;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models.User;

namespace Doug.Menus
{
    public class EquipmentMenu
    {
        public List<Block> Blocks { get; set; }

        public EquipmentMenu(Loadout loadout)
        {
            Blocks = new List<Block>
            {
                EquipmentHeader(),
                new Divider()
            };

            Blocks.AddRange(ItemSection(loadout.Head, "Head"));
            Blocks.AddRange(ItemSection(loadout.Body, "Body"));
            Blocks.AddRange(ItemSection(loadout.Gloves, "Gloves"));
            Blocks.AddRange(ItemSection(loadout.Boots, "Boots"));
            Blocks.AddRange(ItemSection(loadout.LeftHand, "Left Hand"));
            Blocks.AddRange(ItemSection(loadout.RightHand, "Right Hand"));
            Blocks.AddRange(ItemSection(loadout.Neck, "Neck"));
            Blocks.AddRange(ItemSection(loadout.LeftRing, "Left Ring"));
            Blocks.AddRange(ItemSection(loadout.RightRing, "Right Ring"));
            Blocks.AddRange(ItemSection(loadout.Skillbook, "Skill"));
        }

        private Block EquipmentHeader()
        {
            var inventoryButton = new Button(DougMessages.Inventory, "inventory", Actions.InventorySwitch.ToString());
            var equipmentButton = new PrimaryButton(DougMessages.Equipment, "equipment", Actions.EquipmentSwitch.ToString());
            var craftingButton = new Button(DougMessages.Crafting, "crafting", Actions.CraftingSwitch.ToString());

            return new ActionList(new List<Accessory> { inventoryButton, equipmentButton, craftingButton });
        }

        private List<Block> ItemSection(EquipmentItem equipment, string slot)
        {
            var textBlock = new MarkdownText($"{slot} : *Empty*");
            var section = new Section(textBlock);

            if (equipment != null)
            {
                textBlock = new MarkdownText($"{slot} : {equipment.Icon} *{equipment.Name}*");
                var itemOptions = ItemActionsAccessory(equipment.Slot);
                section = new Section(textBlock, itemOptions);
            }

            return new List<Block>
            {
                section,
                new Divider()
            };
        }

        private Accessory ItemActionsAccessory(EquipmentSlot slot)
        {
            var options = new List<Option>
            {
                new Option(DougMessages.UnEquip, $"{EquipmentActions.UnEquip}:{(int)slot}"),
                new Option(DougMessages.Info, $"{EquipmentActions.Info}:{(int)slot}")
            };

            return new Overflow(options, Actions.Equipment.ToString());
        }
    }
}
