using System;
using System.ComponentModel.DataAnnotations.Schema;
using Doug.Monsters;

namespace Doug.Models
{
    public class MonsterAttacker
    {
        public int SpawnedMonsterId { get; set; }
        public string UserId { get; set; }
        public int DamageDealt { get; set; }

        [NotMapped]
        public SpawnedMonster Monster { get; set; }
        [NotMapped]
        public User User { get; set; }

        public MonsterAttacker(int spawnedMonsterId, string userId, int damageDealt)
        {
            SpawnedMonsterId = spawnedMonsterId;
            UserId = userId;
            DamageDealt = damageDealt;
        }
    }
}
