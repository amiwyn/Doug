using System;
using Doug.Models;
using Doug.Slack;

namespace Doug.Items.Equipment
{
    public class CloakOfSpikes : EquipmentItem
    {
        public CloakOfSpikes()
        {
            Id = ItemFactory.CloakOfSpikes;
            Name = "Cloak of spikes";
            Description = "It looks more like wool blanket. It can reflect slurs sent to you.";
            Rarity = Rarity.Rare;
            Icon = ":shirt:";
            Slot = EquipmentSlot.Body;
            Price = 1150;
        }

        public override string OnGettingFlamed(Command command, string slur, ISlackWebApi slack)
        {
            if (new Random().Next(2) == 0)
            {
                slur = slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
            }

            return slur;
        }
    }
}
