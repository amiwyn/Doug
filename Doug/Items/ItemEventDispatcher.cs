using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using System;
using System.Linq;

namespace Doug.Items
{
    public interface IItemEventDispatcher
    {
        string OnGettingFlamed(Command command, string slur);
        string OnFlaming(Command command, string slur);
        double OnGambling(User user, double baseChance);
        double OnStealingChance(User user, double baseChance);
        int OnStealingAmount(User user, int baseAmount);
        void OnConsuming(User user, int energy);
    }

    public class ItemEventDispatcher : IItemEventDispatcher
    {
        private const int MaximumEnergy = 25;
        private readonly ISlackWebApi _slackWebApi;
        private readonly IUserRepository _userRepository;

        public ItemEventDispatcher(ISlackWebApi slackWebApi, IUserRepository userRepository)
        {
            _slackWebApi = slackWebApi;
            _userRepository = userRepository;
        }

        public string OnFlaming(Command command, string slur)
        {
            throw new NotImplementedException();
        }

        public double OnGambling(User user, double baseChance)
        {
            return user.InventoryItems.Aggregate(baseChance, (chance, userItem) => userItem.Item.OnGambling(chance));
        }

        public double OnStealingChance(User user, double baseChance)
        {
            return user.InventoryItems.Aggregate(baseChance, (chance, userItem) => userItem.Item.OnStealingChance(chance));
        }

        public int OnStealingAmount(User user, int baseAmount)
        {
            return user.InventoryItems.Aggregate(baseAmount, (amount, userItem) => userItem.Item.OnStealingAmount(amount));
        }

        public string OnGettingFlamed(Command command, string slur)
        {
            var user = _userRepository.GetUser(command.GetTargetUserId());

            return user.InventoryItems.Aggregate(slur, (acc, userItem) => userItem.Item.OnGettingFlamed(command, acc, _slackWebApi));
        }

        public void OnConsuming(User user, int energy)
        {
            throw new NotImplementedException();
        }
    }
}
