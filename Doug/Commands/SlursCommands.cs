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
        Task Clean(Command command);
        string WhoLast(Command command);
        string Slurs(Command command);
    }

    public class SlursCommands : ISlursCommands
    {
        private const string SlurUserMention = "{user}";
        private const string RandomUserMention = "{random}";

        private const int SpecificFlameCost = 5;
        private const int AddSlurCreditAward = 2;
        private const int WholastCost = 2;
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

        public async Task Clean(Command command)
        {
            await _adminValidator.ValidateUserIsAdmin(command.UserId);

            var slursToRemove = await FilterSlursToRemove(command.ChannelId);

            if (slursToRemove.Count == 0)
            {
                throw new SlursAreCleanException();
            }

            var slurs = slursToRemove.Select(slur => _slurRepository.GetSlur(slur)).ToList();
            var attachment = Attachment.DeletedSlursAttachment(slurs);

            await _slack.SendAttachment(attachment, command.ChannelId);

            slursToRemove.ForEach(slur => _slurRepository.RemoveSlur(slur));

            _slurRepository.ClearRecentSlurs();
        }

        private async Task<List<int>> FilterSlursToRemove(string channelId)
        {
            var recentSlurs = _slurRepository.GetRecentSlurs();
            var slursReactions = recentSlurs.Select(slur => Tuple.Create(slur.SlurId, _slack.GetReactions(slur.TimeStamp, channelId)));

            var slursToRemove = new List<int>();

            foreach (var slurReaction in slursReactions)
            {
                var slurId = slurReaction.Item1;
                var reactionTask = slurReaction.Item2;
                int score = 0;

                var reaction = await reactionTask;

                score += reaction.Single(react => react.Name == DougMessages.UpVote).Count;
                score -= reaction.Single(react => react.Name == DougMessages.Downvote).Count;

                if (score < 0)
                {
                    slursToRemove.Add(slurId);
                }
            }

            return slursToRemove;
        }

        public async Task Flame(Command command)
        {
            var slur = command.GetArgumentCount() > 1 ? SpecificFlame(command) : RandomFlame();
            var users = _userRepository.GetUsers();

            var rnd = new Random();
            var randomUser = users.ElementAt(rnd.Next(users.Count)).Id;
            
            var message = BuildSlurMessage(slur.Text, randomUser, command.GetTargetUserId());

            var timestamp = await _slack.SendMessage(message, command.ChannelId);

            _slurRepository.LogRecentSlur(slur.Id, timestamp);

            await _slack.AddReaction(DougMessages.UpVote, timestamp, command.ChannelId);
            await _slack.AddReaction(DougMessages.Downvote, timestamp, command.ChannelId);
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

        private string BuildSlurMessage(string message, string randomUserid, string targetUserId)
        {
            message = message.Replace(SlurUserMention, Utils.UserMention(targetUserId));
            message = message.Replace(RandomUserMention, Utils.UserMention(randomUserid));

            if (message.Contains("350++"))
            {
                var fat = _slurRepository.GetFat().ToString();
                message = message.Replace("350++", fat);
                _slurRepository.IncrementFat();
            }

            return message;
        }

        public string Slurs(Command command)
        {
            var slurs = _slurRepository.GetSlursFrom(command.UserId);

            return slurs.Aggregate(string.Empty, (acc, slur) => string.Format("{0}{1} = {2}\n", acc, slur.Id, slur.Text));
        }

        public string WhoLast(Command command)
        {
            _userRepository.RemoveCredits(command.UserId, WholastCost);
            var recentSlurs = _slurRepository.GetRecentSlurs().ToList();
            recentSlurs.Sort((e1, e2) => e1.Id.CompareTo(e2.Id));

            var latestFlame = recentSlurs.Last();

            var latestSlur = _slurRepository.GetSlur(latestFlame.SlurId);

            return string.Format(DougMessages.SlurCreatedBy, Utils.UserMention(latestSlur.CreatedBy));
        }
    }
}
