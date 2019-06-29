namespace Doug.Menus.Blocks.Text
{
    public class PlainText : TextBlock
    {
        public bool Emoji { get; set; }

        public PlainText(string text)
        {
            Type = "plain_text";
            Text = text;
            Emoji = true;
        }
    }
}
