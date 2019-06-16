using Doug.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class User
    {
        public string Id { get; set; }
        public int Credits { get; set; }
        public List<UserItem> UserItems { get; set; }

        public User() {
            UserItems = new List<UserItem>();
        }

        public bool HasEnoughCreditsForAmount(int amount)
        {
            return Credits - amount >= 0;
        }

        public DougResponse NotEnoughCreditsForAmountResponse(int amount)
        {
            return new DougResponse(string.Format(DougMessages.NotEnoughCredits, amount, Credits));
        }
    }
}
