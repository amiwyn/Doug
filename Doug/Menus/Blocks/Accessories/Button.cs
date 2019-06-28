using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class Button : Accessory
    {
        public PlainText Text { get; set; }
        public string Value { get; set; }

        public Button(string text, string value, string actionId) : base("button", actionId)
        {
            Text = new PlainText(text);
            Value = value;
        }
    }
}
