using System;
using Doug.Models;
using System.Linq;

namespace Doug.Repositories
{
    public interface IChannelRepository
    {
        void GetAccessTokens(out string bot, out string user);
        void SendGambleChallenge(GambleChallenge challenge);
        GambleChallenge GetGambleChallenge(string target);
        void RemoveGambleChallenge(string target);
        ChannelType GetChannelType(string channelId);
    }

    public class ChannelRepository : IChannelRepository
    {
        private readonly DougContext _db;

        public ChannelRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public GambleChallenge GetGambleChallenge(string target)
        {
            return _db.GambleChallenges.SingleOrDefault(cha => cha.TargetId == target);
        }

        public void RemoveGambleChallenge(string target)
        {
            var challenge = _db.GambleChallenges.SingleOrDefault(cha => cha.TargetId == target);
            if (challenge != null)
            {
                _db.GambleChallenges.Remove(challenge);
                _db.SaveChanges();
            }
        }

        public ChannelType GetChannelType(string channelId)
        {
            var typeString = _db.Channels.Single(channel => channel.Id == channelId).Type;
            Enum.TryParse(typeString, out ChannelType channelType);
            return channelType;
        }

        public void GetAccessTokens(out string bot, out string user)
        {
            var coffee = _db.CoffeeBreak.Single();
            bot = coffee.BotToken;
            user = coffee.UserToken;
        }

        public void SendGambleChallenge(GambleChallenge challenge)
        {
            _db.GambleChallenges.Add(challenge);
            _db.SaveChanges();
        }
    }
}
