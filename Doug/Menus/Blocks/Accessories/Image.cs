namespace Doug.Menus.Blocks.Accessories
{
    public class Image : Accessory
    {
        public string ImageUrl { get; set; }
        public string AltText { get; set; }

        public Image(string url, string text) : base("image")
        {
            ImageUrl = url;
            AltText = text;
        }
    }
}
