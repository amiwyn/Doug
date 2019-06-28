using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class UsersDropdown : Accessory
    {
        public PlainText Placeholder { get; set; }

        public UsersDropdown(string placeholder, string actionId) : base("users_select", actionId)
        {
            Placeholder = new PlainText(placeholder);
        }
    }
}
