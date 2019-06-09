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
    public interface ISlursCommands
    {
        Task Flame(Command command);
        string AddSlur(Command command);
        string Clean(Command command);
        string WhoLast(Command command);
        string Slurs(Command command);
    }

    public class SlursCommands : ISlursCommands
    {
        private const string SlurUserMention = "{user}";
        private const string RandomUserMention = "{random}";

        private const int SpecificFlameCost = 5;
        private const int AddSlurCreditAward = 2;

        private readonly ISlurRepository _slurRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IAdminValidator _adminValidator;

        public SlursCommands(ISlurRepository slursRepository, IUserRepository userRepository, ISlackWebApi messageSender, IAdminValidator adminValidator)
        {
            _slurRepository = slursRepository;
            _userRepository = userRepository;
            _slack = messageSender;
            _adminValidator = adminValidator;
        }

        public string AddSlur(Command command)
        {
            var slur = new Slur(command.Text, command.UserId);

            _slurRepository.AddSlur(slur);

            _userRepository.AddCredits(command.UserId, AddSlurCreditAward);

            return string.Format(DougMessages.GainedCredit, AddSlurCreditAward);
        }

        public string Clean(Command command)
        {
            throw new NotImplementedException();
        }

        public async Task Flame(Command command)
        {
            var slur = command.GetArgumentCount() > 1 ? SpecificFlame(command) : RandomFlame();
            var users = _userRepository.GetUsers();

            var rnd = new Random();
            var randomUser = users.ElementAt(rnd.Next(users.Count)).Id;
            var message = slur.Text;

            message = message.Replace(SlurUserMention, Utils.UserMention(command.GetTargetUserId()));
            message = message.Replace(RandomUserMention, Utils.UserMention(randomUser));

            if (message.Contains("350++"))
            {
                var fat = _slurRepository.GetFat().ToString();
                message = message.Replace("350++", fat);
                _slurRepository.IncrementFat();
            }

            var timestamp = await _slack.SendMessage(message, command.ChannelId);

            _slurRepository.LogRecentSlur(slur.Id, timestamp);

            await _slack.AddReaction("+1", timestamp, command.ChannelId);
            await _slack.AddReaction("-1", timestamp, command.ChannelId);
        }

        private Slur SpecificFlame(Command command)
        {
            int slurId = int.Parse(command.GetArgumentAt(1));

            _userRepository.RemoveCredits(command.UserId, SpecificFlameCost);

            return _slurRepository.GetSlur(slurId);
        }

        private Slur RandomFlame()
        {
            var slurs = _slurRepository.GetSlurs();
            
            var rnd = new Random();
            return slurs.ElementAt(rnd.Next(slurs.Count));
        }

        public string Slurs(Command command)
        {
            throw new NotImplementedException();
        }

        public string WhoLast(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
