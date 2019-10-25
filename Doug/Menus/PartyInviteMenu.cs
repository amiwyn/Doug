using System.Collections.Generic;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;
using Doug.Services;

namespace Doug.Menus
{
    public class PartyInviteMenu
    {
        public List<Block> Blocks { get; set; }

        public PartyInviteMenu(Party party, IUserService userService)
        {
            Blocks = new List<Block>
            {
                new Section(new MarkdownText($"{userService.Mention(party.Leader)} wants to invite you to his party. \n\n *Select your answer:*")),
                new ActionList(new List<Accessory>
                {
                    new Button(DougMessages.Accept, party.Id.ToString(), Actions.AcceptPartyInvite.ToString()),
                    new Button(DougMessages.Reject, party.Id.ToString(), Actions.RejectPartyInvite.ToString())
                })
            };
        }
    }
}
