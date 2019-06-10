using Doug.Commands;
using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    public interface IChannelRepository
    {
        string GetAccessToken();
        string GetRemindJob();
        void SetRemindJob(string jobId);
        void SendGambleChallenge(GambleChallenge challenge);
        GambleChallenge GetGambleChallenge(string target);
        void RemoveGambleChallenge(string target);
    }

    public class ChannelRepository : IChannelRepository
    {
        private readonly DougContext _db;

        public ChannelRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public void AddToRoster(string userId)
        {
            if (!_db.Roster.Any(user => user.Id == userId))
            {
                _db.Roster.Add(new Roster() { Id = userId });
                _db.SaveChanges();
            }
        }

        public void ConfirmUserReady(string userId)
        {
            var user = _db.Roster.SingleOrDefault(usr => usr.Id == userId);
            if (user != null)
            {
                user.IsReady = true;
                _db.SaveChanges();
            }
        }

        public string GetAccessToken()
        {
            return _db.Channel.Single().Token;
        }

        public GambleChallenge GetGambleChallenge(string target)
        {
            var challenge =  _db.GambleChallenges.SingleOrDefault(cha => cha.TargetId == target);

            if (challenge == null)
            {
                throw new UserNotChallengedException();
            }

            return challenge;
        }

        public string GetRemindJob()
        {
            return _db.Channel.Single().CoffeeRemindJobId;
        }

        public void RemoveFromRoster(string userId)
        {
            var user = _db.Roster.SingleOrDefault(usr => usr.Id == userId);
            if (user != null)
            {
                _db.Roster.Remove(user);
                _db.SaveChanges();
            }
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

        public void SendGambleChallenge(GambleChallenge challenge)
        {
            if (_db.GambleChallenges.Any(cha => cha.TargetId == challenge.TargetId))
            {
                throw new UserAlreadyChallengedException();
            }

            _db.GambleChallenges.Add(challenge);
            _db.SaveChanges();
        }

        public void SetRemindJob(string jobId)
        {
            _db.Channel.Single().CoffeeRemindJobId = jobId;
            _db.SaveChanges();
        }

        public void SkipUser(string userId)
        {
            var user = _db.Roster.SingleOrDefault(usr => usr.Id == userId);
            if (user != null)
            {
                user.IsSkipping = true;
                _db.SaveChanges();
            }
        }
    }
}
