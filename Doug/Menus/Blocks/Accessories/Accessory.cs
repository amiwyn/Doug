namespace Doug.Menus.Blocks.Accessories
{
    public abstract class Accessory
    {
        public string Type { get; set; }

        protected Accessory(string type)
        {
            Type = type;
        }
    }
}
