using System.Collections.Generic;
using System.Linq;

namespace Doug.Models
{
    public class Attachment
    {
        public string Fallback { get; set; }
        public string Color { get; set; }
        public string Pretext { get; set; }
        public List<Field> Fields { get; set; }

        private static readonly string[] RarityColor = { "#adadad", "#26cc3e", "#2669cc", "#e26b16", "#860daa" };

        public Attachment()
        {
            Fields = new List<Field>();
        }

        public static Attachment DeletedSlursAttachment(List<Slur> slurs)
        {
            var attachment = new Attachment()
            {
                Fallback = DougMessages.SlursCleaned,
                Color = "#e05f28",
                Pretext = DougMessages.SlursCleaned
            };

            slurs.ForEach(slur => attachment.Fields.Add(new Field(slur.Text)));

            return attachment;
        }

        public static Attachment StatsAttachment(User user)
        {
            var title = string.Format(DougMessages.StatsOf, Utils.UserMention(user.Id));

            var attachment = new Attachment()
            {
                Fallback = title,
                Color = "#69FF69",
                Pretext = title
            };

            attachment.Fields.Add(new Field(string.Format(DougMessages.UserIdStats, user.Id)));
            attachment.Fields.Add(new Field(string.Format(DougMessages.CreditStats, user.Credits)));
            attachment.Fields.Add(new Field(string.Format(DougMessages.LevelStats, user.Level)));
            attachment.Fields.Add(new Field(string.Format(DougMessages.ExperienceStats, user.GetExperienceAdvancement() * 100)));
            attachment.Fields.Add(new Field(string.Format(DougMessages.HealthStats, user.Health, user.TotalHealth())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.EnergyStats, user.Energy, user.TotalEnergy())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.LuckStats, user.TotalLuck())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.AgilityStats, user.TotalAgility())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.CharismaStats, user.TotalCharisma())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.ConstitutionStats, user.TotalConstitution())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.StaminaStats, user.TotalStamina())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.FreeStatPoints, user.FreeStatsPoints)));

            return attachment;
        }

        public static List<Attachment> InventoryAttachments(User user)
        {
            return user.InventoryItems.Select(inventoryItem =>
            {
                var itemAttachment = new Attachment()
                {
                    Color = RarityColor[(int)inventoryItem.Item.Rarity],
                };

                //TODO : make tostring methods or w/e
                itemAttachment.Fields.Add(new Field(string.Format("{0} - {2} {1} {3}", inventoryItem.InventoryPosition, inventoryItem.Item.Name, inventoryItem.Item.Icon, inventoryItem.Quantity == 1 ? string.Empty : "(" + inventoryItem.Quantity + ")")));

                return itemAttachment;
            }).ToList();
        }

        public static List<Attachment> EquipmentAttachments(Loadout loadout)
        {
            return loadout.Equipment.Select(entry =>
            {
                var item = entry.Value;
                var itemAttachment = new Attachment()
                {
                    Color = RarityColor[(int)item.Rarity],
                };

                itemAttachment.Fields.Add(new Field($"{item.Slot.ToString()} - {item.Icon} {item.Name}"));

                return itemAttachment;
            }).ToList();
        }
    }

    public class Field
    {
        public string Title { get; set; }
        public bool Short { get; set; }

        public Field(string text)
        {
            Title = text;
            Short = false;
        }
    }
}
