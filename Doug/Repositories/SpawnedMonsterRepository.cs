using System;
using System.Collections.Generic;
using System.Linq;
using Doug.Models.Monsters;
using Microsoft.EntityFrameworkCore;

namespace Doug.Repositories
{
    public interface ISpawnedMonsterRepository
    {
        IEnumerable<SpawnedMonster> GetMonsters(string channel);
        SpawnedMonster GetMonster(int monsterId);
        void SpawnMonster(Monster monster, string channel);
        void RemoveMonster(int id);
        void SetAttackCooldown(int id, TimeSpan cooldown);
        void RegisterUserDamage(int id, string userId, int damage, int monsterHealth);
    }

    public class SpawnedMonsterRepository : ISpawnedMonsterRepository
    {
        private readonly DougContext _db;

        public SpawnedMonsterRepository(DougContext db)
        {
            _db = db;
        }

        public IEnumerable<SpawnedMonster> GetMonsters(string channel)
        {
            var monsters = _db.SpawnedMonsters
                .Where(monsta => monsta.Channel == channel)
                .Include(monsta => monsta.Monster)
                .ThenInclude(monsta => monsta.DropTable)
                .ThenInclude(droptable => droptable.Items)
                .Include(monsta => monsta.MonsterAttackers)
                .ThenInclude(attacker => attacker.User)
                .ToList();
            monsters.ForEach(monster => monster.LoadMonster());
            return monsters;
        }

        public SpawnedMonster GetMonster(int monsterId)
        {
            var monster = _db.SpawnedMonsters
                .Include(monsta => monsta.Monster)
                .ThenInclude(monsta => monsta.DropTable)
                .ThenInclude(droptable => droptable.Items)
                .Include(monsta => monsta.MonsterAttackers)
                .ThenInclude(attacker => attacker.User)
                .SingleOrDefault(monsta => monsta.Id == monsterId);

            monster?.LoadMonster();
            return monster;
        }

        public void SpawnMonster(Monster monster, string channel)
        {
            _db.SpawnedMonsters.Add(new SpawnedMonster{ Health = monster.Health, MonsterId = monster.Id, Channel = channel });
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
                .Include(monsta => monsta.Monster)
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

            _db.SaveChanges();
        }
    }
}
