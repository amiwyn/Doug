using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Slack;
using Microsoft.Extensions.DependencyInjection;

namespace Doug.API
{
    public class DougApi
    {
        public string you { get; set; }
        public string channel { get; set; }

        public DougApi(string user, string channel)
        {
            this.you = user;
            this.channel = channel;
        }

        public List<User> users => GetUsers();
        private static List<User> GetUsers()
        {
            var services = Startup.CreateStaticDougContext().BuildServiceProvider();
            var userRepository = services.GetService<IUserRepository>();
            return userRepository.GetUsers();
        }

        public Action<object> print => SendEphemeral;
        private void SendEphemeral(object input)
        {
            var services = Startup.CreateStaticDougContext().BuildServiceProvider();
            var slack = services.GetService<ISlackWebApi>();

            slack.SendEphemeralMessage(ParseString(input), you, channel);
        }

        private string ParseString(object input)
        {
            if (input is List<object> list)
            {
                return $"[{string.Join(", ", list.Select(ParseString))}]";
            }

            return input.ToString();
        }

        public Action give => GiveItem;

        private void GiveItem()
        {
            var services = Startup.CreateStaticDougContext().BuildServiceProvider();
            var slack = services.GetService<ISlackWebApi>();
        }
    }
}
