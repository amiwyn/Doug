using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Doug.Models;
using Doug.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Doug.Slack
{
    public interface ISlackWebApi
    {
        void SendMessage(string text, string channelId);
        Task<UserInfo> GetUserInfo(string userId);
    }
    public class SlackWebApi : ISlackWebApi
    {
        private const string PostMessageUrl = "https://slack.com/api/chat.postMessage";

        private readonly HttpClient _client;
        private readonly string _token;
        private readonly JsonSerializerSettings _jsonSettings;

        public SlackWebApi(HttpClient client, IChannelRepository channelRepository)
        {
            _client = client;
            _token = channelRepository.GetAccessToken();

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
        }

        public void SendMessage(string text, string channelId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, PostMessageUrl);
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("token", _token));
            keyValues.Add(new KeyValuePair<string, string>("text", text));
            keyValues.Add(new KeyValuePair<string, string>("channel", channelId));
            request.Content = new FormUrlEncodedContent(keyValues);

            _client.SendAsync(request);
        }

        public async Task<UserInfo> GetUserInfo(string userId)
        {
            var builder = new UriBuilder("https://slack.com/api/users.info");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["token"] = _token;
            query["user"] = userId;
            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await _client.GetStringAsync(url);

            return JsonConvert.DeserializeObject<UserInfoResponse>(response, _jsonSettings).User;
        }
    }
}
