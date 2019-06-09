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
        Task<string> SendMessage(string text, string channelId);
        Task<UserInfo> GetUserInfo(string userId);
        Task AddReaction(string reaction, string timestamp, string channel);
    }

    public class SlackWebApi : ISlackWebApi
    {
        private const string PostMessageUrl = "https://slack.com/api/chat.postMessage";
        private const string UserInfoUrl = "https://slack.com/api/users.info";
        private const string ReactionAddUrl = "https://slack.com/api/reactions.add";
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

        public async Task<string> SendMessage(string text, string channelId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, PostMessageUrl);
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("token", _token));
            keyValues.Add(new KeyValuePair<string, string>("text", text));
            keyValues.Add(new KeyValuePair<string, string>("channel", channelId));
            request.Content = new FormUrlEncodedContent(keyValues);

            var response = await _client.SendAsync(request);

            var body = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MessageResponse>(body, _jsonSettings).Ts;
        }

        public async Task<UserInfo> GetUserInfo(string userId)
        {
            var builder = new UriBuilder(UserInfoUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["token"] = _token;
            query["user"] = userId;
            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await _client.GetStringAsync(url);

            return JsonConvert.DeserializeObject<UserInfoResponse>(response, _jsonSettings).User;
        }

        public async Task AddReaction(string name, string timestamp, string channel)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ReactionAddUrl);
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("token", _token));
            keyValues.Add(new KeyValuePair<string, string>("name", name));
            keyValues.Add(new KeyValuePair<string, string>("channel", channel));
            keyValues.Add(new KeyValuePair<string, string>("timestamp", timestamp));
            request.Content = new FormUrlEncodedContent(keyValues);

            await _client.SendAsync(request);
        }
    }
}
