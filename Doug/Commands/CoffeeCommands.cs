using Doug.Controllers;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public interface ICoffeeCommands
    {
        void JoinCoffee(Command command);
        Task JoinSomeone(Command command);
        Task KickCoffee(Command command);
        Task Resolve(Command command);
        Task Skip(Command command);
    }

    public class CoffeeCommands : ICoffeeCommands
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IAdminValidator _adminValidator;
        private readonly ICoffeeBreakService _coffeeBreakService;

        public CoffeeCommands(IChannelRepository channelRepository, IUserRepository userRepository, ISlackWebApi messageSender, IAdminValidator adminValidator, ICoffeeBreakService coffeeBreakService)
        {
            _channelRepository = channelRepository;
            _userRepository = userRepository;
            _slack = messageSender;
            _adminValidator = adminValidator;
            _coffeeBreakService = coffeeBreakService;
        }

        public void JoinCoffee(Command command)
        {
            JoinUser(command.UserId, command.ChannelId);
        }

        private void JoinUser(string userId, string channelId)
        {
            _channelRepository.AddToRoster(userId);
            _userRepository.AddUser(userId);

            string text = string.Format(DougMessages.JoinedCoffee, Utils.UserMention(userId));
            _slack.SendMessage(text, channelId);
        }

        public async Task JoinSomeone(Command command)
        {
            await _adminValidator.ValidateUserIsAdmin(command.UserId);

            JoinUser(command.GetTargetUserId(), command.ChannelId);
        }

        public async Task KickCoffee(Command command)
        {
            await _adminValidator.ValidateUserIsAdmin(command.UserId);

            var targetUser = command.GetTargetUserId();
            _channelRepository.RemoveFromRoster(targetUser);

            string text = string.Format(DougMessages.KickedCoffee, Utils.UserMention(targetUser));
            _slack.SendMessage(text, command.ChannelId);
        }

        public async Task Resolve(Command command)
        {
            await _adminValidator.ValidateUserIsAdmin(command.UserId);

            _coffeeBreakService.LaunchCoffeeBreak(command.ChannelId);
        }

        public async Task Skip(Command command)
        {
            if (command.IsUserArgument())
            {
                await _adminValidator.ValidateUserIsAdmin(command.UserId);
            }


            throw new NotImplementedException();
        }
    }
}
