namespace Doug.Menus.Blocks.Text
{
    public class MarkdownText : TextBlock
    {
        public MarkdownText(string text)
        {
            Type = "mrkdwn";
            Text = text;
        }
    }
}
