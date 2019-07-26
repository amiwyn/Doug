using System.Linq;
using System.Threading.Tasks;
using Doug.Commands;
using Doug.Menus;
using Doug.Menus.Blocks;
using Doug.Menus.Blocks.Accessories;
using Doug.Menus.Blocks.Text;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services.MenuServices
{
    public interface IInventoryMenuService
    {
        Task Use(Interaction interaction);
        Task Equip(Interaction interaction);
        Task ShowInventory(Interaction interaction);
        Task ShowEquipment(Interaction interaction);
        Task UnEquip(Interaction interaction);
        Task ShowUserSelect(Interaction interaction);
        Task Give(Interaction interaction);
        Task Target(Interaction interaction);
        Task Info(Interaction interaction);
    }

    public class InventoryMenuService : IInventoryMenuService
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryCommands _inventoryCommands;

        public InventoryMenuService(ISlackWebApi slack, IUserRepository userRepository, IInventoryCommands inventoryCommands)
        {
            _slack = slack;
            _userRepository = userRepository;
            _inventoryCommands = inventoryCommands;
        }

        public async Task Use(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var command = new Command { ChannelId = interaction.ChannelId, Text = interaction.Value.Split(":").Last(), UserId = interaction.UserId };

            var message = _inventoryCommands.Use(command).Message;

            await _slack.SendEphemeralMessage(message, interaction.UserId, interaction.ChannelId);
            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public async Task Equip(Interaction interaction)
        {
            var command = new Command { ChannelId = interaction.ChannelId, Text = interaction.Value.Split(":").Last(), UserId = interaction.UserId };

            var message = _inventoryCommands.Equip(command).Message;

            await _slack.SendEphemeralMessage(message, interaction.UserId, interaction.ChannelId);

            var user = _userRepository.GetUser(interaction.UserId);
            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public async Task ShowInventory(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public async Task ShowEquipment(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            await _slack.UpdateInteractionMessage(new EquipmentMenu(user.Loadout).Blocks, interaction.ResponseUrl);
        }

        public async Task UnEquip(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var command = new Command { ChannelId = interaction.ChannelId, Text = interaction.Value.Split(":").Last(), UserId = interaction.UserId };

            var message = _inventoryCommands.UnEquip(command).Message;

            await _slack.SendEphemeralMessage(message, interaction.UserId, interaction.ChannelId);

            await _slack.UpdateInteractionMessage(new EquipmentMenu(user.Loadout).Blocks, interaction.ResponseUrl);
        }

        public async Task ShowUserSelect(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var blocks = new InventoryMenu(user.InventoryItems).Blocks;

            var select = new UsersSelect(DougMessages.SelectTarget, $"{interaction.Value}");
            blocks.Add(new Section(new MarkdownText(DougMessages.SelectTargetText), select));
            
            await _slack.UpdateInteractionMessage(blocks, interaction.ResponseUrl);
        }

        public async Task Give(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var target = $"<@{interaction.Value}|user>";
            var slot = interaction.GetValueFromAction();
            var command = new Command { ChannelId = interaction.ChannelId, Text = $"{target} {slot}", UserId = interaction.UserId };

            var message = _inventoryCommands.Give(command).Message;

            await _slack.SendEphemeralMessage(message, interaction.UserId, interaction.ChannelId);

            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public async Task Target(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var target = $"<@{interaction.Value}|user>";
            var slot = interaction.GetValueFromAction();
            var command = new Command { ChannelId = interaction.ChannelId, Text = $"{target} {slot}", UserId = interaction.UserId };

            var message = _inventoryCommands.Target(command).Message;

            await _slack.SendEphemeralMessage(message, interaction.UserId, interaction.ChannelId);

            await _slack.UpdateInteractionMessage(new InventoryMenu(user.InventoryItems).Blocks, interaction.ResponseUrl);
        }

        public Task Info(Interaction interaction)
        {
            throw new System.NotImplementedException();
        }
    }
}
