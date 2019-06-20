using Doug.Items;
using Doug.Models;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Commands
{
    public interface ICombatCommands
    {
        DougResponse Steal(Command command);
    }

    public class CombatCommands : ICombatCommands
    {
        private const int StealEnergyCost = 1;
        private static readonly DougResponse NoResponse = new DougResponse();

        private readonly IItemEventDispatcher _itemEventDispatcher;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;

        public CombatCommands(IItemEventDispatcher itemEventDispatcher, IUserRepository userRepository, ISlackWebApi slack, IStatsRepository statsRepository)
        {
            _itemEventDispatcher = itemEventDispatcher;
            _userRepository = userRepository;
            _slack = slack;
            _statsRepository = statsRepository;
        }

        public DougResponse Steal(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());

            var energy = user.Energy - StealEnergyCost;

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            _statsRepository.UpdateEnergy(command.UserId, energy);

            var userChance = _itemEventDispatcher.OnStealingChance(user, user.CalculateBaseStealSuccessRate());
            var targetChance = _itemEventDispatcher.OnGettingStolenChance(target, target.CalculateBaseOpponentStealSuccessRate());

            var rollSuccessful = Utils.RollAgainstOpponent(userChance, targetChance);

            var amount = _itemEventDispatcher.OnStealingAmount(user, user.CalculateBaseStealAmount());

            if (target.Credits - amount < 0)
            {
                return new DougResponse(DougMessages.TargetNoMoney);
            }

            if (rollSuccessful)
            {
                _userRepository.RemoveCredits(target.Id, amount);
                _userRepository.AddCredits(command.UserId, amount);

                var message = string.Format(DougMessages.StealCredits, Utils.UserMention(command.UserId), amount, Utils.UserMention(target.Id));
                _slack.SendMessage(message, command.ChannelId);
            }
            else
            {
                var message = string.Format(DougMessages.StealFail, Utils.UserMention(command.UserId), Utils.UserMention(target.Id));
                _slack.SendMessage(message, command.ChannelId);
            }

            return NoResponse;
        }
    }
}
