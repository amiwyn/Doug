using System.Collections.Generic;
using Doug.Models;

namespace Doug.Menus
{
    public class StatsMenu
    {
        public List< BlockMessage> Blocks { get; set; }

        public StatsMenu(User user)
        {
            Blocks = new List<BlockMessage>
            {
                BlockMessage.TextSection(string.Format(DougMessages.StatsOf, Utils.UserMention(user.Id))),
                CreateUserOtherInfo(user),
                BlockMessage.Divider(),
                CreateHealthFields(user),
                CreateEnergyFields(user),
                BlockMessage.Divider()
            };

            Blocks.AddRange(CreateStatsFields(user));
            Blocks.Add(BlockMessage.Divider());

            if (user.FreeStatsPoints > 0)
            {
                var freeStatsPoints = TextBlock.MarkdownTextBlock(string.Format(DougMessages.FreeStatPoints, user.FreeStatsPoints.ToString()));
                Blocks.Add(BlockMessage.Context(new List<TextBlock> { freeStatsPoints }));
            }
        }

        private BlockMessage CreateUserOtherInfo(User user)
        {
            var userMiscInfo = new List<TextBlock>
            {
                TextBlock.MarkdownTextBlock(string.Format(DougMessages.LevelStats, user.Level)),
                TextBlock.MarkdownTextBlock(string.Format(DougMessages.ExperienceStats, user.GetExperienceAdvancement() * 100)),
                TextBlock.MarkdownTextBlock(string.Format(DougMessages.CreditStats, user.Credits))
            };

            return BlockMessage.Context(userMiscInfo);
        }

        private BlockMessage CreateHealthFields(User user)
        {
            var healthFields = new List<TextBlock>
            {
                TextBlock.MarkdownTextBlock(DougMessages.HealthStats),
                TextBlock.MarkdownTextBlock($"{user.Health}/{user.TotalHealth()}")
            };

            return BlockMessage.FieldsSection(healthFields);
        }

        private BlockMessage CreateEnergyFields(User user)
        {
            var energyFields = new List<TextBlock>
            {
                TextBlock.MarkdownTextBlock(DougMessages.EnergyStats),
                TextBlock.MarkdownTextBlock($"{user.Energy}/{user.TotalEnergy()}")
            };

           return BlockMessage.FieldsSection(energyFields);
        }

        private List<BlockMessage> CreateStatsFields(User user)
        {
            var buttonDisplayed = user.FreeStatsPoints > 0;

            var blocks = new List<BlockMessage>
            {
                StatSection(DougMessages.LuckStats, user.Luck, Stats.Luck, buttonDisplayed),
                StatSection(DougMessages.AgilityStats, user.Agility, Stats.Agility, buttonDisplayed),
                StatSection(DougMessages.CharismaStats, user.Charisma, Stats.Charisma, buttonDisplayed),
                StatSection(DougMessages.ConstitutionStats, user.Constitution, Stats.Constitution, buttonDisplayed),
                StatSection(DougMessages.StaminaStats, user.Stamina, Stats.Stamina, buttonDisplayed)
            };

            return blocks;
        }

        private BlockMessage StatSection(string message, int stat, string type, bool buttonDisplayed)
        {
            var textBlock = TextBlock.MarkdownTextBlock(string.Format(message, stat));

            if (!buttonDisplayed)
            {
                return new BlockMessage {Type = "section", Text = textBlock};
            }

            var buttonBlock = Accessory.Button(DougMessages.AddStatPoint, type, "attribution");
            return new BlockMessage { Type = "section", Text = textBlock, Accessory = buttonBlock };
        }
    }
}
