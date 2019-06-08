using Doug.Models;
using Doug.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string userId);
        Task<bool> IsAdmin(string userId);
    }
    public class UserRepository : IUserRepository
    {
        private readonly DougContext _db;
        private readonly ISlackWebApi _slackWebApi;

        public UserRepository(DougContext dougContext, ISlackWebApi slackWebApi)
        {
            _db = dougContext;
            _slackWebApi = slackWebApi;
        }

        public void AddUser(string userId)
        {
            if (!_db.Users.Any(user => user.Id == userId)) {

                var user = new User()
                {
                    Id = userId,
                    Credits = 10
                };

                _db.Users.Add(user);
                _db.SaveChanges();
            }
        }

        public async Task<bool> IsAdmin(string userId)
        {
            var userinfo = await _slackWebApi.GetUserInfo(userId);
            return userinfo.IsAdmin;
        }
    }
}
