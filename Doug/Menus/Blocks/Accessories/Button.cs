using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class Button : Accessory
    {
        public PlainText Text { get; set; }
        public string Value { get; set; }
        public string ActionId { get; set; }

        public Button(string text, string value, string action) : base("button")
        {
            Text = new PlainText(text);
            Value = value;
            ActionId = action;
        }
    }
}
