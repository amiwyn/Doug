using System.Collections.Generic;

namespace Doug.Models
{
    public class User
    {
        public string Id { get; set; }
        public int Credits { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }

        public User() {
            InventoryItems = new List<InventoryItem>();
        }

        public double CalculateBaseGambleChance()
        {
            return 0.5;
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
