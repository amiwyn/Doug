using Doug.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    public interface ICoffeeBreakService
    {
        void LaunchCoffeeBreak(string channelId);
        Task CountParrot(string userId, string channelId);
    }

    public class CoffeeBreakService : ICoffeeBreakService
    {
        private const int CoffeeRemindDelaySeconds = 25;

        private readonly ISlackWebApi _slack;

        public Task CountParrot(string userId, string channelId)
        {
            throw new NotImplementedException();
            //Channel channel = db.Channel.Single();

            //if (!string.IsNullOrEmpty(channel.CoffeeRemindJobId))
            //{
            //    BackgroundJob.Delete(channel.CoffeeRemindJobId);
            //}

            //channel.CoffeeRemindJobId = BackgroundJob.Schedule(() => CoffeeRemind(message.Channel), TimeSpan.FromSeconds(CoffeeRemindDelaySeconds));
            //await db.SaveChangesAsync();
        }

        public void LaunchCoffeeBreak(string channelId)
        {
            throw new NotImplementedException();
            //_slack.SendMessage(DougMessages.CoffeeStart, command.ChannelId);
        }

        private void CoffeeRemind(string ChannelId)
        {
            _slack.SendMessage("reminding", ChannelId);
        }
    }
}
