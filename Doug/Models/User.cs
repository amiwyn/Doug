using System;
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
        public long Experience { get; set; }

        public User() {
            InventoryItems = new List<InventoryItem>();
        }

        public int GetLevel()
        {
            return (int)Math.Floor(Math.Sqrt(Experience) * 0.1 + 1);
        }

        public double GetExperienceAdvancement()
        {
            var nextLevelExp = Math.Pow((GetLevel() + 1) * 10 - 10, 2);
            var prevLevelExp = Math.Pow((GetLevel() - 1) * 10, 2);

            return (Experience - prevLevelExp) / (nextLevelExp - prevLevelExp);
        }

        public double CalculateBaseGambleChance()
        {
            var luckInfluence = Math.Log(Luck / 5.0) / (Math.Log(2) * 100);
            return 0.5 + luckInfluence;
        }

        public double CalculateBaseStealSuccessRate()
        {
            var luckInfluence = (Math.Sqrt(Luck) - Math.Sqrt(5)) * 0.1;
            return 0.25 + luckInfluence;
        }

        public double CalculateBaseOpponentStealSuccessRate()
        {
            return 0.75;
        }

        public int CalculateBaseStealAmount()
        {
            return (int)Math.Floor(6 * (Math.Sqrt(Agility) - Math.Sqrt(5)) + 1);
        }

        public int CalculateTotalHealth()
        {
            return (int)Math.Floor(15.0 * GetLevel() + 85);
        }

        public int CalculateTotalEnergy()
        {
            return (int)Math.Floor(5.0 * GetLevel() + 20);
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
