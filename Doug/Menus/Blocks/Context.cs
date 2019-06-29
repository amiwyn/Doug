using System.Collections.Generic;
using System.Linq;
using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks
{
    public class Context : Block
    {
        public List<TextBlock> Elements { get; set; }

        public Context(List<string> elements) : base("context")
        {
            Elements = elements.Select(txt => (TextBlock)new MarkdownText(txt)).ToList();
        }
    }
}
