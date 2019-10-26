using System;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Items.Equipment;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface IGovernmentService
    {
        void CollectSalesTaxes(Item item);
        int GetPriceWithTaxes(Item item);
        Task<DougResponse> StartRevolutionVote(User leader, string channel);
        void Revolution(string channel);
        Task CountVotes(string timestamp, string channel);
    }

    public class GovernmentService : IGovernmentService
    {
        private readonly IGovernmentRepository _governmentRepository;
        private readonly ISlackWebApi _slack;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public GovernmentService(IGovernmentRepository governmentRepository, ISlackWebApi slack, IUserService userService, IUserRepository userRepository, IEquipmentRepository equipmentRepository, IInventoryRepository inventoryRepository)
        {
            _governmentRepository = governmentRepository;
            _slack = slack;
            _userService = userService;
            _userRepository = userRepository;
            _equipmentRepository = equipmentRepository;
            _inventoryRepository = inventoryRepository;
        }

        public void CollectSalesTaxes(Item item)
        {
            var amount = CalculateTaxAmount(item.Price);
            _governmentRepository.AddTaxesToRuler(amount);
        }

        public int GetPriceWithTaxes(Item item)
        {
            var taxes = CalculateTaxAmount(item.Price);
            return item.Price + taxes;
        }

        public async Task<DougResponse> StartRevolutionVote(User leader, string channel)
        {
            var government = _governmentRepository.GetGovernment();

            if (government.IsInRevolutionCooldown())
            {
                return new DougResponse(string.Format(DougMessages.RevolutionCooldown, government.CalculateRevolutionCooldown()));
            }

            var timestamp = await _slack.BroadcastMessage(string.Format(DougMessages.RevolutionVote, _userService.Mention(leader)), channel);
            await _slack.AddReaction(DougMessages.UpVote, timestamp, channel);
            await _slack.AddReaction(DougMessages.Downvote, timestamp, channel);

            _governmentRepository.StartRevolutionVote(leader.Id, timestamp);

            return new DougResponse();
        }

        public void Revolution(string channel)
        {
            var government = _governmentRepository.GetGovernment();
            var oldRuler = _userRepository.GetUser(government.Ruler);
            var newRuler = _userRepository.GetUser(government.RevolutionLeader);

            if (oldRuler.Id == newRuler.Id)
            {
                return;
            }

            if (government.IsInRevolutionCooldown())
            {
                return;
            }

            if (oldRuler.Loadout.Head == Crown.ItemId)
            {
                _equipmentRepository.UnequipItem(oldRuler, EquipmentSlot.Head);
            }
            else
            {
                var crownItem = oldRuler.InventoryItems.SingleOrDefault(inventoryItem => inventoryItem.Item.Id == Crown.ItemId);

                if (crownItem != null)
                {
                    _inventoryRepository.RemoveItem(oldRuler, crownItem.InventoryPosition);
                }
            }

            _inventoryRepository.AddItem(newRuler, new Crown());

            _slack.BroadcastMessage(string.Format(DougMessages.RevolutionSucceeded, _userService.Mention(oldRuler), _userService.Mention(newRuler)), channel);

            _userService.KillUser(oldRuler, channel);

            _governmentRepository.Revolution();
        }

        public async Task CountVotes(string timestamp, string channel)
        {
            var users = _userRepository.GetUsers();
            var reactions = await _slack.GetReactions(timestamp, channel);
            var upVote = reactions.SingleOrDefault(reaction => reaction.Name == DougMessages.UpVote);

            if (upVote != null && upVote.Count >= users.Count / 2)
            {
                Revolution(channel);
            }
        }

        private int CalculateTaxAmount(int amount)
        {
            var rate = _governmentRepository.GetTaxRate();
            return (int)Math.Ceiling(amount * rate);
        }
    }
}
