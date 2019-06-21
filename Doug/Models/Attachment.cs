using System.Collections.Generic;

namespace Doug.Models
{
    public class Attachment
    {
        public string Fallback { get; set; }
        public string Color { get; set; }
        public string Pretext { get; set; }
        public List<Field> Fields { get; set; }

        private static readonly string[] RarityColor = { "#adadad", "#26cc3e", "#2669cc", "#e26b16", "#dbb313" };

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

        public static Attachment StatsAttachment(int slurCount, User user)
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
            attachment.Fields.Add(new Field(string.Format(DougMessages.SlursAddedStats, slurCount)));
            attachment.Fields.Add(new Field(string.Format(DougMessages.HealthStats, user.Health, user.CalculateTotalHealth())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.EnergyStats, user.Energy, user.CalculateTotalEnergy())));
            attachment.Fields.Add(new Field(string.Format(DougMessages.CharismaStats, user.Charisma)));
            attachment.Fields.Add(new Field(string.Format(DougMessages.AgilityStats, user.Agility)));
            attachment.Fields.Add(new Field(string.Format(DougMessages.LuckStats, user.Luck)));

            attachment.Fields.Add(new Field(DougMessages.ItemStats));

            //user.InventoryItems.ForEach(inventoryItem => attachment.Fields.Add(new Field(string.Format("{0} - {2} {1} {3}", inventoryItem.InventoryPosition, inventoryItem.Item.Name, inventoryItem.Item.Icon, inventoryItem.Quantity == 1 ? string.Empty : "(" + inventoryItem.Quantity + ")"))));

            return attachment;
        }

        public static List<Attachment> InventoryAttachments(User user)
        {
            var attachments = new List<Attachment>();
            foreach (var inventoryItem in user.InventoryItems)
            {
                var itemAttachment = new Attachment()
                {
                    Color = RarityColor[(int)inventoryItem.Item.Rarity],
                };

                itemAttachment.Fields.Add(new Field(string.Format("{0} - {2} {1} {3}", inventoryItem.InventoryPosition, inventoryItem.Item.Name, inventoryItem.Item.Icon, inventoryItem.Quantity == 1 ? string.Empty : "(" + inventoryItem.Quantity + ")")));
                attachments.Add(itemAttachment);
            }

            return attachments;
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
