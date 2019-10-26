using System.Linq;
using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Menus;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services.MenuServices
{
    public interface IMonsterMenuService
    {
        Task Attack(Interaction interaction);
        Task Skill(Interaction interaction);
        Task ShowMonsters(string channel);
    }

    public class MonsterMenuService : IMonsterMenuService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICombatService _combatService;
        private readonly IMonsterRepository _monsterRepository;
        private readonly ISlackWebApi _slack;

        public MonsterMenuService(IUserRepository userRepository, ICombatService combatService, IMonsterRepository monsterRepository, ISlackWebApi slack)
        {
            _userRepository = userRepository;
            _combatService = combatService;
            _monsterRepository = monsterRepository;
            _slack = slack;
        }

        public async Task Attack(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var monster = _monsterRepository.GetMonster(int.Parse(interaction.Value));

            if (monster == null)
            {
                return;
            }

            var response = await _combatService.AttackMonster(user, monster, interaction.ChannelId);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);
            await _slack.UpdateInteractionMessage(new MonsterMenu(monster).Blocks, interaction.ResponseUrl);
        }

        public async Task Skill(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var monster = _monsterRepository.GetMonster(int.Parse(interaction.Value));

            if (monster == null)
            {
                return;
            }

            var response = await _combatService.ActivateSkill(user, monster, interaction.ChannelId);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);
            await _slack.UpdateInteractionMessage(new MonsterMenu(monster).Blocks, interaction.ResponseUrl);
        }

        public async Task ShowMonsters(string channel)
        {
            var monsters = _monsterRepository.GetMonsters(channel);
            var blocks = monsters.SelectMany(monster => new MonsterMenu(monster).Blocks);
            await _slack.BroadcastBlocks(blocks, channel);
        }
    }
}
