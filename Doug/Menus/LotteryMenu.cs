using System.Collections.Generic;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;

namespace Doug.Menus
{
    public class LotteryMenu
    {
        public List<Block> Blocks { get; set; }

        public LotteryMenu(int ticketPrice)
        {
            var accessory = new Button(string.Format(DougMessages.TicketPrice, ticketPrice), "ticket", Actions.BuyTicket.ToString());

            Blocks = new List<Block>
            {
                new Section(new MarkdownText(DougMessages.LotteryText)),
                new Divider(),
                new Section(new MarkdownText(DougMessages.AdditionalTickets), accessory)
            };
        }
    }
}
