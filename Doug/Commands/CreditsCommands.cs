using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Menus;
using Doug.Services;

namespace Doug.Commands
{
    public interface ICreditsCommands
    {
        DougResponse Give(Command command);
        DougResponse Forbes(Command command);
        DougResponse Leaderboard(Command command);
        Task<DougResponse> Shop(Command command);
    }

    public class CreditsCommands : ICreditsCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;

        private static readonly DougResponse NoResponse = new DougResponse();
        private readonly IItemFactory _itemFactory;
        private readonly IUserService _userService;

        public CreditsCommands(IUserRepository userRepository, ISlackWebApi messageSender, IItemFactory itemFactory, IUserService userService)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _itemFactory = itemFactory;
            _userService = userService;
        }

        public DougResponse Give(Command command)
        {
            var amount = int.Parse(command.GetArgumentAt(1));

            if (amount <= 0)
            {
                return new DougResponse(DougMessages.InvalidAmount);
            }

            if (!command.IsUserArgument())
            {
                return new DougResponse(DougMessages.InvalidUserArgument);
            }

            var user = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());

            if (!user.HasEnoughCreditsForAmount(amount))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(amount));
            }

            _userRepository.RemoveCredits(user.Id, amount);
            _userRepository.AddCredits(target.Id, amount);

            var message = string.Format(DougMessages.UserGaveCredits, _userService.Mention(user), amount, _userService.Mention(target));
            _slack.BroadcastMessage(message, command.ChannelId);

            return NoResponse;
        }

        public DougResponse Forbes(Command command)
        {
            var users = _userRepository.GetUsers();

            return new DougResponse(users.Aggregate(string.Empty, (acc, user) => string.Format("{0}{3}{2} = {1}\n", acc, _userService.Mention(user), user.Credits, DougMessages.CreditEmoji)));
        }

        public DougResponse Leaderboard(Command command)
        {
            var list = _userRepository.GetUsers().ToList();

            list.Sort((u1, u2) => u1.Credits.CompareTo(u2.Credits));
            list.Reverse();
            list = list.GetRange(0, 5);

            var userList = list.Select(u => $"{_userService.Mention(u)} : {u.Credits}");

            var users = userList.Aggregate((first, next) => first + "\n" + next);
            var message = $"{DougMessages.Top5}\n{users}";

            _slack.BroadcastMessage(message, command.ChannelId);

            return NoResponse;
        }

        public async Task<DougResponse> Shop(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            var items = ShopMenuService.ShopItems.Select(itm => _itemFactory.CreateItem(itm));

            await _slack.SendEphemeralBlocks(new ShopMenu(items, user).Blocks, command.UserId, command.ChannelId);

            return NoResponse; 
        }
    }
}
