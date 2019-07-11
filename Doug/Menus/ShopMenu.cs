using System.Collections.Generic;
using System.Linq;
using Doug.Items;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;
using Doug.Services;

namespace Doug.Menus
{
    public class ShopMenu
    {
        private readonly IGovernmentService _governmentService;
        public List<Block> Blocks { get; set; }

        public ShopMenu(IEnumerable<Item> items, User user, IGovernmentService governmentService)
        {
            _governmentService = governmentService;
            Blocks = new List<Block>
            {
                new Section(new MarkdownText(DougMessages.ShopSpeech)),
                new Divider()
            };

            Blocks.AddRange(items.Select(ShopItemSection));
            Blocks.Add(new Divider());

            Blocks.Add(new Context(new List<string> { string.Format(DougMessages.Balance, user.Credits) }));
        }

        private Block ShopItemSection(Item item)
        {
            var textBlock = new MarkdownText($"{item.Icon} *{item.Name}* \n {item.Description}");

            var buttonBlock = new Button(string.Format(DougMessages.BuyFor, _governmentService.GetPriceWithTaxes(item)), item.Id, Actions.Buy.ToString());
            return new Section(textBlock, buttonBlock);
        }
    }
}
