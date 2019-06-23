using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using System;
using System.Linq;

namespace Doug.Items
{
    public interface IItemEventDispatcher
    {
        string OnFlaming(Command command, string slur);
        double OnGambling(User user, double baseChance);
        double OnStealingChance(User user, double baseChance);
        double OnGettingStolenChance(User user, double baseChance);
        int OnStealingAmount(User user, int baseAmount);
    }

    public class ItemEventDispatcher : IItemEventDispatcher
    {
        private readonly ISlackWebApi _slackWebApi;
        private readonly IUserRepository _userRepository;

        public ItemEventDispatcher(ISlackWebApi slackWebApi, IUserRepository userRepository)
        {
            _slackWebApi = slackWebApi;
            _userRepository = userRepository;
        }

        public string OnFlaming(Command command, string slur)
        {
            var caller = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());

            slur = target.Loadout.Equipment.Aggregate(slur, (acc, item) => item.Value.OnGettingFlamed(command, acc, _slackWebApi));

            return caller.Loadout.Equipment.Aggregate(slur, (acc, item) => item.Value.OnFlaming(command, acc, _slackWebApi));
        }

        public double OnGambling(User user, double baseChance)
        {
            return user.Loadout.Equipment.Aggregate(baseChance, (chance, item) => item.Value.OnGambling(chance));
        }

        public double OnStealingChance(User user, double baseChance)
        {
            return user.Loadout.Equipment.Aggregate(baseChance, (chance, item) => item.Value.OnStealingChance(chance));
        }

        public double OnGettingStolenChance(User user, double baseChance)
        {
            return user.Loadout.Equipment.Aggregate(baseChance, (chance, item) => item.Value.OnGettingStolenChance(chance));
        }

        public int OnStealingAmount(User user, int baseAmount)
        {
            return user.Loadout.Equipment.Aggregate(baseAmount, (amount, item) => item.Value.OnStealingAmount(amount));
        }
    }
}
