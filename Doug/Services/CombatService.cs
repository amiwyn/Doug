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
        Task<DougResponse> Steal(User user, User target, string channel);
        Task<DougResponse> Attack(User user, User target, string channel);
        Task<DougResponse> AttackMonster(User user, SpawnedMonster spawnedMonster, string channel);
        DougResponse ActivateSkill(User user, ICombatable target, string channel);
    }

    public class CombatService : ICombatService
    {
        private const int StealEnergyCost = 1;
        private const int AttackEnergyCost = 1;
        private const int KillExperienceGain = 100;

        private readonly IEventDispatcher _eventDispatcher;
        private readonly IUserRepository _userRepository;
        private readonly ISlackWebApi _slack;
        private readonly IStatsRepository _statsRepository;
        private readonly IRandomService _randomService;
        private readonly IUserService _userService;
        private readonly IChannelRepository _channelRepository;
        private readonly IMonsterRepository _monsterRepository;
        private readonly IMonsterService _monsterService;

        public CombatService(IEventDispatcher eventDispatcher, IUserRepository userRepository, ISlackWebApi slack, IStatsRepository statsRepository, IRandomService randomService, IUserService userService, IChannelRepository channelRepository, IMonsterRepository monsterRepository, IMonsterService monsterService)
        {
            _eventDispatcher = eventDispatcher;
            _userRepository = userRepository;
            _slack = slack;
            _statsRepository = statsRepository;
            _randomService = randomService;
            _userService = userService;
            _channelRepository = channelRepository;
            _monsterRepository = monsterRepository;
            _monsterService = monsterService;
        }

        public async Task<DougResponse> Steal(User user, User target, string channel)
        {
            if (user.IsStealOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateStealCooldownRemaining()));
            }

            var channelType = _channelRepository.GetChannelType(channel);
            if (channelType != ChannelType.Common && channelType != ChannelType.Pvp)
            {
                return new DougResponse(DougMessages.NotInRightChannel);
            }

            var usersInChannel = await _slack.GetUsersInChannel(channel);
            if (usersInChannel.All(usr => usr != target.Id))
            {
                return new DougResponse(DougMessages.UserIsNotInPvp);
            }

            var energy = user.Energy - StealEnergyCost;

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            _statsRepository.UpdateEnergy(user.Id, energy);
            _userRepository.SetStealCooldown(user.Id, user.GetStealCooldown());

            var userChance = _eventDispatcher.OnStealingChance(user, user.BaseStealSuccessRate());
            var targetChance = _eventDispatcher.OnGettingStolenChance(target, target.BaseOpponentStealSuccessRate());

            var rollSuccessful = _randomService.RollAgainstOpponent(userChance, targetChance);

            var amount = _eventDispatcher.OnStealingAmount(user, user.BaseStealAmount());

            if (target.Credits - amount < 0)
            {
                amount = target.Credits;
            }

            if (rollSuccessful)
            {
                _userRepository.RemoveCredits(target.Id, amount);
                _userRepository.AddCredits(user.Id, amount);

                var message = string.Format(DougMessages.StealCredits, _userService.Mention(user), amount, _userService.Mention(target));
                await _slack.BroadcastMessage(message, channel);
            }
            else
            {
                var message = string.Format(DougMessages.StealFail, _userService.Mention(user), _userService.Mention(target));
                await _slack.BroadcastMessage(message, channel);
            }

            return new DougResponse();
        }

        public async Task<DougResponse> Attack(User user, User target, string channel)
        {
            var energy = user.Energy - AttackEnergyCost;

            if (user.IsAttackOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateAttackCooldownRemaining()));
            }

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
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

            _statsRepository.UpdateEnergy(user.Id, energy);
            _userRepository.SetAttackCooldown(user.Id, user.GetAttackCooldown());

            await DealDamage(user, target, channel);

            return new DougResponse();
        }

        private async Task DealDamage(User user, User target, string channel)
        {
            var attack = user.AttackTarget(target, _eventDispatcher);

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
            var energy = user.Energy - AttackEnergyCost;
            var monster = spawnedMonster.Monster;

            if (user.IsAttackOnCooldown())
            {
                return new DougResponse(string.Format(DougMessages.CommandOnCooldown, user.CalculateAttackCooldownRemaining()));
            }

            if (energy < 0)
            {
                return new DougResponse(DougMessages.NotEnoughEnergy);
            }

            _statsRepository.UpdateEnergy(user.Id, energy);
            _userRepository.SetAttackCooldown(user.Id, user.GetAttackCooldown());

            var attack = user.AttackTarget(monster, _eventDispatcher);

            var message = attack.Status.ToMessage(_userService.Mention(user), $"*{monster.Name}*", attack.Damage);
            await _slack.BroadcastMessage(message, channel);

            _monsterRepository.RegisterUserDamage(spawnedMonster.Id, user.Id, attack.Damage);

            if (monster.IsDead())
            {
                await _monsterService.HandleMonsterDeathByUser(user, spawnedMonster, channel);
            }
            else if (!spawnedMonster.IsAttackOnCooldown())
            {
                await MonsterAttackUser(monster, user, spawnedMonster.Id, channel);
            }

            return new DougResponse();
        }

        public DougResponse ActivateSkill(User user, ICombatable target, string channel)
        {
            var skillbook = user.Loadout.GetSkill();
            return skillbook.Activate(user, target, channel);
        }

        private async Task MonsterAttackUser(Monster monster, User user, int spawnedMonsterId, string channel)
        {
            var retaliationAttack = monster.AttackTarget(user, _eventDispatcher);

            var retaliationMessage = retaliationAttack.Status.ToMessage($"*{monster.Name}*", _userService.Mention(user), retaliationAttack.Damage);
            await _slack.BroadcastMessage(retaliationMessage, channel);
            _monsterRepository.SetAttackCooldown(spawnedMonsterId, monster.GetAttackCooldown());

            if (user.IsDead())
            {
                await _userService.HandleDeath(user, channel);
            }

            _statsRepository.UpdateHealth(user.Id, user.Health);
        }
    }
}
