using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks
{
    public class Section : Block
    {
        public TextBlock Text { get; set; }
        public Accessory Accessory { get; set; }
        public string BlockId { get; set; }

        public Section(TextBlock text) : base("section")
        {
            Text = text;
        }

        public Section(TextBlock text, Accessory accessory) : base("section")
        {
            Text = text;
            Accessory = accessory;
        }

        public Section(TextBlock text, Accessory accessory, string blockId) : base("section")
        {
            Text = text;
            Accessory = accessory;
            BlockId = blockId;
        }
    }
}
