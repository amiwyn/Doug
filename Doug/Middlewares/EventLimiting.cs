using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Doug.Middlewares
{
    public class EventLimiting : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers.ContainsKey("X-Slack-Retry-Num"))
            {
                await context.Response.WriteAsync("OK");
                return;
            }
            await next(context);
        }
    }
}
