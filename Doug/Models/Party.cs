using System.Collections.Generic;

namespace Doug.Models
{
    public class Party
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User.User Leader { get; set; }

        public List<User.User> Users { get; set; }
    }
}
