using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Doug.Slack
{
    public interface ISlackWebApi
    {
        Task<string> SendMessage(string text, string channelId);
        Task<UserInfo> GetUserInfo(string userId);
        Task AddReaction(string reaction, string timestamp, string channel);
        Task<List<Reaction>> GetReactions(string timestamp, string channel);
        Task SendAttachments(IEnumerable<Attachment> attachments, string channel);
        Task SendEphemeralMessage(string text, string user, string channel);
        Task SendEphemeralBlocks(IEnumerable<BlockMessage> blocks, string user, string channel);
    }

    public class SlackWebApi : ISlackWebApi
    {
        private const string PostMessageUrl = "https://slack.com/api/chat.postMessage";
        private const string UserInfoUrl = "https://slack.com/api/users.info";
        private const string ReactionAddUrl = "https://slack.com/api/reactions.add";
        private const string ReactionGetUrl = "https://slack.com/api/reactions.get";
        private const string EphemeralUrl = "https://slack.com/api/chat.postEphemeral";
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
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task<string> SendMessage(string text, string channelId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, PostMessageUrl);
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _token),
                new KeyValuePair<string, string>("text", text),
                new KeyValuePair<string, string>("channel", channelId)
            };
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
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _token),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("timestamp", timestamp)
            };
            request.Content = new FormUrlEncodedContent(keyValues);

            await _client.SendAsync(request);
        }

        public async Task<List<Reaction>> GetReactions(string timestamp, string channel)
        {
            var builder = new UriBuilder(ReactionGetUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["token"] = _token;
            query["channel"] = channel;
            query["timestamp"] = timestamp;
            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await _client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<ReactionInfoResponse>(response, _jsonSettings).Message.Reactions;
        }

        public async Task SendAttachments(IEnumerable<Attachment> attachments, string channel)
        {
            var attachmentString = JsonConvert.SerializeObject(attachments, _jsonSettings);

            var request = new HttpRequestMessage(HttpMethod.Post, PostMessageUrl);
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _token),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("attachments", attachmentString)
            };
            request.Content = new FormUrlEncodedContent(keyValues);

            await _client.SendAsync(request);
        }

        public async Task SendEphemeralMessage(string text, string user, string channel)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EphemeralUrl);
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _token),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("text", text)
            };
            request.Content = new FormUrlEncodedContent(keyValues);

            await _client.SendAsync(request);
        }

        public async Task SendEphemeralBlocks(IEnumerable<BlockMessage> blocks, string user, string channel)
        {
            var attachmentString = JsonConvert.SerializeObject(blocks, _jsonSettings);

            var request = new HttpRequestMessage(HttpMethod.Post, EphemeralUrl);
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _token),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("blocks", attachmentString)
            };
            request.Content = new FormUrlEncodedContent(keyValues);

            await _client.SendAsync(request);
        }
    }
}
