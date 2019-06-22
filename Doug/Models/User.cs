using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Slack;

namespace Doug.Models
{
    public class User
    {
        private int _luck;
        private int _agility;
        private int _charisma;
        private int _constitution;
        private int _stamina;

        public string Id { get; set; }
        public int Credits { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public long Experience { get; set; }

        public int Level => (int)Math.Floor(Math.Sqrt(Experience) * 0.1 + 1);
        public int TotalStatsPoints => (int)Math.Floor(Level + 5 * Math.Floor(Level * 0.1)) + 5;
        public int FreeStatsPoints => TotalStatsPoints + 25 - (_luck + _agility + _charisma + _constitution + _stamina);
        public double ExperienceAdvancement
        {
            get
            {
                var nextLevelExp = Math.Pow((Level + 1) * 10 - 10, 2);
                var prevLevelExp = Math.Pow((Level - 1) * 10, 2);

                return (Experience - prevLevelExp) / (nextLevelExp - prevLevelExp);
            }
        }

        public int Health { get; set; }
        public int TotalHealth
        {
            get
            {
                var healthFromLevel = (int)Math.Floor(15.0 * Level + 85);
                var healthFromConstitution = (int)Math.Floor(15.0 * Constitution - 75);
                return healthFromLevel + healthFromConstitution;
            }
        }

        public int Energy { get; set; }
        public int TotalEnergy
        {
            get
            {
                var energyFromLevel = (int)Math.Floor(5.0 * Level + 20);
                var energyFromStamina = (int)Math.Floor(5.0 * Stamina - 25);
                return energyFromLevel + energyFromStamina;
            }
        }

        public int Luck
        {
            get => InventoryItems.Sum(item => item.Item.Luck) + _luck;
            set => _luck = value;
        }
        public int Agility
        {
            get => InventoryItems.Sum(item => item.Item.Agility) + _agility;
            set => _agility = value;
        }
        public int Charisma
        {
            get => InventoryItems.Sum(item => item.Item.Charisma) + _charisma;
            set => _charisma = value;
        }
        public int Constitution
        {
            get => InventoryItems.Sum(item => item.Item.Constitution) + _constitution;
            set => _constitution = value;
        }
        public int Stamina
        {
            get => InventoryItems.Sum(item => item.Item.Stamina) + _stamina;
            set => _stamina = value;
        }

        public User() {
            InventoryItems = new List<InventoryItem>();
            _luck = 5;
            _agility = 5;
            _charisma = 5;
            _constitution = 5;
            _stamina = 5;
        }

        public void AddExperience(long experience, string channel, ISlackWebApi slack) // TODO: move this somewhere else
        {
            var previousLevel = Level;
            Experience += experience;

            slack.SendEphemeralMessage(string.Format(DougMessages.GainedExp, experience), Id, channel);

            if (previousLevel < Level)
            {
                slack.SendMessage(string.Format(DougMessages.LevelUp, Utils.UserMention(Id), Level), channel);
            }
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
