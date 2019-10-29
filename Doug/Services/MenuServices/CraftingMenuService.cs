using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Menus;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services.MenuServices
{
    public interface ICraftingMenuService
    {
        Task ShowCraftingMenu(Interaction interaction);
        Task Craft(Interaction interaction);
    }

    public class CraftingMenuService : ICraftingMenuService
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserRepository _userRepository;
        private readonly ICraftingService _craftingService;

        public CraftingMenuService(ISlackWebApi slack, IUserRepository userRepository, ICraftingService craftingService)
        {
            _slack = slack;
            _userRepository = userRepository;
            _craftingService = craftingService;
        }

        public async Task ShowCraftingMenu(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            await _slack.UpdateInteractionMessage(new CraftingMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public async Task Craft(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            var items = user.InventoryItems.Where(item => interaction.Values.Contains(item.InventoryPosition.ToString())).ToList();

            var response = _craftingService.Craft(items, user);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);
            await _slack.UpdateInteractionMessage(new CraftingMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }
    }
}
