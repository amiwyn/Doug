using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    public interface ICoffeeService
    {
        void LaunchCoffeeBreak(string channelId);
        void CountParrot(string userId, string channelId, DateTime currentTime);
    }

    public class CoffeeService : ICoffeeService
    {
        private const int CoffeeRemindDelaySeconds = 25;
        private const int CoffeeBreakDurationMinutes = 15;
        private const int MorningBreak = 13;
        private const int AfternoonBreak = 24;
        private const int Tolerance = 30;
        private const int CoffeeBreakAward = 10;

        private readonly ISlackWebApi _slack;
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IUserRepository _userRepository;

        public CoffeeService(ISlackWebApi slackWebApi, ICoffeeRepository coffeeRepository, IChannelRepository channelRepository, IBackgroundJobClient backgroundJobClient, IUserRepository userRepository)
        {
            _slack = slackWebApi;
            _coffeeRepository = coffeeRepository;
            _channelRepository = channelRepository;
            _backgroundJobClient = backgroundJobClient;
            _userRepository = userRepository;
        }

        public void CountParrot(string userId, string channelId, DateTime currentTime)
        {
            if (!Utils.IsInTimespan(currentTime, TimeSpan.FromHours(MorningBreak), Tolerance) &&
                !Utils.IsInTimespan(currentTime, TimeSpan.FromHours(AfternoonBreak), Tolerance))
            {
                return;
            }

            if (_coffeeRepository.IsCoffeeBreak())
            {
                return;
            }

            _coffeeRepository.ConfirmUserReady(userId);

            if (_coffeeRepository.GetMissingParticipants().Count == 0)
            {
                LaunchCoffeeBreak(channelId);
                return;
            }

            var remindJob = _channelRepository.GetRemindJob();

            if (!string.IsNullOrEmpty(remindJob))
            {
                _backgroundJobClient.Delete(remindJob);
            }

            var newRemindJob = _backgroundJobClient.Schedule(() => CoffeeRemind(channelId), TimeSpan.FromSeconds(CoffeeRemindDelaySeconds));

            _channelRepository.SetRemindJob(newRemindJob);
        }

        public void CoffeeRemind(string ChannelId)
        {
            var readyParticipants = _coffeeRepository.GetReadyParticipants();
            var missingParticipants = _coffeeRepository.GetMissingParticipants();

            var total = missingParticipants.Count + readyParticipants.Count;

            var userMentionList = missingParticipants
                .Select(userId => Utils.UserMention(userId))
                .Aggregate(string.Empty, (userId, acc) => acc + " " + userId);

            _slack.SendMessage(string.Format(DougMessages.Remind, readyParticipants.Count, total, userMentionList), ChannelId);
        }

        public void LaunchCoffeeBreak(string channelId)
        {
            if (_coffeeRepository.IsCoffeeBreak())
            {
                return;
            }

            _slack.SendMessage(DougMessages.CoffeeStart, channelId);

            _backgroundJobClient.Schedule(() => EndCoffee(channelId), TimeSpan.FromMinutes(CoffeeBreakDurationMinutes));
        }

        public void EndCoffee(string channelId)
        {
            var participants = _coffeeRepository.GetReadyParticipants();

            foreach (var participant in participants)
            {
                _userRepository.AddCredits(participant, CoffeeBreakAward);
            }

            _coffeeRepository.ResetRoster();

            _slack.SendMessage(DougMessages.BackToWork, channelId);
        }
    }
}
