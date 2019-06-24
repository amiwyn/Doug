using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IShopService
    {
        void Buy(Interaction interaction);
    }

    public class ShopService : IShopService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IInventoryRepository _inventoryRepository;

        public ShopService(IUserRepository userRepository, ISlackWebApi slack, IInventoryRepository inventoryRepository)
        {
            _userRepository = userRepository;
            _slack = slack;
            _inventoryRepository = inventoryRepository;
        }

        public void Buy(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var item = ItemFactory.CreateItem(interaction.Value);

            if (!user.HasEnoughCreditsForAmount(item.Price))
            {
                var message = user.NotEnoughCreditsForAmountResponse(item.Price);
                _slack.SendEphemeralMessage(message, user.Id, interaction.ChannelId);
                return;
            }

            _userRepository.RemoveCredits(user.Id, item.Price);

            _inventoryRepository.AddItem(user.Id, item.Id);
        }
    }
}
