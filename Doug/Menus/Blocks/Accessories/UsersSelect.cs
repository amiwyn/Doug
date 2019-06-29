using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class UsersSelect : Accessory
    {
        public PlainText Placeholder { get; set; }

        public UsersSelect(string placeholder, string action) : base("users_select", action)
        {
            Placeholder = new PlainText(placeholder);
        }
    }
}
