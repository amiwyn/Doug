using System;
using System.Collections.Generic;
using Doug.Items;

namespace Doug.Models
{
    public class User
    {
        private int _health;
        private int _energy;
        private int _luck;
        private int _agility;
        private int _charisma;
        private int _constitution;
        private int _stamina;

        public string Id { get; set; }
        public int Credits { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public Loadout Loadout { get; set; }
        public long Experience { get; set; }

        public int Health
        {
            get => _health;
            set
            {
                if (value <= 0)
                {
                    _health = 0;
                    return;
                }

                if (value >= TotalHealth())
                {
                    _health = TotalHealth();
                    return;
                }

                _health = value;
            }
        }

        public int Energy
        {
            get => _energy;
            set
            {
                if (value <= 0)
                {
                    _energy = 0;
                    return;
                }

                if (value >= TotalEnergy())
                {
                    _energy = TotalEnergy();
                    return;
                }

                _energy = value;
            }
        }

        public int Luck
        {
            get => Loadout.Luck + _luck;
            set => _luck = value;
        }

        public int Agility
        {
            get => Loadout.Agility + _agility;
            set => _agility = value;
        }

        public int Charisma
        {
            get => Loadout.Charisma + _charisma;
            set => _charisma = value;
        }

        public int Constitution
        {
            get => Loadout.Constitution + _constitution;
            set => _constitution = value;
        }

        public int Stamina
        {
            get => Loadout.Stamina + _stamina;
            set => _stamina = value;
        }

        public int Level => (int)Math.Floor(Math.Sqrt(Experience) * 0.1 + 1);
        public int TotalStatsPoints => (int)Math.Floor(Level + 5 * Math.Floor(Level * 0.1)) + 4;
        public int FreeStatsPoints => TotalStatsPoints + 25 - (Luck + Agility + Charisma + Constitution + Stamina);
        public int Attack => (int)Math.Floor(Charisma * 2.0);

        public User()
        {
            InventoryItems = new List<InventoryItem>();
            Loadout = new Loadout();
            
            Luck = 5;
            Agility = 5;
            Charisma = 5;
            Constitution = 5;
            Stamina = 5;
        }

        public void LevelUp()
        {
            Health = TotalHealth();
            Energy = TotalEnergy();
        }

        public void LoadItems(IItemFactory itemFactory)
        {
            InventoryItems.ForEach(item => item.CreateItem(itemFactory));
            Loadout.CreateEquipment(itemFactory);
        }

        public double GetExperienceAdvancement()
        {
            var nextLevelExp = Math.Pow((Level + 1) * 10 - 10, 2);
            var prevLevelExp = Math.Pow((Level - 1) * 10, 2);

            return (Experience - prevLevelExp) / (nextLevelExp - prevLevelExp);
        }

        public int TotalHealth()
        {
            var healthFromLevel = (int)Math.Floor(15.0 * Level + 85);
            var healthFromConstitution = (int)Math.Floor(15.0 * Constitution - 75);
            return healthFromLevel + healthFromConstitution;
        }

        public int TotalEnergy()
        {
            var energyFromLevel = (int)Math.Floor(5.0 * Level + 20);
            var energyFromStamina = (int)Math.Floor(5.0 * Stamina - 25);
            return energyFromLevel + energyFromStamina;
        }

        public double BaseGambleChance()
        {
            var luckInfluence = Math.Log(Luck / 5.0) / (Math.Log(2) * 100);
            return 0.5 + luckInfluence;
        }

        public double BaseStealSuccessRate()
        {
            var luckInfluence = (Math.Sqrt(Luck) - Math.Sqrt(5)) * 0.1;
            return 0.25 + luckInfluence;
        }

        public double BaseOpponentStealSuccessRate()
        {
            return 0.75;
        }

        public int BaseStealAmount()
        {
            return (int)Math.Floor(3 * (Math.Sqrt(Agility) - Math.Sqrt(5)) + 1);
        }

        public bool HasEnoughCreditsForAmount(int amount)
        {
            return Credits - amount >= 0;
        }

        public string NotEnoughCreditsForAmountResponse(int amount)
        {
            return string.Format(DougMessages.NotEnoughCredits, amount, Credits);
        }

        public bool IsEmptyInventory() => InventoryItems.Count == 0;
    }
}
