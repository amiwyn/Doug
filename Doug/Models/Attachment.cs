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
