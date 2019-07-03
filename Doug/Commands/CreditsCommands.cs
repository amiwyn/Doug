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

        public CreditsCommands(IUserRepository userRepository, ISlackWebApi messageSender, IItemFactory itemFactory)
        {
            _userRepository = userRepository;
            _slack = messageSender;
            _itemFactory = itemFactory;
        }

        public DougResponse Give(Command command)
        {
            var amount = int.Parse(command.GetArgumentAt(1));
            var target = command.GetTargetUserId();

            if (amount <= 0)
            {
                return new DougResponse(DougMessages.InvalidAmount);
            }

            if (!command.IsUserArgument())
            {
                return new DougResponse(DougMessages.InvalidUserArgument);
            }

            var user = _userRepository.GetUser(command.UserId);

            if (!user.HasEnoughCreditsForAmount(amount))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(amount));
            }

            _userRepository.RemoveCredits(command.UserId, amount);
            _userRepository.AddCredits(target, amount);

            var message = string.Format(DougMessages.UserGaveCredits, Utils.UserMention(command.UserId), amount, Utils.UserMention(target));
            _slack.BroadcastMessage(message, command.ChannelId);

            return NoResponse;
        }

        public DougResponse Forbes(Command command)
        {
            var users = _userRepository.GetUsers();

            return new DougResponse(users.Aggregate(string.Empty, (acc, user) => string.Format("{0}{3}{2} = {1}\n", acc, Utils.UserMention(user.Id), user.Credits, DougMessages.CreditEmoji)));
        }

        public DougResponse Leaderboard(Command command)
        {
            var list = _userRepository.GetUsers().ToList();

            list.Sort((u1, u2) => u1.Credits.CompareTo(u2.Credits));
            list.Reverse();
            list = list.GetRange(0, 5);

            var userList = list.Select(u => $"{Utils.UserMention(u.Id)} : {u.Credits}");

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
