using System.Collections.Generic;

namespace Doug.Menus.Blocks.Accessories
{
    public class Overflow : Accessory
    {
        public List<Option> Options { get; set; }
        public string ActionId { get; set; }

        public Overflow(List<Option> options, string action) : base("overflow")
        {
            Options = options;
            ActionId = action;
        }
    }
}
