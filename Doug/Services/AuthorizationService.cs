using Doug.Slack;
using System.Threading.Tasks;

namespace Doug.Services
{
    public interface IAuthorizationService
    {
        Task<bool> IsUserSlackAdmin(string userId);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISlackWebApi _slack;

        public AuthorizationService(ISlackWebApi slackWebApi)
        {
            _slack = slackWebApi;
        }

        public async Task<bool> IsUserSlackAdmin(string userId)
        {
            var userInfo = await _slack.GetUserInfo(userId);

            if (!userInfo.IsAdmin)
            {
                return false;
            }

            return true;
        }
    }
}
