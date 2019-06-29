namespace Doug.Menus.Blocks
{
    public abstract class Block
    {
        public string Type { get; set; }

        protected Block(string type)
        {
            Type = type;
        }
    }
}
