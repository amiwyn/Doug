using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class Option
    {
        public PlainText Text { get; set; }
        public string Value { get; set; }

        public Option(string text, string value)
        {
            Text = new PlainText(text);
            Value = value;
        }
    }
}
