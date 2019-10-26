using System.Collections.Generic;

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
