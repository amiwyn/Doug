using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Models.Monsters;
using Doug.Monsters;
using Microsoft.EntityFrameworkCore;

namespace Doug.Repositories
{
    public interface IMonsterRepository
    {
        IEnumerable<SpawnedMonster> GetMonsters(string channel);
        SpawnedMonster GetMonster(int monsterId);
        void SpawnMonster(Monster monster, string channel);
        void RemoveMonster(int id);
        void SetAttackCooldown(int id, TimeSpan cooldown);
        void RegisterUserDamage(int id, string userId, int damage, int monsterHealth);
    }

    public class MonsterRepository : IMonsterRepository
    {
        private readonly DougContext _db;
        private readonly IMonsterFactory _monsterFactory;

        public MonsterRepository(DougContext db, IMonsterFactory monsterFactory)
        {
            _db = db;
            _monsterFactory = monsterFactory;
        }

        public IEnumerable<SpawnedMonster> GetMonsters(string channel)
        {
            var monsters = _db.SpawnedMonsters
                .Where(monsta => monsta.Channel == channel)
                .Include(monsta => monsta.MonsterAttackers)
                .ThenInclude(attacker => attacker.User)
                .ToList();
            monsters.ForEach(monster => monster.LoadMonster(_monsterFactory));
            return monsters;
        }

        public SpawnedMonster GetMonster(int monsterId)
        {
            var monster = _db.SpawnedMonsters
                .Include(monsta => monsta.MonsterAttackers)
                .ThenInclude(attacker => attacker.User)
                .SingleOrDefault(monsta => monsta.Id == monsterId);

            monster?.LoadMonster(_monsterFactory);
            return monster;
        }

        public void SpawnMonster(Monster monster, string channel)
        {
            _db.SpawnedMonsters.Add(new SpawnedMonster{ Health = monster.MaxHealth, MonsterId = monster.Id, Channel = channel });
            _db.SaveChanges();
        }

        public void RemoveMonster(int id)
        {
            var monster = _db.SpawnedMonsters.Single(monsta => monsta.Id == id);
            _db.SpawnedMonsters.Remove(monster);
            _db.SaveChanges();
        }

        public void SetAttackCooldown(int id, TimeSpan cooldown)
        {
            var monster = _db.SpawnedMonsters.Single(monsta => monsta.Id == id);
            monster.AttackCooldown = DateTime.UtcNow + cooldown;
            _db.SaveChanges();
        }

        public void RegisterUserDamage(int id, string userId, int damage, int monsterHealth)
        {
            var monster = _db.SpawnedMonsters
                .Include(monsta => monsta.MonsterAttackers)
                .Single(monsta => monsta.Id == id);

            var attacker = monster.MonsterAttackers.SingleOrDefault(user => user.UserId == userId);

            if (attacker == null)
            {
                monster.MonsterAttackers.Add(new MonsterAttacker(id, userId, damage));
            }
            else
            {
                attacker.DamageDealt += damage;
            }

            monster.Health = monsterHealth;
            monster.Monster.Health = monsterHealth;

            _db.SaveChanges();
        }
    }
}
