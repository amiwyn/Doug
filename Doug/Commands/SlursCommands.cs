using Doug.Items;
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
        Task<DougResponse> Flame(Command command);
        DougResponse AddSlur(Command command);
        Task<DougResponse> Clean(Command command);
        DougResponse WhoLast(Command command);
        DougResponse Slurs(Command command);
    }

    public class SlursCommands : ISlursCommands
    {
        private const string SlurUserMention = "{user}";
        private const string RandomUserMention = "{random}";

        private const int SpecificFlameCost = 5;
        private const int AddSlurCredit = 2;
        private const int WholastCost = 2;
        private const string Fatty = "350++";
        private readonly ISlurRepository _slurRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IAuthorizationService _adminValidator;
        private readonly IEventDispatcher _eventDispatcher;

        private static readonly DougResponse NoResponse = new DougResponse();
        private readonly IUserService _userService;

        public SlursCommands(ISlurRepository slursRepository, IUserRepository userRepository, ISlackWebApi messageSender, IAuthorizationService adminValidator, IEventDispatcher eventDispatcher, IUserService userService)
        {
            _slurRepository = slursRepository;
            _userRepository = userRepository;
            _slack = messageSender;
            _adminValidator = adminValidator;
            _eventDispatcher = eventDispatcher;
            _userService = userService;
        }

        public DougResponse AddSlur(Command command)
        {
            var existingSlur = _slurRepository.GetSlurByText(command.Text);

            if (existingSlur != null)
            {
                return new DougResponse(DougMessages.SlurAlreadyExists);
            }

            var slur = new Slur(command.Text, command.UserId);

            _slurRepository.AddSlur(slur);

            _userRepository.AddCredits(command.UserId, AddSlurCredit);

            return new DougResponse(string.Format(DougMessages.GainedCredit, AddSlurCredit));
        }

        public async Task<DougResponse> Clean(Command command)
        {
            if (!await _adminValidator.IsUserSlackAdmin(command.UserId))
            {
                return new DougResponse(DougMessages.NotAnAdmin);
            }

            var slursToRemove = await FilterSlursToRemove(command.ChannelId);

            if (slursToRemove.Count == 0)
            {
                return new DougResponse(DougMessages.SlursAreClean);
            }

            var slurs = slursToRemove.Select(slur => _slurRepository.GetSlur(slur)).ToList();
            var attachment = Attachment.DeletedSlursAttachment(slurs);

            await _slack.SendAttachments(new List<Attachment>{ attachment } , command.ChannelId);

            slursToRemove.ForEach(slur => _slurRepository.RemoveSlur(slur));

            _slurRepository.ClearRecentSlurs();

            slurs.ForEach(slur => _userRepository.RemoveCredits(slur.CreatedBy, AddSlurCredit));

            return NoResponse;
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

        public async Task<DougResponse> Flame(Command command)
        {
            if (command.GetArgumentCount() > 1)
            {
                return await SpecificFlame(command);
            }

            return await RandomFlame(command);
        }

        private async Task<DougResponse> SpecificFlame(Command command)
        {
            var slurId = int.Parse(command.GetArgumentAt(1));

            var user = _userRepository.GetUser(command.UserId);

            if (!user.HasEnoughCreditsForAmount(SpecificFlameCost))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(SpecificFlameCost));
            }

            _userRepository.RemoveCredits(command.UserId, SpecificFlameCost);

            var slur = _slurRepository.GetSlur(slurId);

            await SendSlurToChannel(command, slur);

            return NoResponse;
        }

        private async Task<DougResponse> RandomFlame(Command command)
        {
            var slurs = _slurRepository.GetSlurs();
            
            var rnd = new Random();
            var slur = slurs.ElementAt(rnd.Next(slurs.Count));

            await SendSlurToChannel(command, slur);

            return NoResponse;
        }

        private async Task SendSlurToChannel(Command command, Slur slur)
        {
            var users = _userRepository.GetUsers();
            var caller = users.Single(usr => usr.Id == command.UserId);
            var target = users.Single(usr => usr.Id == command.GetTargetUserId());

            var rnd = new Random();
            var randomUser = users.ElementAt(rnd.Next(users.Count));

            var message = BuildSlurMessage(slur.Text, randomUser, target);

            message = _eventDispatcher.OnFlaming(caller, target, command, message);

            var timestamp = await _slack.BroadcastMessage(message, command.ChannelId);

            _slurRepository.LogRecentSlur(slur.Id, timestamp);

            await _slack.AddReaction(DougMessages.UpVote, timestamp, command.ChannelId);
            await _slack.AddReaction(DougMessages.Downvote, timestamp, command.ChannelId);
        }

        private string BuildSlurMessage(string message, User randomUser, User targetUser)
        {
            message = message.Replace(SlurUserMention, _userService.Mention(targetUser));
            message = message.Replace(RandomUserMention, _userService.Mention(randomUser));

            if (message.Contains(Fatty))
            {
                var fat = _slurRepository.GetFat().ToString();
                message = message.Replace(Fatty, fat);
                _slurRepository.IncrementFat();
            }

            return message;
        }

        public DougResponse Slurs(Command command)
        {
            var slurs = _slurRepository.GetSlursFrom(command.UserId);

            return new DougResponse(slurs.Aggregate(string.Empty, (acc, slur) => string.Format("{0}{1} = {2}\n", acc, slur.Id, slur.Text)));
        }

        public DougResponse WhoLast(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            if (!user.HasEnoughCreditsForAmount(WholastCost))
            {
                return new DougResponse(user.NotEnoughCreditsForAmountResponse(WholastCost));
            }

            _userRepository.RemoveCredits(command.UserId, WholastCost);
            var recentSlurs = _slurRepository.GetRecentSlurs().ToList();
            recentSlurs.Sort((e1, e2) => e1.Id.CompareTo(e2.Id));

            var latestFlame = recentSlurs.Last();
            var latestSlur = _slurRepository.GetSlur(latestFlame.SlurId);
            var creator = _userRepository.GetUser(latestSlur.CreatedBy);

            return new DougResponse(string.Format(DougMessages.SlurCreatedBy, _userService.Mention(creator)));
        }
    }
}
