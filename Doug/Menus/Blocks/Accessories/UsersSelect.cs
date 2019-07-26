using Doug.Menus.Blocks.Text;

namespace Doug.Menus.Blocks.Accessories
{
    public class UsersSelect : Accessory
    {
        public PlainText Placeholder { get; set; }
        public string ActionId { get; set; }

        public UsersSelect(string placeholder, string action) : base("users_select")
        {
            Placeholder = new PlainText(placeholder);
            ActionId = action;
        }
    }
}
