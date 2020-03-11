using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Slack;

namespace Doug.API
{
    public class DougApi
    {
        public string you { get; set; }
        public string channel { get; set; }

        private readonly ISlackWebApi _slack;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryService _inventoryService;

        public DougApi(ISlackWebApi slack, IUserRepository userRepository, IInventoryService inventoryService)
        {
            _slack = slack;
            _userRepository = userRepository;
            _inventoryService = inventoryService;
        }

        public void AddContext(string userId, string channelId)
        {
            you = userId;
            channel = channelId;
        }

        public List<User> users => GetUsers();
        private List<User> GetUsers()
        {
            return _userRepository.GetUsers(); // TODO Map to a lua friendly format
        }

        public Action<object> print => SendEphemeral;
        private void SendEphemeral(object input)
        {
            _slack.SendEphemeralMessage(ParseString(input), you, channel);
        }

        private string ParseString(object input)
        {
            if (input is List<object> list)
            {
                return $"[{string.Join(", ", list.Select(ParseString))}]";
            }

            return input.ToString();
        }

        public Action<string, int> give => GiveItem;
        private void GiveItem(string targetId, int itemSlot)
        {
            var target = _userRepository.GetUser(targetId);
            var user = _userRepository.GetUser(you);

            _inventoryService.Give(user, target, itemSlot, channel);
        }
    }
}
