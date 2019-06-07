using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Doug.Models;
using Doug.Repositories;

namespace Doug.Services
{
    public interface IMessageSender
    {
        void SendMessage(string text, string channelId);
    }
    public class SlackWebApi : IMessageSender
    {
        private const string PostMessageUrl = "https://slack.com/api/chat.postMessage";

        private HttpClient client;
        private string token;

        public SlackWebApi(HttpClient client, IChannelRepository channelRepository)
        {
            this.client = client;
            this.token = channelRepository.GetAccessToken();
        }

        public void SendMessage(string text, string channelId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, PostMessageUrl);
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("token", token));
            keyValues.Add(new KeyValuePair<string, string>("text", text));
            keyValues.Add(new KeyValuePair<string, string>("channel", channelId));
            request.Content = new FormUrlEncodedContent(keyValues);

            client.SendAsync(request);
        }
    }
}
