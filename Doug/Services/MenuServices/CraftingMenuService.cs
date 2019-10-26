using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Menus;
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

        public CraftingMenuService(ISlackWebApi slack, IUserRepository userRepository)
        {
            _slack = slack;
            _userRepository = userRepository;
        }

        public async Task ShowCraftingMenu(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            await _slack.UpdateInteractionMessage(new CraftingMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public Task Craft(Interaction interaction)
        {
            throw new System.NotImplementedException();
        }
    }
}
