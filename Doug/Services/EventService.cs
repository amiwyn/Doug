using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Models;
using Doug.Slack;
using Hangfire;

namespace Doug.Services
{
    public interface IEventService
    {
        Task MessageReceived(MessageEvent messageEvent);
    }

    public class EventService : IEventService
    {
        private const int _CoffeeRemindDelay = 25;
        private readonly ISlackWebApi _slack;
        private readonly DougContext _db;

        public EventService(ISlackWebApi messageSender, DougContext dougContext)
        {
            _slack = messageSender;
            _db = dougContext;
        }

        public async Task MessageReceived(MessageEvent message)
        {
            //if (message.IsValidCoffeeParrot())
            //{
            //    Channel channel = db.Channel.Single();

            //    if (!string.IsNullOrEmpty(channel.CoffeeRemindJobId))
            //    {
            //        BackgroundJob.Delete(channel.CoffeeRemindJobId);
            //    }

            //    channel.CoffeeRemindJobId = BackgroundJob.Schedule(() => CoffeeRemind(message.Channel), TimeSpan.FromSeconds(CoffeeRemindDelay));
            //    await db.SaveChangesAsync();
            //}
        }

        public void CoffeeRemind(string ChannelId)
        {
            _slack.SendMessage("reminding", ChannelId);
        }
    }
}
