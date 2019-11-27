using System.Linq;
using System.Threading.Tasks;
using Doug.Controllers;
using Doug.Menus;
using Doug.Models.Monsters;
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
        private readonly ISpawnedMonsterRepository _spawnedMonsterRepository;
        private readonly ISlackWebApi _slack;
        private readonly ISkillService _skillService;

        public MonsterMenuService(IUserRepository userRepository, ICombatService combatService, ISpawnedMonsterRepository spawnedMonsterRepository, ISlackWebApi slack, ISkillService skillService)
        {
            _userRepository = userRepository;
            _combatService = combatService;
            _spawnedMonsterRepository = spawnedMonsterRepository;
            _slack = slack;
            _skillService = skillService;
        }

        public async Task Attack(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var monster = _spawnedMonsterRepository.GetMonster(int.Parse(interaction.Value));

            if (monster == null)
            {
                return;
            }

            var response = await _combatService.AttackMonster(user, monster, interaction.ChannelId);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);
            await UpdateMonsterAttackBlocks(monster, interaction.ResponseUrl);
        }

        public async Task Skill(Interaction interaction)
        {
            var user = _userRepository.GetUser(interaction.UserId);
            var monster = _spawnedMonsterRepository.GetMonster(int.Parse(interaction.Value));

            if (monster == null)
            {
                return;
            }

            var response = await _skillService.ActivateSkill(user, monster, interaction.ChannelId);

            await _slack.SendEphemeralMessage(response.Message, user.Id, interaction.ChannelId);
            await UpdateMonsterAttackBlocks(monster, interaction.ResponseUrl);
        }

        private async Task UpdateMonsterAttackBlocks(SpawnedMonster spawnedMonster, string url)
        {
            if (spawnedMonster.IsDead())
            {
                await _slack.DeleteInteractionMessage(url);
            }
            else
            {
                await _slack.UpdateInteractionMessage(new MonsterMenu(spawnedMonster).Blocks, url);
            }
        }

        public async Task ShowMonsters(string channel)
        {
            var monsters = _spawnedMonsterRepository.GetMonsters(channel);
            var blocks = monsters.SelectMany(monster => new MonsterMenu(monster).Blocks);
            await _slack.BroadcastBlocks(blocks, channel);
        }
    }
}
