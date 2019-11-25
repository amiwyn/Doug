using System.Threading.Tasks;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;
using Doug.Services;
using Doug.Services.MenuServices;

namespace Doug.Commands
{
    public interface ICombatCommands
    {
        Task<DougResponse> Attack(Command command);
        Task<DougResponse> Revolution(Command command);
        Task<DougResponse> ListMonsters(Command command);
        Task<DougResponse> Skill(Command command);
        Task<DougResponse> PartyInvite(Command command);
        DougResponse LeaveParty(Command command);
    }

    public class CombatCommands : ICombatCommands
    {
        private readonly IUserRepository _userRepository;
        private readonly ICombatService _combatService;
        private readonly IGovernmentService _governmentService;
        private readonly IMonsterMenuService _monsterMenuService;
        private readonly IPartyService _partyService;
        private readonly ISkillService _skillService;

        public CombatCommands(IUserRepository userRepository, ICombatService combatService, IGovernmentService governmentService, IMonsterMenuService monsterMenuService, IPartyService partyService, ISkillService skillService)
        {
            _userRepository = userRepository;
            _combatService = combatService;
            _governmentService = governmentService;
            _monsterMenuService = monsterMenuService;
            _partyService = partyService;
            _skillService = skillService;
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

        public async Task<DougResponse> ListMonsters(Command command)
        {
            await _monsterMenuService.ShowMonsters(command.ChannelId);
            return new DougResponse();
        }

        public async Task<DougResponse> Skill(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            User target = null;
            if (command.IsUserArgument())
            {
                target = _userRepository.GetUser(command.GetTargetUserId());
            }

            return await _skillService.ActivateSkill(user, target, command.ChannelId);
        }

        public async Task<DougResponse> PartyInvite(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);
            var target = _userRepository.GetUser(command.GetTargetUserId());

            return await _partyService.SendInvite(target, user, command.ChannelId);
        }

        public DougResponse LeaveParty(Command command)
        {
            var user = _userRepository.GetUser(command.UserId);

            return _partyService.LeaveParty(user);
        }
    }
}
