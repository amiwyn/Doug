using System.Collections.Generic;
using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class MultiSelect : Accessory
    {
        public List<Option> Options { get; set; }
        public string ActionId { get; set; }
        public PlainText Placeholder { get; set; }

        public MultiSelect(List<Option> options, PlainText placeholder, string action) : base("multi_static_select")
        {
            Options = options;
            Placeholder = placeholder;
            ActionId = action;
        }
    }
}
