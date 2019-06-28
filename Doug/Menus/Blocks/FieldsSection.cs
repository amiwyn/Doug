using System.Collections.Generic;
using System.Linq;
using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks
{
    public class FieldsSection : Block
    {
        public List<TextBlock> Fields { get; set; }

        public FieldsSection(List<string> fields) : base("section")
        {
            Fields = fields.Select(txt => (TextBlock)new MarkdownText(txt)).ToList();
        }
    }
}
