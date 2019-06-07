using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Models;
using Hangfire;

namespace Doug.Services
{
    public interface IEventService
    {
        Task MessageReceived(MessageEvent messageEvent);
    }

    public class EventService : IEventService
    {
        private const int CoffeeRemindDelay = 25;
        private IMessageSender slack;
        private DougContext db;

        public EventService(IMessageSender messageSender, DougContext dougContext)
        {
            this.slack = messageSender;
            this.db = dougContext;
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
            slack.SendMessage("reminding", ChannelId);
        }
    }
}
