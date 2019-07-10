using Doug.Repositories;
using Doug.Slack;
using Hangfire;
using System;
using System.Linq;
using Doug.Items;

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
        private const int MorningBreak = 9 + 4;
        private const int AfternoonBreak = 14 + 4;
        private const int Tolerance = 30;
        private const int CoffeeBreakAward = 10;
        private const int CoffeeExperienceAward = 300;

        private readonly ISlackWebApi _slack;
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUserService _userService;
        private readonly IStatsRepository _statsRepository;

        public CoffeeService(ISlackWebApi slackWebApi, ICoffeeRepository coffeeRepository, IBackgroundJobClient backgroundJobClient, IUserRepository userRepository, IInventoryRepository inventoryRepository, IUserService userService, IStatsRepository statsRepository)
        {
            _slack = slackWebApi;
            _coffeeRepository = coffeeRepository;
            _backgroundJobClient = backgroundJobClient;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
            _userService = userService;
            _statsRepository = statsRepository;
        }

        public void CountParrot(string userId, string channelId, DateTime currentTime)
        {
            if (!IsInTimespan(currentTime, TimeSpan.FromHours(MorningBreak), Tolerance) &&
                !IsInTimespan(currentTime, TimeSpan.FromHours(AfternoonBreak), Tolerance))
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

            var remindJob = _coffeeRepository.GetRemindJob();

            if (!string.IsNullOrEmpty(remindJob))
            {
                _backgroundJobClient.Delete(remindJob);
            }

            var newRemindJob = _backgroundJobClient.Schedule(() => CoffeeRemind(channelId), TimeSpan.FromSeconds(CoffeeRemindDelaySeconds));

            _coffeeRepository.SetRemindJob(newRemindJob);
        }

        private static bool IsInTimespan(DateTime currentTime, TimeSpan targetTime, int tolerance)
        {
            var start = targetTime.Subtract(TimeSpan.FromMinutes(tolerance));
            var end = targetTime.Add(TimeSpan.FromMinutes(tolerance));

            return (currentTime.TimeOfDay > start) && (currentTime.TimeOfDay < end);
        }

        public void CoffeeRemind(string channelId)
        {
            var readyParticipants = _coffeeRepository.GetReadyParticipants();
            var missingParticipants = _coffeeRepository.GetMissingParticipants();

            var total = missingParticipants.Count + readyParticipants.Count;

            var userMentionList = missingParticipants
                .Select(_userService.Mention)
                .Aggregate(string.Empty, (userId, acc) => acc + " " + userId);


            var message = DougMessages.Remind;
            if (readyParticipants.Count == 6 && total == 9)
            {
                message = DougMessages.Remind69;
            }

            _slack.BroadcastMessage(string.Format(message, readyParticipants.Count, total, userMentionList), channelId);
        }

        public void LaunchCoffeeBreak(string channelId)
        {
            _coffeeRepository.StartCoffeeBreak();

            _slack.BroadcastMessage(DougMessages.CoffeeStart, channelId);

            _backgroundJobClient.Schedule(() => EndCoffee(channelId), TimeSpan.FromMinutes(CoffeeBreakDurationMinutes));
        }

        public void EndCoffee(string channelId)
        {
            var participants = _coffeeRepository.GetReadyParticipants().ToList();
            var participantsId = participants.Select(user => user.Id).ToList();

            _statsRepository.RegenerateUsersHealth(participantsId);
            _userRepository.AddCreditsToUsers(participantsId, CoffeeBreakAward);
            _inventoryRepository.AddItemToUsers(participants, ItemFactory.CoffeeCup);

            _userService.AddBulkExperience(participants, CoffeeExperienceAward, channelId).Wait();

            _coffeeRepository.ResetRoster();
            _coffeeRepository.EndCoffeeBreak();

            _slack.BroadcastMessage(DougMessages.BackToWork, channelId);
        }
    }
}
