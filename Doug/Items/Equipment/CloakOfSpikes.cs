using System;
using Doug.Models;

namespace Doug.Items.Equipment
{
    public class CloakOfSpikes : EquipmentItem
    {
        public const string ItemId = "cloak_spikes";

        public CloakOfSpikes()
        {
            Id = ItemId;
            Name = "Cloak of spikes";
            Description = "It looks more like wool blanket. It can reflect slurs sent to you.";
            Rarity = Rarity.Rare;
            Icon = ":cloak_spikes:";
            Slot = EquipmentSlot.Body;
            Price = 1150;
            LevelRequirement = 10;
        }

        public override string OnGettingFlamed(Command command, string slur)
        {
            if (new Random().Next(2) == 0)
            {
                slur = slur.Replace($"<@{command.GetTargetUserId()}>", $"<@{command.UserId}>");
            }

            return slur;
        }
    }
}
