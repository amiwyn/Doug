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
    }

    public class ChannelRepository : IChannelRepository
    {
        private DougContext _db;

        public ChannelRepository(DougContext dougContext)
        {
            this._db = dougContext;
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
    }
}
