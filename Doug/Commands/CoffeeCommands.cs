using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public interface ICoffeeCommands
    {
        DougResponse JoinCoffee(Command command);
        Task<DougResponse> JoinSomeone(Command command);
        Task<DougResponse> KickCoffee(Command command);
        Task<DougResponse> Resolve(Command command);
        Task<DougResponse> Skip(Command command);
    }

    public class CoffeeCommands : ICoffeeCommands
    {
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IAuthorizationService _adminValidator;
        private readonly ICoffeeService _coffeeBreakService;

        private static readonly DougResponse NoResponse = new DougResponse();

        public CoffeeCommands(ICoffeeRepository coffeeRepository, IUserRepository userRepository, ISlackWebApi messageSender, IAuthorizationService adminValidator, ICoffeeService coffeeBreakService)
        {
            _coffeeRepository = coffeeRepository;
            _userRepository = userRepository;
            _slack = messageSender;
            _adminValidator = adminValidator;
            _coffeeBreakService = coffeeBreakService;
        }

        public DougResponse JoinCoffee(Command command)
        {
            JoinUser(command.UserId, command.ChannelId);

            return NoResponse;
        }

        private void JoinUser(string userId, string channelId)
        {
            _coffeeRepository.AddToRoster(userId);
            _userRepository.AddUser(userId);

            string text = string.Format(DougMessages.JoinedCoffee, Utils.UserMention(userId));
            _slack.BroadcastMessage(text, channelId);
        }

        public async Task<DougResponse> JoinSomeone(Command command)
        {
            if (!await _adminValidator.IsUserSlackAdmin(command.UserId))
            {
                return new DougResponse(DougMessages.NotAnAdmin);
            }

            JoinUser(command.GetTargetUserId(), command.ChannelId);

            return NoResponse;
        }

        public async Task<DougResponse> KickCoffee(Command command)
        {
            if (!await _adminValidator.IsUserSlackAdmin(command.UserId))
            {
                return new DougResponse(DougMessages.NotAnAdmin);
            }

            var targetUser = command.GetTargetUserId();
            _coffeeRepository.RemoveFromRoster(targetUser);

            string text = string.Format(DougMessages.KickedCoffee, Utils.UserMention(targetUser));
            await _slack.BroadcastMessage(text, command.ChannelId);

            return NoResponse;
        }

        public async Task<DougResponse> Resolve(Command command)
        {
            if (!await _adminValidator.IsUserSlackAdmin(command.UserId))
            {
                return new DougResponse(DougMessages.NotAnAdmin);
            }

            _coffeeBreakService.LaunchCoffeeBreak(command.ChannelId);

            return NoResponse;
        }

        public async Task<DougResponse> Skip(Command command)
        {
            string user;

            if (command.IsUserArgument())
            {
                if (!await _adminValidator.IsUserSlackAdmin(command.UserId))
                {
                    return new DougResponse(DougMessages.NotAnAdmin);
                }

                user = command.GetTargetUserId();
            }
            else
            {
                user = command.UserId;
            }

            _coffeeRepository.SkipUser(user);
            await _slack.BroadcastMessage(string.Format(DougMessages.SkippedCoffee, Utils.UserMention(user)), command.ChannelId);

            return NoResponse;
        }
    }
}
