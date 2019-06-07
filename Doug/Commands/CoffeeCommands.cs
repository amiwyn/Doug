using Doug.Models;
using Doug.Repositories;
using Doug.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public interface ICoffeeCommands
    {
        void JoinCoffee(Command command);
        void JoinSomeone(Command command);
        void KickCoffee(Command command);
        void Resolve(Command command);
        void Skip(Command command);
    }

    public class CoffeeCommands : ICoffeeCommands
    {
        private IChannelRepository channelRepository;
        private IUserRepository userRepository;
        private IMessageSender slack;

        public CoffeeCommands(IChannelRepository channelRepository, IUserRepository userRepository, IMessageSender messageSender)
        {
            this.channelRepository = channelRepository;
            this.userRepository = userRepository;
            this.slack = messageSender;
        }

        public void JoinCoffee(Command command)
        {
            channelRepository.AddToRoster(command.UserId);
            userRepository.AddUser(command.UserId);

            string text = string.Format("{0} joined coffee break", Utils.UserMention(command.UserId));
            slack.SendMessage(text, command.ChannelId);
        }

        public void JoinSomeone(Command command)
        {
            throw new NotImplementedException();
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
