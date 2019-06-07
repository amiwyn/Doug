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
        private DougContext db;

        public ChannelRepository(DougContext dougContext)
        {
            this.db = dougContext;
        }

        public void AddToRoster(string userId)
        {
            if (!db.Roster.Any(user => user.Id == userId))
            {
                db.Roster.Add(new Roster() { Id = userId });
                db.SaveChanges();
            }
        }

        public string GetAccessToken()
        {
            return db.Channel.Single().Token;
        }
    }
}
