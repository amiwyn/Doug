using System.Linq;
using System.Threading.Tasks;
using Doug.Slack;
using Microsoft.AspNetCore.Http;

namespace Doug.Middlewares
{
    public class Authentication : IMiddleware
    {
        private readonly DougContext _db;
        private readonly ISlackWebApi _slack;

        public Authentication(DougContext db, ISlackWebApi slack)
        {
            _db = db;
            _slack = slack;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"].ToString()))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid token");
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Substring(7);
            var user = _db.Users.SingleOrDefault(usr => usr.Token == token);

            if (user == null)
            {
                var userIdentity = await _slack.Identify(token);

                if (userIdentity?.Id == null)
                {
                    context.Response.StatusCode = 403;
                    return;
                }

                await UpdateUserToken(userIdentity.Id, token);
            }

            await next(context);
        }

        private async Task UpdateUserToken(string userId, string token)
        {
            var user = _db.Users.SingleOrDefault(usr => usr.Id == userId);

            if (user != null)
            {
                user.Token = token;
                await _db.SaveChangesAsync();
            }
        }
    }
}
