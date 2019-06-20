using Doug.Models;
using Doug.Repositories;

namespace Doug.Items.Consumables
{
    public class NormalEnergyDrink : Item
    {
        private const int RecoverAmount = 25;

        public NormalEnergyDrink()
        {
            Name = "Coffee";
            Description = "A good cuppa. Restore 25 energy.";
            Rarity = Rarity.Common;
            Icon = ":coffee:";
        }

        public override string Use(int itemPos, User user, IUserRepository userRepository, IStatsRepository statsRepository)
        {
            var energy = user.Energy + RecoverAmount;

            energy = energy >= user.CalculateTotalEnergy() ? user.CalculateTotalEnergy() : energy;

            statsRepository.UpdateEnergy(user.Id, energy);

            userRepository.RemoveItem(user.Id, itemPos);

            return string.Format(DougMessages.RecoverItem, Name, RecoverAmount, "energy");
        }
    }
}
