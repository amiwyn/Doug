using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    public interface IChannelRepository
    {
        void AddToRoster(string userId);
        string GetAccessToken();
        void RemoveFromRoster(string userId);
    }

    public class ChannelRepository : IChannelRepository
    {
        private DougContext _db;

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

        public string GetAccessToken()
        {
            return _db.Channel.Single().Token;
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
    }
}
