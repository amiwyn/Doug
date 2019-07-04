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
        private readonly IUserService _userService;

        public CoffeeCommands(ICoffeeRepository coffeeRepository, IUserRepository userRepository, ISlackWebApi messageSender, IAuthorizationService adminValidator, ICoffeeService coffeeBreakService, IUserService userService)
        {
            _coffeeRepository = coffeeRepository;
            _userRepository = userRepository;
            _slack = messageSender;
            _adminValidator = adminValidator;
            _coffeeBreakService = coffeeBreakService;
            _userService = userService;
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
            var user = _userRepository.GetUser(userId);

            string text = string.Format(DougMessages.JoinedCoffee, _userService.Mention(user));
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

            _coffeeRepository.RemoveFromRoster(command.GetTargetUserId());
            var user = _userRepository.GetUser(command.GetTargetUserId());

            string text = string.Format(DougMessages.KickedCoffee, _userService.Mention(user));
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
            string userId;

            if (command.IsUserArgument())
            {
                if (!await _adminValidator.IsUserSlackAdmin(command.UserId))
                {
                    return new DougResponse(DougMessages.NotAnAdmin);
                }

                userId = command.GetTargetUserId();
            }
            else
            {
                userId = command.UserId;
            }

            _coffeeRepository.SkipUser(userId);

            var user = _userRepository.GetUser(userId);

            await _slack.BroadcastMessage(string.Format(DougMessages.SkippedCoffee, _userService.Mention(user)), command.ChannelId);

            return NoResponse;
        }
    }
}
