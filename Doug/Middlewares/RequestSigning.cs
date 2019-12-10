using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Doug.Middlewares
{
    public class RequestSigning : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers["x-slack-request-timestamp"]) ||
                string.IsNullOrWhiteSpace(context.Request.Headers["x-slack-signature"]))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid signature");
                return;
            }

            string slackSignature = context.Request.Headers["x-slack-signature"];
            var timestamp = long.Parse(context.Request.Headers["x-slack-request-timestamp"]);
            var signingSecret = Environment.GetEnvironmentVariable("SLACK_SIGNING_SECRET");
            string content;

            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                content = await reader.ReadToEndAsync();
            }

            context.Request.Body.Position = 0;

            var sigBase = $"v0:{timestamp}:{content}";

            var encoding = new UTF8Encoding();

            var hmac = new HMACSHA256(encoding.GetBytes(signingSecret));
            var hashBytes = hmac.ComputeHash(encoding.GetBytes(sigBase));

            var generatedSignature = "v0=" + BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            if (generatedSignature == slackSignature)
            {
                await next(context);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Request signing failed");
            }

            hmac.Dispose();
        }
    }
}
