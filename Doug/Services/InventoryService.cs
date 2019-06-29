using System.Linq;
using System.Threading.Tasks;
using Doug.Commands;
using Doug.Menus;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IInventoryService
    {
        Task Use(Interaction interaction);
        Task Equip(Interaction interaction);
        Task ShowInventory(Interaction interaction);
        Task ShowEquipment(Interaction interaction);
        Task UnEquip(Interaction interaction);
        Task Info(Interaction interaction);
    }

    public class InventoryService : IInventoryService
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryCommands _inventoryCommands;

        public InventoryService(ISlackWebApi slack, IUserRepository userRepository, IInventoryCommands inventoryCommands)
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
            var user = _userRepository.GetUser(interaction.UserId);
            var command = new Command { ChannelId = interaction.ChannelId, Text = interaction.Value.Split(":").Last(), UserId = interaction.UserId };

            var message = _inventoryCommands.Equip(command).Message;

            await _slack.SendEphemeralMessage(message, interaction.UserId, interaction.ChannelId);

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

        public Task Info(Interaction interaction)
        {
            throw new System.NotImplementedException();
        }
    }
}
