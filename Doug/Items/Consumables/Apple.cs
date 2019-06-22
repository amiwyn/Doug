using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class Apple : ConsumableItem
    {
        private const int RecoverAmount = 25;

        public Apple()
        {
            Name = "Apple";
            Description = "Ahhh, a fresh apple. So healthy. Restore 25 health.";
            Rarity = Rarity.Common;
            Icon = ":apple:";
        }

        public override string Use(int itemPos, User user, IUserRepository userRepository, IStatsRepository statsRepository)
        {
            base.Use(itemPos, user, userRepository, statsRepository);

            var health = user.Health + RecoverAmount;
            health = health >= user.TotalHealth ? user.TotalHealth : health;

            statsRepository.UpdateHealth(user.Id, health);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "health");
        }
    }
}
