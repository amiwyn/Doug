using System;
using System.ComponentModel.DataAnnotations.Schema;
using Doug.Monsters;

namespace Doug.Models
{
    public class SpawnedMonster
    {
        public int Id { get; set; }
        public string MonsterId { get; set; }
        public string Channel { get; set; }
        public int Health { get; set; }
        public DateTime AttackCooldown { get; set; }

        [NotMapped]
        public Monster Monster { get; set; }

        public void LoadMonster(IMonsterFactory monsterFactory)
        {
            Monster = monsterFactory.CreateMonster(MonsterId);
            Monster.Health = Health;
        }

        public bool IsAttackOnCooldown()
        {
            return DateTime.UtcNow <= AttackCooldown;
        }
    }
}
