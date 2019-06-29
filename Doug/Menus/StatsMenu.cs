using System.Collections.Generic;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;

namespace Doug.Menus
{
    public class StatsMenu
    {
        public List<Block> Blocks { get; set; }

        public StatsMenu(User user)
        {
            Blocks = new List<Block>
            {
                new Section(new MarkdownText(string.Format(DougMessages.StatsOf, Utils.UserMention(user.Id)))),
                CreateUserOtherInfo(user),
                new Divider(),
                CreateHealthFields(user),
                CreateEnergyFields(user),
                new Divider()
            };

            Blocks.AddRange(CreateStatsFields(user));
            Blocks.Add(new Divider());

            if (user.FreeStatsPoints > 0)
            {
                var freeStatsPoints = string.Format(DougMessages.FreeStatPoints, user.FreeStatsPoints.ToString());
                Blocks.Add(new Context(new List<string> { freeStatsPoints }));
            }
        }

        private Block CreateUserOtherInfo(User user)
        {
            var userMiscInfo = new List<string>
            {
                string.Format(DougMessages.LevelStats, user.Level),
                string.Format(DougMessages.ExperienceStats, user.GetExperienceAdvancement() * 100),
                string.Format(DougMessages.CreditStats, user.Credits)
            };

            return new Context(userMiscInfo);
        }

        private Block CreateHealthFields(User user)
        {
            var healthFields = new List<string>
            {
                DougMessages.HealthStats,
                $"{user.Health}/{user.TotalHealth()}"
            };

            return new FieldsSection(healthFields);
        }

        private Block CreateEnergyFields(User user)
        {
            var energyFields = new List<string>
            {
                DougMessages.EnergyStats,
                $"{user.Energy}/{user.TotalEnergy()}"
            };

           return new FieldsSection(energyFields);
        }

        private List<Block> CreateStatsFields(User user)
        {
            var buttonDisplayed = user.FreeStatsPoints > 0;

            return new List<Block>
            {
                StatSection(DougMessages.LuckStats, user.TotalLuck(), Stats.Luck, buttonDisplayed),
                StatSection(DougMessages.AgilityStats, user.TotalAgility(), Stats.Agility, buttonDisplayed),
                StatSection(DougMessages.CharismaStats, user.TotalCharisma(), Stats.Charisma, buttonDisplayed),
                StatSection(DougMessages.ConstitutionStats, user.TotalConstitution(), Stats.Constitution, buttonDisplayed),
                StatSection(DougMessages.StaminaStats, user.TotalStamina(), Stats.Stamina, buttonDisplayed)
            };
        }

        private Block StatSection(string message, int stat, string type, bool buttonDisplayed)
        {
            var textBlock = new MarkdownText(string.Format(message, stat));

            if (!buttonDisplayed)
            {
                return new Section(textBlock);
            }

            var buttonBlock = new Button(DougMessages.AddStatPoint, type, Actions.Attribution.ToString());
            return new Section(textBlock, buttonBlock);
        }
    }
}
