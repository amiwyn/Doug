using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class UsersDropdown : Accessory
    {
        public PlainText Placeholder { get; set; }

        public UsersDropdown(string placeholder, Actions action) : base("users_select", action)
        {
            Placeholder = new PlainText(placeholder);
        }
    }
}
