namespace Doug.Menus.Blocks.Accessories
{
    public abstract class Accessory
    {
        public string Type { get; set; }
        public string ActionId { get; set; }

        protected Accessory(string type, string action)
        {
            Type = type;
            ActionId = action;
        }
    }
}
