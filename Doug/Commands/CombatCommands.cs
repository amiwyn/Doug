using System.Threading.Tasks;
using Doug.Models;
using Doug.Repositories;
using Doug.Services;

namespace Doug.Commands
{
    public interface ICombatCommands
    {
        Task<DougResponse> Steal(Command command);
        Task<DougResponse> Attack(Command command);
        Task<DougResponse> Revolution(Command command);
    }

    public class CombatCommands : ICombatCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ICombatService _combatService;
        private readonly IGovernmentService _governmentService;

        public CombatCommands(IUserRepository userRepository, ICombatService combatService, IGovernmentService governmentService)
        {
            _userRepository = userRepository;
            _combatService = combatService;
            _governmentService = governmentService;
        }

        public async Task<DougResponse> Steal(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());

            return await _combatService.Steal(user, target, command.ChannelId);
        }

        public async Task<DougResponse> Attack(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());
            
            return await _combatService.Attack(user, target, command.ChannelId);
        }

        public async Task<DougResponse> Revolution(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            return await _governmentService.StartRevolutionVote(user, command.ChannelId);
        }
    }
}
