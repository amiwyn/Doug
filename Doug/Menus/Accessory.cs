using System.Collections.Generic;

namespace Doug.Menus
{
    public class Accessory
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ActionId { get; set; }
        public TextBlock Text { get; set; }
        public List<OptionBlock> Options { get; set; }

        public static Accessory Button(string text, string value, string action)
        {
            return new Accessory { Type = "button", Text = TextBlock.PlainTextBlock(text), Value = value, ActionId = action };
        }

        public static Accessory OptionList(List<OptionBlock> optionBlocks, string action)
        {
            return new Accessory { Type = "overflow", Options = optionBlocks, ActionId = action };
        }
    }

    public class OptionBlock
    {
        public TextBlock Text { get; set; }
        public string Value { get; set; }

        public OptionBlock(string text, string value)
        {
            Text = TextBlock.PlainTextBlock(text);
            Value = value;
        }
    }
}
