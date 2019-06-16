using System;

namespace Doug.Models
{
    public class Command
    {
        public string ChannelId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }

        public string GetTargetUserId()
        {
            var parts = GetArgumentAt(0).Split('|');
            return parts[0].Substring(2);
        }

        public int GetArgumentCount()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return 0;
            }

            var args = Text.Split(' ');
            return args.Length;
        }

        public bool IsUserArgument()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return false;
            }

            var argument = GetArgumentAt(0);
            return argument.StartsWith("<@");
        }

        public string GetArgumentAt(int index)
        {
            if (Text == null)
            {
                throw new ArgumentException(DougMessages.InvalidArgumentCount);
            }

            var args = Text.Split(' ');
            if (args.Length <= index)
            {
                throw new ArgumentException(DougMessages.InvalidArgumentCount);
            }

            return args[index];
        }
    }
}
