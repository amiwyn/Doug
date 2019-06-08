using Doug.Models;
using Doug.Repositories;
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
        void KickCoffee(Command command);
        void Resolve(Command command);
        void Skip(Command command);
    }

    public class CoffeeCommands : ICoffeeCommands
    {
        private IChannelRepository _channelRepository;
        private IUserRepository _userRepository;
        private ISlackWebApi _slack;

        public CoffeeCommands(IChannelRepository channelRepository, IUserRepository userRepository, ISlackWebApi messageSender)
        {
            this._channelRepository = channelRepository;
            this._userRepository = userRepository;
            this._slack = messageSender;
        }

        public void JoinCoffee(Command command)
        {
            JoinUser(command.UserId, command.ChannelId);
        }

        private void JoinUser(string userId, string channelId)
        {
            _channelRepository.AddToRoster(userId);
            _userRepository.AddUser(userId);

            string text = string.Format("{0} joined coffee break", Utils.UserMention(userId));
            _slack.SendMessage(text, channelId);
        }

        public async Task JoinSomeone(Command command)
        {
            bool isAdmin = await _userRepository.IsAdmin(command.UserId);
            if (!isAdmin)
            {
                throw new Exception("you are not an admin");
            }

            JoinUser(command.GetTargetUserId(), command.ChannelId);
        }

        public void KickCoffee(Command command)
        {
            throw new NotImplementedException();
        }

        public void Resolve(Command command)
        {
            throw new NotImplementedException();
        }

        public void Skip(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
