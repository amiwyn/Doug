using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Text;
using Doug.Models;

namespace Doug.Menus
{
    public class ShortProfileMenu
    {
        public List<Block> Blocks { get; set; }

        public ShortProfileMenu(User user, Party party)
        {
            Blocks = new List<Block>
            {
                new Section(new MarkdownText($"<@{user.Id}>")),
                CreateSmallUserInfo(user),
                new Divider()
            };

            if (party != null)
            {
                Blocks.Add(CreatePartyFields(party));
                Blocks.Add(new Divider());
            }
            
            if (user.Effects.Count > 0)
            {
                Blocks.Add(CreateEffectFields(user));
                Blocks.Add(new Divider());
            }

            if (!user.Loadout.IsEmpty())
            {
                Blocks.Add(CreateEquipmentInfo(user));
            }
        }

        private Block CreateEquipmentInfo(User user)
        {
            return new FieldsSection(user.Loadout.GetDisplayEquipmentList().ToList());
        }

        private Block CreateSmallUserInfo(User user)
        {
            var userMiscInfo = new List<string>
            {
                string.Format(DougMessages.LevelStats, user.Level),
                string.Format(DougMessages.SmallHealthDisplay, $"*{user.Health}*/{user.TotalHealth()}"),
                string.Format(DougMessages.CreditStats, user.Credits)
            };

            return new Context(userMiscInfo);
        }

        private Block CreateEffectFields(User user)
        {
            var fields = user.Effects.Select(ef => $"{ef.Effect.Icon} - {ef.Effect.Name} ({ef.GetDurationString()})").ToList();
            return new FieldsSection(fields);
        }

        private Block CreatePartyFields(Party party)
        {
            var fields = party.Users.Select(usr => $"<@{usr.Id}>").ToList();
            fields.Insert(0, "In party with :");
            return new Context(fields);
        }
    }
}
