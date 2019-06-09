using Doug.Models;
using Doug.Repositories;
using Doug.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Commands
{
    public interface ICreditsCommands
    {
        string Balance(Command command);
        void Stats(Command command);
        void Give(Command command);
        string Gamble(Command command);
    }

    public class CreditsCommands : ICreditsCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;

        public CreditsCommands(IUserRepository userRepository, ISlackWebApi messageSender)
        {
            _userRepository = userRepository;
            _slack = messageSender;
        }
        public string Balance(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return string.Format(DougMessages.Balance, user.Credits);
        }

        public string Gamble(Command command)
        {
            throw new NotImplementedException();
        }

        public void Give(Command command)
        {
            throw new NotImplementedException();
        }

        public void Stats(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
