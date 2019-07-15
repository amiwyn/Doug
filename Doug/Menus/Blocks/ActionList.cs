using System.Collections.Generic;
using Doug.Menus.Blocks.Accessories;

namespace Doug.Menus.Blocks
{
    public class ActionList : Block
    {
        public List<Accessory> Elements { get; set; }
        public string BlockId { get; set; }

        public ActionList(List<Accessory> elements) : base("actions")
        {
            Elements = elements;
        }

        public ActionList(List<Accessory> elements, string blockId) : base("actions")
        {
            Elements = elements;
            BlockId = blockId;
        }
    }
}
