namespace Doug.Menus
{
    public class TextBlock
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public bool? Emoji { get; set; }

        public static TextBlock MarkdownTextBlock(string text)
        {
            return new TextBlock
            {
                Type = "mrkdwn",
                Text = text,
                Emoji = null
            };
        }

        public static TextBlock PlainTextBlock(string text)
        {
            return new TextBlock()
            {
                Type = "plain_text",
                Text = text,
                Emoji = true
            };
        }
    }
}
