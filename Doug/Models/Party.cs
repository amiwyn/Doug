using System.Collections.Generic;

namespace Doug.Models
{
    public class Party
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User Leader { get; set; }

        public List<User> Users { get; set; }
    }
}
