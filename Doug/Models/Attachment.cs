using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class Attachment
    {
        public string Fallback { get; set; }
        public string Color { get; set; }
        public string Pretext { get; set; }
        public List<Field> Fields { get; set; }

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

            return attachment;
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
