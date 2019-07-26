using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class PrimaryButton : Accessory
    {
        public PlainText Text { get; set; }
        public string Value { get; set; }
        public string Style { get; set; }
        public string ActionId { get; set; }

        public PrimaryButton(string text, string value, string action) : base("button")
        {
            Text = new PlainText(text);
            Value = value;
            Style = "primary";
            ActionId = action;
        }
    }
}
