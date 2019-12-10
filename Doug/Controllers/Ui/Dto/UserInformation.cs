using Doug.Models.User;

namespace Doug.Controllers.Ui.Dto
{
    public class UserInformation
    {
        public User User { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public UserInformation(User user, string name, string icon)
        {
            User = user;
            Name = name;
            Icon = icon;
        }
    }
}
