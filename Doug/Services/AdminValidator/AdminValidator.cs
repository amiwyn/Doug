using Doug.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    public interface IAdminValidator
    {
        Task ValidateUserIsAdmin(string userId);
    }

    public class AdminValidator : IAdminValidator
    {
        private ISlackWebApi _slack;

        public AdminValidator(ISlackWebApi slackWebApi)
        {
            _slack = slackWebApi;
        }

        public async Task ValidateUserIsAdmin(string userId)
        {
            var userInfo = await _slack.GetUserInfo(userId);

            if (!userInfo.IsAdmin)
            {
                throw new UserNotAdminException();
            }
        }
    }
}
