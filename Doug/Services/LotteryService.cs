using Doug.Controllers;
using Doug.Items;
using Doug.Menus;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface ILotteryService
    {
        void StartLottery();
        void EndLottery();
        DougResponse BuyTicket(Interaction interaction);
    }

    public class LotteryService : ILotteryService
    {
        private readonly ISlackWebApi _slack;
        private readonly IUserRepository _userRepository;
        private readonly IRandomService _randomService;
        private readonly ICreditsRepository _creditsRepository;
        private readonly IUserService _userService;
        private readonly IInventoryRepository _inventoryRepository;

        private const string General = "CAZMWHXPU";
        private const int TicketPrice = 5000;

        public LotteryService(ISlackWebApi slack, IUserRepository userRepository, IRandomService randomService, ICreditsRepository creditsRepository, IUserService userService, IInventoryRepository inventoryRepository)
        {
            _slack = slack;
            _userRepository = userRepository;
            _randomService = randomService;
            _creditsRepository = creditsRepository;
            _userService = userService;
            _inventoryRepository = inventoryRepository;
        }

        public void StartLottery()
        {
            _userRepository.AddTicketsToUsers();

            _slack.BroadcastBlocks(new LotteryMenu(TicketPrice).Blocks, General);
        }

        public void EndLottery()
        {
            var users = _userRepository.GetUsers();

            var winner = _randomService.DrawLotteryWinner(users);

            _userRepository.ClearTickets();

            _inventoryRepository.AddItem(winner, new Item { Id = "lottery_box" });

            _slack.BroadcastMessage(string.Format(DougMessages.LotteryWinner, _userService.Mention(winner)), General);
        }

        public DougResponse BuyTicket(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            if (!user.HasEnoughCreditsForAmount(TicketPrice))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(TicketPrice));
            }

            _creditsRepository.RemoveCredits(user.Id, TicketPrice);

            _userRepository.AddSingleTicket(user.Id);

            return new DougResponse();
        }
    }
}
