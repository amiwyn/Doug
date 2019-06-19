using System.Collections.Generic;

namespace Doug.Models
{
    public class User
    {
        public string Id { get; set; }
        public int Credits { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public int Health { get; set; }
        public int Energy { get; set; }

        public int Luck { get; set; }
        public int Agility { get; set; }
        public int Charisma { get; set; }

        public User() {
            InventoryItems = new List<InventoryItem>();
        }

        public double CalculateBaseGambleChance()
        {
            return 0.5;
        }

        public double CalculateBaseStealChance()
        {
            return 0.25;
        }

        public int CalculateBaseStealAmount()
        {
            return 1;
        }

        public double CalculateTotalHealth()
        {
            return 100;
        }

        public int CalculateTotalEnergy()
        {
            return 25;
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
