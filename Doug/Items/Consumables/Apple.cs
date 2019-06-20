using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class Apple : Item
    {
        private const int RecoverAmount = 25;

        public Apple()
        {
            Name = "Apple";
            Description = "Ahhh, fresh apple so healthy. Restore 25 health.";
            Rarity = Rarity.Common;
            Icon = ":apple:";
        }

        public override string Use(int itemPos, User user, IUserRepository userRepository, IStatsRepository statsRepository)
        {
            var health = user.Health + RecoverAmount;

            health = health >= user.CalculateTotalHealth() ? user.CalculateTotalHealth() : health;

            statsRepository.UpdateHealth(user.Id, health);

            userRepository.RemoveItem(user.Id, itemPos);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "health");
        }
    }
}
