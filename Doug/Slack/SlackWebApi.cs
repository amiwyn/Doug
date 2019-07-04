using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Doug.Menus.Blocks;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Doug.Slack
{
    public interface ISlackWebApi
    {
        Task<string> BroadcastMessage(string text, string channelId);
        Task<UserInfo> GetUserInfo(string userId);
        Task AddReaction(string reaction, string timestamp, string channel);
        Task<List<Reaction>> GetReactions(string timestamp, string channel);
        Task SendAttachments(IEnumerable<Attachment> attachments, string channel);
        Task SendEphemeralMessage(string text, string user, string channel);
        Task SendEphemeralBlocks(IEnumerable<Block> blocks, string user, string channel);
        Task UpdateInteractionMessage(IEnumerable<Block> blocks, string url);
        Task KickUser(string user, string channel);
        Task InviteUser(string user, string channel);
        Task<List<string>> GetUsersInChannel(string channel);
    }

    public class SlackWebApi : ISlackWebApi
    {
        private const string PostMessageUrl = "https://slack.com/api/chat.postMessage";
        private const string UserInfoUrl = "https://slack.com/api/users.info";
        private const string ReactionAddUrl = "https://slack.com/api/reactions.add";
        private const string ReactionGetUrl = "https://slack.com/api/reactions.get";
        private const string EphemeralUrl = "https://slack.com/api/chat.postEphemeral";
        private const string KickUrl = "https://slack.com/api/conversations.kick";
        private const string InviteUrl = "https://slack.com/api/conversations.invite";
        private const string ChannelInfoUrl = "https://slack.com/api/conversations.members";


        private readonly HttpClient _client;
        private readonly string _botToken;
        private readonly string _userToken;
        private readonly JsonSerializerSettings _jsonSettings;

        public SlackWebApi(HttpClient client, IChannelRepository channelRepository)
        {
            _client = client;
            channelRepository.GetAccessTokens(out _botToken, out _userToken);

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

        public async Task<string> BroadcastMessage(string text, string channelId)
        {
            var keyValues = CreateBaseRequestPayload(channelId);
            keyValues.Add(new KeyValuePair<string, string>("text", text));

            var body = await PostToUrl(PostMessageUrl, keyValues);

            return JsonConvert.DeserializeObject<MessageResponse>(body, _jsonSettings).Ts;
        }

        private List<KeyValuePair<string, string>> CreateBaseRequestPayload(string channel)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _botToken),
                new KeyValuePair<string, string>("channel", channel)
            };
        }

        private async Task PostToUrlWithoutResponse(string url, List<KeyValuePair<string, string>> payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url) {Content = new FormUrlEncodedContent(payload)};
            await _client.SendAsync(request);
        }

        private async Task<string> PostToUrl(string url, List<KeyValuePair<string, string>> payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url) {Content = new FormUrlEncodedContent(payload)};
            var response = await _client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<UserInfo> GetUserInfo(string userId)
        {
            var builder = new UriBuilder(UserInfoUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["token"] = _botToken;
            query["user"] = userId;
            builder.Query = query.ToString();
            string url = builder.ToString();

            var response = await _client.GetStringAsync(url);

            return JsonConvert.DeserializeObject<UserInfoResponse>(response, _jsonSettings).User;
        }

        public async Task AddReaction(string name, string timestamp, string channel)
        {
            var keyValues = CreateBaseRequestPayload(channel);
            keyValues.Add(new KeyValuePair<string, string>("name", name));
            keyValues.Add(new KeyValuePair<string, string>("timestamp", timestamp));

            await PostToUrlWithoutResponse(ReactionAddUrl, keyValues);
        }

        public async Task<List<Reaction>> GetReactions(string timestamp, string channel)
        {
            var builder = new UriBuilder(ReactionGetUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["token"] = _botToken;
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

            var keyValues = CreateBaseRequestPayload(channel);
            keyValues.Add(new KeyValuePair<string, string>("attachments", attachmentString));

            await PostToUrlWithoutResponse(PostMessageUrl, keyValues);
        }

        public async Task SendEphemeralMessage(string text, string user, string channel)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var keyValues = CreateBaseRequestPayload(channel);
            keyValues.Add(new KeyValuePair<string, string>("user", user));
            keyValues.Add(new KeyValuePair<string, string>("text", text));

            await PostToUrlWithoutResponse(EphemeralUrl, keyValues);
        }

        public async Task SendEphemeralBlocks(IEnumerable<Block> blocks, string user, string channel)
        {
            var blocksString = JsonConvert.SerializeObject(blocks, _jsonSettings);

            var keyValues = CreateBaseRequestPayload(channel);
            keyValues.Add(new KeyValuePair<string, string>("user", user));
            keyValues.Add(new KeyValuePair<string, string>("blocks", blocksString));

            await PostToUrlWithoutResponse(EphemeralUrl, keyValues);
        }

        public async Task UpdateInteractionMessage(IEnumerable<Block> blocks, string url)
        {
            var updatedMessage = new
            {
                ReplaceOriginal = "true",
                Blocks = blocks
            };

            var content = new StringContent(JsonConvert.SerializeObject(updatedMessage, _jsonSettings), Encoding.UTF8, "application/json");
            await _client.PostAsync(url, content);
        }

        public async Task KickUser(string user, string channel)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _userToken),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("user", user)
            };

            await PostToUrlWithoutResponse(KickUrl, keyValues);
        }

        public async Task InviteUser(string user, string channel)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", _userToken),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("users", user)
            };

            await PostToUrlWithoutResponse(InviteUrl, keyValues);
        }

        public async Task<List<string>> GetUsersInChannel(string channel)
        {
            var keyValues = CreateBaseRequestPayload(channel);

            var response = await PostToUrl(ChannelInfoUrl, keyValues);

            return JsonConvert.DeserializeObject<ChannelMembersResponse>(response, _jsonSettings).Members;
        }
    }
}
