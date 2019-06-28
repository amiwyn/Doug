using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class PrimaryButton : Accessory
    {
        public PlainText Text { get; set; }
        public string Value { get; set; }
        public string Style { get; set; }

        public PrimaryButton(string text, string value, string actionId) : base("button", actionId)
        {
            Text = new PlainText(text);
            Value = value;
            Style = "primary";
        }
    }
}
