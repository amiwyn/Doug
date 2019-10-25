using System.Linq;
using System.Threading.Tasks;
using Doug.Items;
using Doug.Models;
using Doug.Models.Combat;
using Doug.Monsters;
using Doug.Repositories;
using Doug.Slack;

namespace Doug.Services
{
    public interface ICombatService
    {
        Task<DougResponse> Attack(User user, User target, string channel);
        Task<DougResponse> AttackMonster(User user, SpawnedMonster spawnedMonster, string channel);
        Task<DougResponse> ActivateSkill(User user, ICombatable target, string channel);
        Task DealDamage(User user, Attack attack, ICombatable target, string channel);
    }

    public class CombatService : ICombatService
    {
        private const int KillExperienceGain = 100;

        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IUserService _userService;
        private readonly IChannelRepository _channelRepository;
        private readonly IMonsterRepository _monsterRepository;
        private readonly IMonsterService _monsterService;

        public CombatService(IEventDispatcher eventDispatcher, ISlackWebApi slack, IStatsRepository statsRepository, IUserService userService, IChannelRepository channelRepository, IMonsterRepository monsterRepository, IMonsterService monsterService)
        {
            _eventDispatcher = eventDispatcher;
            _slack = slack;
            _statsRepository = statsRepository;
            _userService = userService;
            _channelRepository = channelRepository;
            _monsterRepository = monsterRepository;
            _monsterService = monsterService;
        }

        public async Task<DougResponse> Attack(User user, User target, string channel)
        {
            if (user.IsAttackOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateAttackCooldownRemaining()));
            }

            var channelType = _channelRepository.GetChannelType(channel);
            if (channelType != ChannelType.Pvp)
            {
                return new DougResponse(DougMessages.NotInRightChannel);
            }

            var usersInChannel = await _slack.GetUsersInChannel(channel);
            if (usersInChannel.All(usr => usr != target.Id))
            {
                return new DougResponse(DougMessages.UserIsNotInPvp);
            }

            _statsRepository.SetAttackCooldown(user.Id, user.GetAttackCooldown());

            var attack = user.AttackTarget(target, _eventDispatcher);

            await DealDamageToUser(user, attack, target, channel);

            return new DougResponse();
        }

        private async Task DealDamageToUser(User user, Attack attack, User target, string channel)
        {
            var message = attack.Status.ToMessage(_userService.Mention(user), _userService.Mention(target), attack.Damage);

            await _slack.BroadcastMessage(message, channel);

            if (target.IsDead() && await _userService.HandleDeath(target, channel))
            {
                _eventDispatcher.OnDeathByUser(target, user);
                await _userService.AddExperience(user, KillExperienceGain, channel);
            }

            _statsRepository.UpdateHealth(target.Id, target.Health);
        }

        public async Task<DougResponse> AttackMonster(User user, SpawnedMonster spawnedMonster, string channel)
        {
            if (user.IsAttackOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateAttackCooldownRemaining()));
            }

            _statsRepository.SetAttackCooldown(user.Id, user.GetAttackCooldown());

            var attack = user.AttackTarget(spawnedMonster, _eventDispatcher);

            await DealDamageToMonster(user, attack, spawnedMonster, channel);

            return new DougResponse();
        }

        public async Task<DougResponse> ActivateSkill(User user, ICombatable target, string channel)
        {
            var skillbook = user.Loadout.GetSkill();

            if (skillbook == null)
            {
                return new DougResponse(DougMessages.SkillCannotBeActivated);
            }

            return await skillbook.Activate(user, target, channel);
        }

        public async Task DealDamage(User user, Attack attack, ICombatable target, string channel)
        {
            if (target is User targetUser)
            {
                await DealDamageToUser(user, attack, targetUser, channel);
            }
            else if (target is SpawnedMonster targetMonster)
            {
                await DealDamageToMonster(user, attack, targetMonster, channel);
            }
        }

        private async Task DealDamageToMonster(User user, Attack attack, SpawnedMonster spawnedMonster, string channel)
        {
            var monster = spawnedMonster.Monster;
            var message = attack.Status.ToMessage(_userService.Mention(user), $"*{monster.Name}*", attack.Damage);
            await _slack.BroadcastMessage(message, channel);

            _monsterRepository.RegisterUserDamage(spawnedMonster.Id, user.Id, attack.Damage, spawnedMonster.Health);

            if (monster.IsDead())
            {
                await _monsterService.HandleMonsterDeathByUser(user, spawnedMonster, channel);
            }
            else if (!spawnedMonster.IsAttackOnCooldown())
            {
                await MonsterAttackUser(spawnedMonster, user, channel);
            }
        }

        private async Task MonsterAttackUser(SpawnedMonster spawnedMonster, User user, string channel)
        {
            var monster = spawnedMonster.Monster;
            var retaliationAttack = spawnedMonster.AttackTarget(user, _eventDispatcher);

            var retaliationMessage = retaliationAttack.Status.ToMessage($"*{monster.Name}*", _userService.Mention(user), retaliationAttack.Damage);
            await _slack.BroadcastMessage(retaliationMessage, channel);
            _monsterRepository.SetAttackCooldown(spawnedMonster.Id, monster.GetAttackCooldown());

            if (user.IsDead())
            {
                await _userService.HandleDeath(user, channel);
            }

            _statsRepository.UpdateHealth(user.Id, user.Health);
        }
    }
}
