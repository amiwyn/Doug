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
        double OnGambling(User user);
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
            throw new NotImplementedException();
        }

        public double OnGambling(User user)
        {
            return user.InventoryItems.Aggregate(user.CalculateBaseGambleChance(), (chance, userItem) => userItem.Item.OnGambling(chance));
        }

        public string OnGettingFlamed(Command command, string slur)
        {
            var user = _userRepository.GetUser(command.GetTargetUserId());

            return user.InventoryItems.Aggregate(slur, (acc, userItem) => userItem.Item.OnGettingFlamed(command, acc, _slackWebApi));
        }
    }
}
