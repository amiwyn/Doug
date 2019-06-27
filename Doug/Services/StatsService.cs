using Doug.Models;
using Doug.Repositories;

namespace Doug.Services
{
    public interface IStatsService
    {
        void AttributeStatPoint(Interaction interaction);
    }

    public class StatsService : IStatsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStatsRepository _statsRepository;

        public StatsService(IStatsRepository statsRepository, IUserRepository userRepository)
        {
            _statsRepository = statsRepository;
            _userRepository = userRepository;
        }

        public void AttributeStatPoint(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);

            if (user.FreeStatsPoints <= 0)
            {
                return; //TODO error message, send user feedback
            }

            _statsRepository.AttributeStatPoint(user.Id, interaction.Value);
        }
    }
}
